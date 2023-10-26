using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> catalogRepository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogRepository)
    {
        this.catalogRepository = catalogRepository;
    }
    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;

        var catalogItem = await catalogRepository.GetAsync(message.ItemId);

        if (catalogItem != null)
        {
            return;
        }

        catalogItem = new CatalogItem
        {
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description
        };

        await catalogRepository.CreateAsync(catalogItem);
    }
}