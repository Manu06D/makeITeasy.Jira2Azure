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
            //Action<ItemChangeMessage> action =
            //    notification.ItemChangeMessage.EventType switch
            //    {
            //        ItemChangeEventType.Create => async (x) => await _itemservice.CreateItemProcessAsync(x),
            //        ItemChangeEventType.Update => async (x) =>  await _itemservice.UpdateItemProcessAsync(x),
            //        ItemChangeEventType.Delete => async (x) => await _itemservice.DeleteItemProcessAsync(x),
            //        _ => throw new Exception("Unable to find type for command")
            //    };


            //   action(notification.ItemChangeMessage);

            Func<ItemChangeMessage, Task<bool>> action =
                    notification.ItemChangeMessage.EventType switch
                    {
                        ItemChangeEventType.Create =>  (x) =>  _itemservice.CreateItemProcessAsync(x),
                        ItemChangeEventType.Update =>  (x) =>  _itemservice.UpdateItemProcessAsync(x),
                        ItemChangeEventType.Delete =>  (x) =>  _itemservice.DeleteItemProcessAsync(x),
                        _ => throw new Exception("Unable to find type for command")
                    };


            var _ = await action(notification.ItemChangeMessage);
        }
    }
}
