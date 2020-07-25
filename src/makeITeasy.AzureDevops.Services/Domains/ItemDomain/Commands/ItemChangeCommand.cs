using makeITeasy.AzureDevops.Models;
using MediatR;

namespace makeITeasy.AzureDevops.Services.Domains.ItemDomain.Commands
{
    public class ItemChangeCommand : INotification
    {
        public ItemChangeMessage ItemChangeMessage { get; private set; }

        public ItemChangeCommand(ItemChangeMessage itemChangeMessage)
        {
            ItemChangeMessage = itemChangeMessage;
        }
    }
}
