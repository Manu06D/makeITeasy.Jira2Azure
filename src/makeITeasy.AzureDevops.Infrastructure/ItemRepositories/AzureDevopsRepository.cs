using System;
using System.Collections.Generic;
using System.Text;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsRepository : IItemRepository
    {
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;
        private Uri repoURL;
        private String token;
        private String projectName;

        public AzureDevopsRepository(AzureDevopsConfiguration azureDevopsConfiguration)
        {
            this._azureDevopsConfiguration = azureDevopsConfiguration;
            repoURL = new Uri(azureDevopsConfiguration.Uri);
            token = azureDevopsConfiguration.Token;
            projectName = azureDevopsConfiguration.ProjectName;
        }

        public bool CreateItem(Item newItem)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(new JsonPatchOperation() {Operation = Operation.Add, Path = "/fields/System.Title", Value = $"{newItem.ID}{newItem.Title}"});

            VssConnection connection = new VssConnection(repoURL, new VssBasicCredential(string.Empty, token));
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

        public bool UpdateItem(Item item)
        {
            return true;
        }

    }
}
