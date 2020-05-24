using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;

namespace makeITeasy.AzureDevops.Models
{
    public class ModelMappingProfiles : Profile
    {
        public ModelMappingProfiles()
        {
            CreateMap<JiraWebHookReceiveMessage, ItemChangeMessage>()
                .ForMember(dest => dest.PropertiesChanged, opt => opt.MapFrom(src => src.Changelog.Items.Select(x => x.FieldId).ToList()))
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.Issue))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.IssueEventTypeName))
                .ForMember(dest => dest.ShouldUpdate, opt => opt.MapFrom<ShouldUpdateResolver>())
                ;

            //CreateMap<JiraIssueEventType, ItemChangeEventType>().ConvertUsing(value =>
            //{
            //    switch (value)
            //    {
            //        case JiraIssueEventType.issue_created:
            //            return ItemChangeEventType.Create;
            //        case JiraIssueEventType.issue_created:
            //            return ItemChangeEventType.Create;
            //        case EnumSrc.Option3:
            //            return EnumDst.Choice3;
            //        default:
            //            return EnumDst.None;
            //    }
            //});

            CreateMap<JiraIssue, Item>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Key))
                .IncludeMembers(src => src.Fields)
                ;

            CreateMap<Fields, Item>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                ;


            CreateMap<JiraIssueEventType, ItemChangeEventType>()
                .ReverseMap();

            CreateMap<Item, Item>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.ID))
                ;

            //"{\"System.Id\":3,\"System.Title\":\"[SAM-1]Test Ticket 1\",\"System.Description\":\"<div>qsdqsdqdsqd</div>\"}"
            CreateMap<Dictionary<String, object>, Item>()
                .ConvertUsing(x => JsonConvert.DeserializeObject<Item>(JsonConvert.SerializeObject(x)));
        }

        public class ShouldUpdateResolver : IValueResolver<JiraWebHookReceiveMessage, ItemChangeMessage, bool>
        {
            public bool Resolve(JiraWebHookReceiveMessage source, ItemChangeMessage destination, bool member, ResolutionContext context)
            {
                var t = context.ConfigurationProvider.FindTypeMapFor(typeof(JiraIssue), typeof(Item)).PropertyMaps.ToList()[1].SourceMembers.ToList()[1];

                var propertiesToWatch =
                    context.ConfigurationProvider.FindTypeMapFor(typeof(JiraIssue), typeof(Item)).PropertyMaps.Select(x => x.SourceMember.Name)
                    .Union(context.ConfigurationProvider.FindTypeMapFor(typeof(Fields), typeof(Item)).PropertyMaps.Select(x => x.SourceMember.Name)).ToList();


                return source.Changelog.Items.Any(x => propertiesToWatch.Contains(x.FieldId, StringComparer.InvariantCultureIgnoreCase));
            }
        }
    }
}
