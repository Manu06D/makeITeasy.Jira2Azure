using System;
using System.Collections.Generic;
using System.Text;
using makeITeasy.AzureDevops.Models;

namespace makeITeasy.AzureDevops.Services.Interfaces
{
    public interface IItemRepository
    {
        bool CreateItem(Item newItem);
        bool DeleteItem(Item item);
        bool UpdateItem(Item item);
    }
}
