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
            _logger = logger;
            _itemservice = itemservice;
        }

        public async Task Handle(ItemChangeCommand notification, CancellationToken cancellationToken)
        {
            Func<ItemChangeMessage, Task<OperationResult<Item>>> action =
                    notification.ItemChangeMessage.EventType switch
                    {
                        ItemChangeEventType.Create =>  (x) => _itemservice.CreateItemProcessAsync(x),
                        ItemChangeEventType.Update =>  (x) => _itemservice.UpdateItemProcessAsync(x),
                        ItemChangeEventType.Delete =>  (x) => _itemservice.DeleteItemProcessAsync(x),
                        _ => throw new Exception("Unable to find type for command")
                    };

            var _ = await action(notification.ItemChangeMessage);
        }
    }
}
