using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public interface IItemService
    {
        Task<OperationResult<Item>> CreateItemProcessAsync(ItemChangeMessage itemMessage);
        Task<OperationResult<Item>> DeleteItemProcessAsync(ItemChangeMessage itemMessage);
        Task<OperationResult<Item>> UpdateItemProcessAsync(ItemChangeMessage itemMessage);
    }
}