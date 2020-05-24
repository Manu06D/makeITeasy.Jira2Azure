using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public class ItemChangeMessage
    {
        public Item Item { get; set; }
        public ItemChangeEventType EventType {get;set;}
        public List<string> PropertiesChanged { get; set; }
        public bool ShouldUpdate { get; set; }
    }
}
