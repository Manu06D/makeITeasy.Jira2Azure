using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using makeITeasy.AzureDevops.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain.Commands
{
    public class ItemChangeCommandHandler : INotificationHandler<ItemChangeCommand>
    {
        private readonly ILogger<ItemChangeCommandHandler> _logger;
        private readonly IItemService _itemservice;

        public ItemChangeCommandHandler(ILogger<ItemChangeCommandHandler> logger, IItemService itemservice)
        {
            this._logger = logger;
            this._itemservice = itemservice;
        }

        public async Task Handle(ItemChangeCommand notification, CancellationToken cancellationToken)
        {
            Action<Item> action =
                notification.EventType switch
                {
                    EventType.Create => async (x) => await _itemservice.CreateItemProcessAsync(x),
                    EventType.Update => async (x) => await _itemservice.UpdateItemProcessAsync(x),
                    EventType.Delete => async (x) => await _itemservice.DeleteItemProcessAsync(x),
                    _ => throw new Exception("Unable to find type for command")
                };

            action(notification.Item);
        }
    }
}
