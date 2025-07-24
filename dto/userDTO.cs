using System.ComponentModel.DataAnnotations;
using Entity;
namespace dto
{
    public record userDTO(int UserId,string UserName, string? FirstName,string? LastName,
        ICollection<orderDTO> Orders);
    public record RegisterUserDTO(string UserName, string? FirstName,
        string? LastName, string? Password);
}

