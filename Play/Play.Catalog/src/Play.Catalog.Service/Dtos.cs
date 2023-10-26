using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    public record CreateItemDto([Required] string Name, string Description, [Required, Range(0, 100)] decimal Price);

    public record UpdateItemDto(string Name, string Description, [Required, Range(0, 100)] decimal Price);

}