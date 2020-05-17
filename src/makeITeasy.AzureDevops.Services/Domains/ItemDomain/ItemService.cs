using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Attributed;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService([WithKey("Destination")]IItemRepository itemRepository, [WithKey("Source")]IItemRepository sourceItemRepository)
        {
            this._itemRepository = itemRepository;
        }

        public async Task<bool> CreateItemProcessAsync(Item item)
        {
            var x = item;

            await Task.Delay(20000);

            return true;
        }

        public Task<bool> DeleteItemProcessAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemProcessAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
