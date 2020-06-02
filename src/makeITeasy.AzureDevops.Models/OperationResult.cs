using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public class OperationResult<TResult> where TResult : class
    {
        public TResult Item { get; set; }
        public bool HasSucceed { get; set; }
        public string ErrorMessage { get; set; }

        public OperationResult(TResult item, bool hasSucceed)
        {
            Item = item;
            HasSucceed = hasSucceed;
        }

        public OperationResult()
        {
        }
    }
}
