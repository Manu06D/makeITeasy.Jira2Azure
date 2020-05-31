using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public class ItemOperationResult
    {
        public Item Item { get; set; }
        public bool HasSucceed { get; set; }
        public string ErrorMessage { get; set; }

        public ItemOperationResult(Item item, bool hasSucceed)
        {
            Item = item;
            HasSucceed = hasSucceed;
        }

        public ItemOperationResult()
        {
        }
    }
}
