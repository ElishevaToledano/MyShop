using System.ComponentModel.DataAnnotations;
using Entity;
namespace dto
{
    public record userDTO(string UserName, string? FirstName,string? LastName,
        ICollection<orderDTO> Orders);
    public record RegisterUserDTO(string UserName, string? FirstName,
        string? LastName, string? Password);
    //public record UpdateUserDTO(string UserName, string? FirstName,
    //string? LastName, string? Password);

}

