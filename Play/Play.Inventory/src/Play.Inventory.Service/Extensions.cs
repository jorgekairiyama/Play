using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Extensions;

public static class Extensions
{
    public static ItemDto AsDto(this InventoryItem itm, string name, string description)
    {
        return new ItemDto(name, description, itm.Quantity, itm.AcquiredDate);
    }
}