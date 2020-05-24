using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public interface IItemService
    {
        Task<bool> CreateItemProcessAsync(ItemChangeMessage itemMessage);
        Task<bool> DeleteItemProcessAsync(ItemChangeMessage itemMessage);
        Task<bool> UpdateItemProcessAsync(ItemChangeMessage itemMessage);
    }
}