using System;
using System.Collections.Generic;
using System.Text;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsSourceControlRepository : ISourceControlRepository
    {
        public bool CreateNewBranch(string name)
        {
            throw new NotImplementedException();
        }
    }
}
