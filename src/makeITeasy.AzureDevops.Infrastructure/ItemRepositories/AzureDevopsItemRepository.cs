using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AzureDevopsItemRepository> _logger;

        public AzureDevopsItemRepository(AzureDevopsConfiguration azureDevopsConfiguration, IMapper mapper, ILogger<AzureDevopsItemRepository> logger)
        {
            _azureDevopsConfiguration = azureDevopsConfiguration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<Item>> CreateItemAsync(Item newItem)
        {
            OperationResult<Item> result = new OperationResult<Item>();

            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Title", Value = $"[{newItem.ID}]-{newItem.Title}" });
            patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Description", Value = "" });

            WorkItem remoteResult = await ProcessWorkItemTrackingHttpAction<WorkItem>(x => x.CreateWorkItemAsync(patchDocument, _azureDevopsConfiguration.ProjectName, "Feature"));

            result.Item = _mapper.Map<Item>(remoteResult);
            result.HasSucceed = remoteResult != null;

            _logger.LogInformation($"Creation of item {newItem} => {result.HasSucceed}");

            return result;
        }

        public async Task<OperationResult<Item>> DeleteItemAsync(Item item)
        {
            OperationResult<Item> result = new OperationResult<Item>();

            var getAzureItem = await GetByExternalIDAsync(item.ID);

            if (!getAzureItem.HasSucceed)
            {
                int.TryParse(getAzureItem.Item.ID, out int azureItemID);

                var _ = await ProcessWorkItemTrackingHttpAction(x => x.DeleteWorkItemAsync(_azureDevopsConfiguration.ProjectName, azureItemID));
            }
            else
            {
                result.ErrorMessage = $"Couldn't find item with id :{item.ID}";
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
            var wiql = new Wiql() { Query = $"Select [Id] From WorkItems Where [System.TeamProject] = '{_azureDevopsConfiguration.ProjectName}' And Title contains '[{Id}]'" };

            var remoteResult = await ProcessWorkItemTrackingHttpAction(x => x.QueryByWiqlAsync(wiql));

            var workItemIds = remoteResult.WorkItems.Select(item => item.Id).ToArray();

            if (workItemIds.Any())
            {
                var fields = new[] { "System.Id", "System.Title", "System.Description" };
                
                var workItems = await ProcessWorkItemTrackingHttpAction(x => x.GetWorkItemsAsync(workItemIds, fields));

                return workItems.FirstOrDefault();
            }

            return null;
        }

        public async Task<OperationResult<Item>> UpdateItemAsync(Item item)
        {
            var getAzureItem = await GetByExternalIDAsync(item.ID);

            if (!getAzureItem.HasSucceed)
            {
                getAzureItem = await CreateItemAsync(item);
            }

            if (getAzureItem.HasSucceed)
            {
                Item mergedItem = _mapper.Map<Item, Item>(item, getAzureItem.Item);
                var azureWorkItem = _mapper.Map<JsonPatchDocument>(mergedItem);

                int.TryParse(mergedItem.ID, out int azureItemID);

                WorkItem result = await ProcessWorkItemTrackingHttpAction(x => x.UpdateWorkItemAsync(azureWorkItem, azureItemID));

                var transformedWorkItem = result?.Fields.ToDictionary(p => p.Key.StartsWith("System.") ? p.Key.Substring(7) : p.Key, v => v.Value);

                if (result != null && transformedWorkItem != null)
                {
                    transformedWorkItem["ID"] = item.ID;

                    return new OperationResult<Item>(_mapper.Map<Item>(transformedWorkItem), true);
                }
            }

            return new OperationResult<Item>();
        }

        public async Task<bool> UpdateItemWithSourceControlInfoAsync(Item item, GitCommitInfo commitInfo)
        {
            WorkItem azureWorkItem = await InternalGetByExternalIDAsync(item.ID);

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

            WorkItem remoteResult = await ProcessWorkItemTrackingHttpAction(x => x.UpdateWorkItemAsync(patchDocument, azureWorkItem.Id.GetValueOrDefault()));

            return remoteResult != null;
        }

        public async Task<R> ProcessWorkItemTrackingHttpAction<R>(Func<WorkItemTrackingHttpClient, Task<R>> action)
        {
            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            {
                using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
                {
                    return await action(client);
                }
            }
        }
    }
}
