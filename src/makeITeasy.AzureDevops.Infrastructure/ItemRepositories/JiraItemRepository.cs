using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class JiraItemRepository : IItemRepository
    {
        public JiraItemRepository()
        {
        }

        public bool CreateItem(Item newItem)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<Item> GetByExternalID(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
