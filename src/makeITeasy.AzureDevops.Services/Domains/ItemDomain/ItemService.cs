using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _destinationItemRepository;
        private readonly IItemRepository _sourceItemRepository;
        private readonly ISourceControlRepository sourceControlRepository;
        private readonly ILogger<ItemService> _logger;

        public ItemService([KeyFilter("Destination")]IItemRepository destinationItemRepository, [KeyFilter("Source")]IItemRepository sourceItemRepository, ISourceControlRepository sourceControlRepository, ILogger<ItemService> logger)
        {
            this._destinationItemRepository = destinationItemRepository;
            this._sourceItemRepository = sourceItemRepository;
            this.sourceControlRepository = sourceControlRepository;
            this._logger = logger;
        }

        public async Task<bool> CreateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            _logger.LogInformation($"Creating Item {itemMessage.Item.ID}");
            return _destinationItemRepository.CreateItem(itemMessage.Item);
        }

        public async Task<bool> UpdateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            bool result = false;

            if (itemMessage.ShouldUpdate)
            {
                _logger.LogInformation($"Updating Item {itemMessage.Item.ID}");
                result = await _destinationItemRepository.UpdateItem(itemMessage.Item);
            }

            return result;
        }

        public Task<bool> DeleteItemProcessAsync(ItemChangeMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
