using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Models
{
    public class GitCommitInfo
    {
        public string RepositoryID { get; set; }
        public string CommitID { get; set; }
        public String ProjectID { get; set; }
        public string BranchName { get; set; }
    }
}
