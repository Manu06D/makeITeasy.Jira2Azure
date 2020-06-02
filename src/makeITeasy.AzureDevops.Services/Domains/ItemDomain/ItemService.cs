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

        public async Task<OperationResult<Item>> CreateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            OperationResult<Item> result = null;

            _logger.LogInformation($"Creating Item {itemMessage.Item.ID}");
            result = await _destinationItemRepository.CreateItemAsync(itemMessage.Item);

            return result;
        }

        public async Task<OperationResult<Item>> UpdateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            OperationResult<Item> result = null;

            if (itemMessage.ShouldUpdate)
            {
                _logger.LogInformation($"Updating Item {itemMessage.Item.ID}");

                result = await _destinationItemRepository.UpdateItemAsync(itemMessage.Item);

                if(result != null && itemMessage.PropertiesChanged.Contains("labels") && itemMessage.Item.Labels.Contains("git"))
                {
                    var newBranchResult = await sourceControlRepository.CreateNewBranch(itemMessage.Item.ID);

                    var t = await _destinationItemRepository.UpdateItemWithSourceControlInfoAsync(result.Item, newBranchResult.Item);

                    //if (newBranchResult )
                    //{
                    //    var t = await _destinationItemRepository.UpdateItemWithSourceControlInfoAsync(result.Item);
                    //}

                    result.HasSucceed = newBranchResult.HasSucceed;
                }
            }

            return result;
        }

        public Task<OperationResult<Item>> DeleteItemProcessAsync(ItemChangeMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
