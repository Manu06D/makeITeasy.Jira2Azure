using AutoMapper;
using makeITeasy.AzureDevops.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace makeITeasy.AzureDevops.Infrastructure
{
    public class InfrastructureMappingProfiles : Profile
    {
        public InfrastructureMappingProfiles()
        {
            CreateMap<Item, JsonPatchDocument>()
                .ConvertUsing(new ItemToJsonPathDocumentConverter());

            CreateMap<WorkItem, Item>()
                .ConvertUsing(new WorkItemToItemConverter());
        }

        public class ItemToJsonPathDocumentConverter : ITypeConverter<Item, JsonPatchDocument>
        {
            public JsonPatchDocument Convert(Item source, JsonPatchDocument destination, ResolutionContext context)
            {
                var patchDocument = new JsonPatchDocument();
                patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Title", Value = $"[{source.ExternalID}]-{source.Title}" });
                patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Description", Value = $"{source.Description}" });

                return patchDocument;
            }
        }

        public class WorkItemToItemConverter : ITypeConverter<WorkItem, Item>
        {
            public Item Convert(WorkItem source, Item destination, ResolutionContext context)
            {
                var result = new Item();
                result.ID = source.Id.ToString();
                result.Title = source.Fields["System.Title"].ToString();

                return result;
            }
        }
    }
}
