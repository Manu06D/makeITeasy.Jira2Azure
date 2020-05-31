using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface IItemRepository
    {
        Task<ItemOperationResult> GetByExternalIDAsync(string id);
        Task<ItemOperationResult> CreateItemAsync(Item newItem);
        Task<ItemOperationResult> DeleteItemAsync(Item item);
        Task<ItemOperationResult> UpdateItemAsync(Item item);
    }
}
