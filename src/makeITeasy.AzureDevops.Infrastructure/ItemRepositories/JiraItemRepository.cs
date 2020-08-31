using System;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Infrastructure.ItemRepositories
{
    public class JiraItemRepository : IItemRepository
    {

        public Task<OperationResult<Item>> DeleteItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<Item>> GetByExternalIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemWithSourceControlInfoAsync(Item item, GitCommitInfo commitInfo)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<Item>> IItemRepository.CreateItemAsync(Item newItem)
        {
            throw new NotImplementedException();
        }

        Task<OperationResult<Item>> IItemRepository.UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
