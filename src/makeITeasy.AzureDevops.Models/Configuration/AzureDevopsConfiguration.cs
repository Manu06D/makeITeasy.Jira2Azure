using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models.Configuration
{
    public class AzureDevopsConfiguration
    {
        public Uri Uri { get; set; }
        public String Token { get; set; }
        public String ProjectName { get; set; }
        public GITSourceControl GITSourceControl { get; set; }
    }
    public class GITSourceControl
    {
        public string RepositoryName { get; set; }
        public string MasterBranch { get; set; }
        public string NewBranchPath { get; set; }
    }
}
