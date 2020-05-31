using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.TeamFoundation.Build.WebApi;
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

        public async Task<ItemOperationResult> CreateItemAsync(Item newItem)
        {
            ItemOperationResult result = new ItemOperationResult();

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

        public async Task<ItemOperationResult> DeleteItemAsync(Item item)
        {
            ItemOperationResult result = new ItemOperationResult();

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

        public async Task<ItemOperationResult> GetByExternalIDAsync(string Id)
        {
            ItemOperationResult result = new ItemOperationResult();

            var wiql = new Wiql() { Query = $"Select [Id]  From WorkItems Where [System.TeamProject] = '{_azureDevopsConfiguration.ProjectName}' And Title contains '[{Id}]'" };

            using (var connection = new VssConnection(_azureDevopsConfiguration.Uri, new VssBasicCredential(string.Empty, _azureDevopsConfiguration.Token)))
            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                var remoteResult = await client.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var workItemIds = remoteResult.WorkItems.Select(item => item.Id).ToArray();

                if (workItemIds.Any())
                {
                    var fields = new[] { "System.Id", "System.Title", "System.Description" };
                    var workItems = await client.GetWorkItemsAsync(workItemIds, fields, remoteResult.AsOf).ConfigureAwait(false);

                    var transformedWorkItem = workItems.FirstOrDefault()?.Fields.ToDictionary(p => p.Key.StartsWith("System.") ? p.Key.Substring(7) : p.Key, v => v.Value);

                    result.Item = _mapper.Map<Item>(transformedWorkItem);
                    result.HasSucceed = result.Item != null;
                }
            }

            return result;
        }

        public async Task<ItemOperationResult> UpdateItemAsync(Item item)
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
                    var t = _mapper.Map<JsonPatchDocument>(mergedItem);

                    int.TryParse(mergedItem.ID, out int azureItemID);
                    var result = await client.UpdateWorkItemAsync(t, azureItemID);
                }


                return new ItemOperationResult(mergedItem, true);
            }
            else
            {
                return new ItemOperationResult();
            }
        }
    }
}
