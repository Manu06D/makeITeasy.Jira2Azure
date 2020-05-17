using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace makeITeasy.AzureDevops.Models
{
    public class ModelMappingProfiles : Profile
    {
        public ModelMappingProfiles()
        {
            CreateMap<JiraIssue, Item>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Key))
                .IncludeMembers(src => src.Fields)
                ;

            CreateMap<Fields, Item>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                ;


            CreateMap<JiraIssueEventType, EventType>()
                .ReverseMap();
        }
    }
}
