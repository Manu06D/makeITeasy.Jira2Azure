using System;
using Xunit;
using AutoMapper;
using makeITeasy.AzureDevops.Models;
using System.IO;
using Newtonsoft.Json;
using FluentAssertions;
using System.Linq;

namespace makeITeasy.Jira2Azure.Tests
{
    public class MappingUnitTest
    {
        [Fact]
        public void TestMappingFromTrace()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(ModelsAssembly.GetAssembly)));

            string jsonTrace = new StreamReader("traces\\createdIssue.json").ReadToEnd();

            //var jiraMessage = JsonConvert.DeserializeObject<JiraWebHookReceiveMessage>(jsonTrace);

            //var result = mapper.Map<ItemChangeMessage>(jiraMessage);

            //var x = mapper.ConfigurationProvider.FindTypeMapFor(typeof(JiraIssue), typeof(Item)).PropertyMaps.ToList();

            //result.Item.Title.Should().Be(jiraMessage.Issue.Fields.Summary);
            //result.PropertiesChanged.Should().NotBeEmpty().And.HaveCount(5);
            //result.EventType.Should().Be(ItemChangeEventType.Create);
            //result.ShouldUpdate.Should().BeTrue();
        }
    }
}
