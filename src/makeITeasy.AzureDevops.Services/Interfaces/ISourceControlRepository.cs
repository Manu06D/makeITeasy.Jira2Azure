using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface ISourceControlRepository
    {
        Task<OperationResult<GitCommitInfo>> CreateNewBranch(string name);
    }
}
