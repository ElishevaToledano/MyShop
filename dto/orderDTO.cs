using Entity;
using System.ComponentModel.DataAnnotations;

namespace dto
{
    public record orderDTO(int? OrderId, DateTime? OrderDate, int? OrderSum,
         string? UserUserName, ICollection<orderItemsDTO> OrderItems);
public record addOrderDTO(DateTime OrderDate, int? OrderSum,
    int? UserId, ICollection<orderItemsDTO> OrderItems);
}
