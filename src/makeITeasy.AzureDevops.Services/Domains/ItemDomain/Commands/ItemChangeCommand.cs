using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using makeITeasy.AzureDevops.Models;
using MediatR;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain.Commands
{
    public class ItemChangeCommand : INotification
    {
        public EventType EventType { get; private set; }
        public Item Item { get; private set; }

        public ItemChangeCommand(EventType eventType, Item item)
        {
            EventType = eventType;
            Item = item;
        }
    }
}
