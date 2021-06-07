using System.ComponentModel.DataAnnotations;
using TodoDemo.ValidationAttributes;

namespace TodoDemo.Models
{
    [CheckLogin]
    public class LoginVm
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), MinLength(5), MaxLength(100)]
        public string Password { get; set; }
    
    }
}