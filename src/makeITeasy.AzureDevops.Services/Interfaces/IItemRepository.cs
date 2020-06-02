using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface IItemRepository
    {
        Task<OperationResult<Item>> GetByExternalIDAsync(string id);
        Task<OperationResult<Item>> CreateItemAsync(Item newItem);
        Task<OperationResult<Item>> DeleteItemAsync(Item item);
        Task<OperationResult<Item>> UpdateItemAsync(Item item);
        Task<bool> UpdateItemWithSourceControlInfoAsync(Item item, GitCommitInfo commitInfo);
    }
}
