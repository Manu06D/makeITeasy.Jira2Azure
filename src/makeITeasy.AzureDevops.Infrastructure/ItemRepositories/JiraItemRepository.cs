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

        public Task<ItemOperationResult> DeleteItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<ItemOperationResult> GetByExternalIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<ItemOperationResult> IItemRepository.CreateItemAsync(Item newItem)
        {
            throw new NotImplementedException();
        }

        Task<ItemOperationResult> IItemRepository.UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
