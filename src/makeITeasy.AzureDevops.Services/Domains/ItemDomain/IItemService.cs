using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public interface IItemService
    {
        Task<bool> CreateItemProcessAsync(Item item);
        Task<bool> DeleteItemProcessAsync(Item item);
        Task<bool> UpdateItemProcessAsync(Item item);
    }
}