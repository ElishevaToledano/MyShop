using Entity;
using System.ComponentModel.DataAnnotations;

namespace dto
{
    public record productDTO( string ProductName,
        decimal Price, string Descriptions, string CategoryCategoryName,
        string? Image)

    { }
}


