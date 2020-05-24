using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace makeITeasy.AzureDevops.Infrastructure
{
    public class InfrastructureMappingProfiles : Profile
    {
        public InfrastructureMappingProfiles()
        {
            CreateMap<Item, JsonPatchDocument>()
                .ConvertUsing(new JsonPatchDocumentTypeConverter());
        }

        public class JsonPatchDocumentTypeConverter : ITypeConverter<Item, JsonPatchDocument>
        {
            public JsonPatchDocument Convert(Item source, JsonPatchDocument destination, ResolutionContext context)
            {
                var patchDocument = new JsonPatchDocument();
                patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Title", Value = $"[{source.ExternalID}]-{source.Title}" });
                patchDocument.Add(new JsonPatchOperation() { Operation = Operation.Add, Path = "/fields/System.Description", Value = $"{source.Description}" });

                return patchDocument;
            }
        }
    }
}
