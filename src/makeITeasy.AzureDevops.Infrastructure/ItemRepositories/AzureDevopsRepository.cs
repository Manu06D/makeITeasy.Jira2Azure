using System;
using System.Collections.Generic;
using System.Text;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Models.Configuration;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class AzureDevopsRepository : IItemRepository
    {
        private readonly AzureDevopsConfiguration _azureDevopsConfiguration;

        public AzureDevopsRepository(AzureDevopsConfiguration azureDevopsConfiguration)
        {
            this._azureDevopsConfiguration = azureDevopsConfiguration;
        }

        public bool CreateItem(Item newItem)
        {
            return true;
        }

        public bool DeleteItem(Item item)
        {
            return true;
        }

        public bool UpdateItem(Item item)
        {
            return true;
        }
    }
}
