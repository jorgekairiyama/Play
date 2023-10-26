namespace Play.Inventory.Service.Dtos;

public record createItemDto(Guid UserId, Guid CatalogId, int Quantity);

public record ItemDto(string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

public record CatalogItemDto(Guid CatalogId, string Name, string Description);