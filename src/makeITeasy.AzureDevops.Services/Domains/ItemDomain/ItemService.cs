﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using makeITeasy.AzureDevops.Models;
using makeITeasy.AzureDevops.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain
{
    public class ItemService : IItemService
    {
        private const string GitLabelPrefix = "git";
        private readonly IItemRepository _destinationItemRepository;
        private readonly IItemRepository _sourceItemRepository;
        private readonly ISourceControlRepository _sourceControlRepository;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
            [KeyFilter("Destination")]IItemRepository destinationItemRepository, 
            [KeyFilter("Source")]IItemRepository sourceItemRepository, 
            ISourceControlRepository sourceControlRepository, 
            ILogger<ItemService> logger)
        {
            _destinationItemRepository = destinationItemRepository;
            _sourceItemRepository = sourceItemRepository;
            _sourceControlRepository = sourceControlRepository;
            _logger = logger;
        }

        public async Task<OperationResult<Item>> CreateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            _logger.LogInformation($"Creating Item {itemMessage.Item.ID}");
            OperationResult<Item>  result = await _destinationItemRepository.CreateItemAsync(itemMessage.Item);

            return result;
        }

        public async Task<OperationResult<Item>> UpdateItemProcessAsync(ItemChangeMessage itemMessage)
        {
            if (itemMessage?.ShouldUpdate == true)
            {
                _logger.LogInformation($"Updating Item {itemMessage.Item.ID}");

                OperationResult<Item> result = await _destinationItemRepository.UpdateItemAsync(itemMessage.Item);

                if (result?.HasSucceed == true && itemMessage.PropertiesChanged?.Contains("labels") == true && itemMessage.Item?.Labels?.Any(x => x.StartsWith(GitLabelPrefix)) == true)
                {
                    string gitLabel = itemMessage.Item?.Labels?.FirstOrDefault(x => x.StartsWith(GitLabelPrefix)) ?? String.Empty;

                    var newBranchResult = await _sourceControlRepository.CreateNewBranch($"{itemMessage.Item.ID}{gitLabel.Substring(GitLabelPrefix.Length)}");

                    if (newBranchResult.HasSucceed)
                    {
                        bool isNewBranchItemAssociated = await _destinationItemRepository.UpdateItemWithSourceControlInfoAsync(result.Item, newBranchResult.Item);

                        if(isNewBranchItemAssociated)
                        {
                            result.HasSucceed = newBranchResult.HasSucceed;

                            return result;
                        }
                    }
                }
            }

            return new OperationResult<Item>() { HasSucceed = false } ;
        }

        public Task<OperationResult<Item>> DeleteItemProcessAsync(ItemChangeMessage itemMessage)
        {
            throw new NotImplementedException();
        }
    }
}
