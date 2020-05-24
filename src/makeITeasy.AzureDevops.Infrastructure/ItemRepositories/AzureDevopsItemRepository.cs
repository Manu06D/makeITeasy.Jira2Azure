using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsItemRepository : IItemRepository
    {
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;
        private readonly IMapper _mapper;
        private Uri repoURL;
        private String token;
        private String projectName;
        private VssConnection connection;

        public AzureDevopsItemRepository(AzureDevopsConfiguration azureDevopsConfiguration, IMapper mapper)
        {
            _azureDevopsConfiguration = azureDevopsConfiguration;
            _mapper = mapper;
            repoURL = new Uri(azureDevopsConfiguration.Uri);
            token = azureDevopsConfiguration.Token;
            projectName = azureDevopsConfiguration.ProjectName;
            connection = new VssConnection(repoURL, new VssBasicCredential(string.Empty, token));
        }
            public bool CreateItem(Item newItem)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(new JsonPatchOperation() {Operation = Operation.Add, Path = "/fields/System.Title", Value = $"{newItem.ID}{newItem.Title}"});

            var workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                //var result = workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, projectName, "Task").Result;

                return true;
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Error creating bug: {0}", ex.InnerException.Message);
                return false;
            }

            //patchDocument.Add(new JsonPatchOperation(){Operation = Operation.Add,Path = "/fields/Microsoft.VSTS.Common.Priority",Value = "1"});
            //patchDocument.Add(new JsonPatchOperation(){Operation = Operation.Add,Path = "/fields/Microsoft.VSTS.Common.Severity",Value = "2 - High"});
            //    WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            //    try
            //    {
            //        // Get the specified work item
            //        WorkItem workitem = await witClient.GetWorkItemAsync(WorkItemId);

            //        // Output the work item's field values
            //        foreach (var field in workitem.Fields)
            //        {
            //            Console.WriteLine("  {0}: {1}", field.Key, field.Value);
            //        }
            //    }
            //    catch (AggregateException aex)
            //    {
            //        VssServiceException vssex = aex.InnerException as VssServiceException;
            //        if (vssex != null)
            //        {
            //            Console.WriteLine(vssex.Message);
            //        }
            //    }
            //}
            return true;
        }

        public bool DeleteItem(Item item)
        {
            //int id = Convert.ToInt32(Context.GetValue<WorkItem>("$newWorkItem2").Id);

            //// Get a client
            //VssConnection connection = Context.Connection;
            //WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            //// Delete the work item (but don't destroy it completely)
            //WorkItemDelete results = workItemTrackingClient.DeleteWorkItemAsync(id, destroy: false).Result;

            return true;
        }

        public async Task<Item> GetByExternalID(string Id)
        {
            var wiql = new Wiql(){Query = $"Select [Id]  From WorkItems Where [System.TeamProject] = '{projectName}' And Title contains '[{Id}]'" };

            using (var client = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                var result = await client.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var workItemIds = result.WorkItems.Select(item => item.Id).ToArray();

                if (workItemIds.Any())
                {
                    var fields = new[] { "System.Id", "System.Title", "System.Description" };
                    var workItems = await client.GetWorkItemsAsync(workItemIds, fields, result.AsOf).ConfigureAwait(false);
 
                    var transformedWorkItem = workItems.FirstOrDefault()?.Fields.ToDictionary(p => p.Key.StartsWith("System.") ? p.Key.Substring(7) : p.Key, v => v.Value);

                    return _mapper.Map<Item>(transformedWorkItem);
                }
            }

            return null;
        }

        public async Task<bool> UpdateItem(Item item)
        {
            var azureItem = await GetByExternalID(item.ID);

            azureItem = _mapper.Map<Item, Item>(item, azureItem);

            var t = _mapper.Map<JsonPatchDocument>(azureItem);

            return true;
        }
    }
}
