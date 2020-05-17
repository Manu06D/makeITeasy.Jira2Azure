using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _destinationItemRepository;
        private readonly IItemRepository _sourceItemRepository;

        public ItemService([KeyFilter("Destination")]IItemRepository destinationItemRepository, [KeyFilter("Source")]IItemRepository sourceItemRepository)
        {
            this._destinationItemRepository = destinationItemRepository;
            this._sourceItemRepository = sourceItemRepository;
        }

        public async Task<bool> CreateItemProcessAsync(Item item)
        {
            var x = item;

            _destinationItemRepository.CreateItem(item);

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
