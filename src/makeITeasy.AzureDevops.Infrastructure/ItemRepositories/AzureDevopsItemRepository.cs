using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsItemRepository : IItemRepository
    {
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;
        private readonly IMapper _mapper;

        public AzureDevopsItemRepository(AzureDevopsConfiguration azureDevopsConfiguration, IMapper mapper)
        {
            _azureDevopsConfiguration = azureDevopsConfiguration;
            _mapper = mapper;
        }

        public async Task<OperationResult<Item>> CreateItemAsync(Item newItem)
        {
            OperationResult<Item> result = new OperationResult<Item>();

            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Title", Value = $"[{newItem.ID}]-{newItem.Title}" });
            //TODO : Add Desc + item URL

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                    var remoteResult = await workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, _azureDevopsConfiguration.ProjectName, "Task");

                    result.Item = _mapper.Map<Item>(remoteResult);
                    result.HasSucceed = remoteResult != null;
            }

            return result;
        }

        public async Task<OperationResult<Item>> DeleteItemAsync(Item item)
        {
            OperationResult<Item> result = new OperationResult<Item>();

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                var getAzureItem = await GetByExternalIDAsync(item.ID);

                if (!getAzureItem.HasSucceed)
                {
                    int.TryParse(getAzureItem.Item.ID, out int azureItemID);
                    var remoteResult = await workItemTrackingHttpClient.DeleteWorkItemAsync(_azureDevopsConfiguration.ProjectName, azureItemID);

                    int? x = remoteResult.Code;
                }
                else
                {
                    result.ErrorMessage = $"Couldn't find item with id :{item.ID}";
                }
            }

            return result;
        }

        public async Task<OperationResult<Item>> GetByExternalIDAsync(string Id)
        {
            OperationResult<Item> result = new OperationResult<Item>();

            WorkItem azureWorkItem = await InternalGetByExternalIDAsync(Id);

            var transformedWorkItem = azureWorkItem?.Fields.ToDictionary(p => p.Key.StartsWith("System.") ? p.Key.Substring(7) : p.Key, v => v.Value);

            result.Item = _mapper.Map<Item>(transformedWorkItem);
            result.HasSucceed = result.Item != null;

            return result;
        }

        private async Task<WorkItem> InternalGetByExternalIDAsync(string Id)
        {
            var wiql = new Wiql() { Query = $"Select [Id]  From WorkItems Where [System.TeamProject] = '{_azureDevopsConfiguration.ProjectName}' And Title contains '[{Id}]'" };

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                var remoteResult = await client.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var workItemIds = remoteResult.WorkItems.Select(item => item.Id).ToArray();

                if (workItemIds.Any())
                {
                    var fields = new[] { "System.Id", "System.Title", "System.Description" };
                    var workItems = await client.GetWorkItemsAsync(workItemIds, fields).ConfigureAwait(false);

                    return workItems.FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<OperationResult<Item>> UpdateItemAsync(Item item)
        {
            var getAzureItem = await GetByExternalIDAsync(item.ID);

            if(!getAzureItem.HasSucceed)
            {
                getAzureItem = await CreateItemAsync(item);
            }

            if (getAzureItem.HasSucceed)
            {
                Item mergedItem = _mapper.Map<Item, Item>(item, getAzureItem.Item);

                using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    var azureWorkItem = _mapper.Map<JsonPatchDocument>(mergedItem);

                    int.TryParse(mergedItem.ID, out int azureItemID);

                    WorkItem result = await client.UpdateWorkItemAsync(azureWorkItem, azureItemID);

                    var transformedWorkItem = result?.Fields.ToDictionary(p => p.Key.StartsWith("System.") ? p.Key.Substring(7) : p.Key, v => v.Value);
                    transformedWorkItem["ID"] = item.ID;

                    if (result != null)
                    {
                        return new OperationResult<Item>(_mapper.Map<Item>(transformedWorkItem), true);
                    }
                }
            }

            return new OperationResult<Item>();
        }

        public async Task<bool> UpdateItemWithSourceControlInfoAsync(Item item, GitCommitInfo commitInfo)
        {
            WorkItem azureWorkItem = await InternalGetByExternalIDAsync(item.ID);

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {

                JsonPatchDocument patchDocument = new JsonPatchDocument();

                patchDocument.Add(
                    new JsonPatchOperation()
                    {
                        Operation = Operation.Add,
                        Path = "/relations/-",
                        Value = new
                        {
                            rel = "ArtifactLink",
                            url = $"vstfs:///Git/Ref/{commitInfo.ProjectID}%2F{commitInfo.RepositoryID}%2FGBfeatures%2F{commitInfo.BranchName}",
                            attributes = new
                            {
                                name = "Branch",
                                comment = "Comment"
                            },
                        }
                    });

                int.TryParse(item.ID, out int azureItemID);

                var remoteResult = await client.UpdateWorkItemAsync(patchDocument, azureWorkItem.Id.GetValueOrDefault());
            }

            return true;
        }
    }
}
