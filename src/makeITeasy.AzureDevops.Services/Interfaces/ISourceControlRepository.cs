using System;
using System.Collections.Generic;
using System.Text;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface ISourceControlRepository
    {
        bool CreateNewBranch(string name);
    }
}
