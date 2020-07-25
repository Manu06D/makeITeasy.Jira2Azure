using System.Collections.Generic;

namespace makeITeasy.AzureDevops.Models
{
    public class ItemChangeMessage
    {
        public Item Item { get; set; }
        public ItemChangeEventType EventType {get;set;}
        public List<string> PropertiesChanged { get; set; } = new List<string>();
        public bool ShouldUpdate { get; set; }

        public override string ToString()
        {
            return $"{EventType}:{Item.ID}";
        }
    }
}
