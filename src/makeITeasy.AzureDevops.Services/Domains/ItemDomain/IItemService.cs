using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public interface IItemService
    {
        Task<ItemOperationResult> CreateItemProcessAsync(ItemChangeMessage itemMessage);
        Task<ItemOperationResult> DeleteItemProcessAsync(ItemChangeMessage itemMessage);
        Task<ItemOperationResult> UpdateItemProcessAsync(ItemChangeMessage itemMessage);
    }
}