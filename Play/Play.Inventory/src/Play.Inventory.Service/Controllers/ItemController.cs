using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Extensions;

namespace Play.Inventory.Service.Controller;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IRepository<InventoryItem> inventoryRepository;
    private readonly IRepository<CatalogItem> catalogRepository;

    public ItemController(IRepository<InventoryItem> inventoryRepository, IRepository<CatalogItem> catalogRepository)
    {
        this.inventoryRepository = inventoryRepository;
        this.catalogRepository = catalogRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }

        var inventoryItems = await inventoryRepository.GetAllAsync(item => item.UserId == userId);
        var catalogIds = inventoryItems.Select(imt => imt.CatalogId);
        var catalogItems = await catalogRepository.GetAllAsync(itm => catalogIds.Contains(itm.Id));

        var invetoryItemDtos = inventoryItems.Select(itm =>
        {
            var catalogItem = catalogItems.Single(catItm => itm.CatalogId == catItm.Id);
            return itm.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(invetoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(createItemDto itemDto)
    {
        var inventoryItem = await inventoryRepository.GetAsync(item => item.UserId == itemDto.UserId && item.CatalogId == itemDto.CatalogId);

        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogId = itemDto.CatalogId,
                UserId = itemDto.UserId,
                Quantity = itemDto.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow
            };

            await inventoryRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity = itemDto.Quantity;
            await inventoryRepository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}