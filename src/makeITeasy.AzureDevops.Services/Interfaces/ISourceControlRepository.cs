using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface ISourceControlRepository
    {
        Task<bool> CreateNewBranch(string name);
    }
}
