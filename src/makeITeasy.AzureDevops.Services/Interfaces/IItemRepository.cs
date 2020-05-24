using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface IItemRepository
    {
        Task<Item> GetByExternalID(string id);
        bool CreateItem(Item newItem);
        bool DeleteItem(Item item);
        Task<bool> UpdateItem(Item item);
    }
}
