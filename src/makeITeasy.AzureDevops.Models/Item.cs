using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public class Item
    {
        public string ID { get; set; }
        public string ExternalID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<string> Labels { get; set; }
    }
}
