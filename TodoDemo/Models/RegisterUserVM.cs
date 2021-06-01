using System.ComponentModel.DataAnnotations;

namespace TodoDemo.Models
{
    public class RegisterUserVM
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        
       
        [Required, DataType(DataType.Password), MinLength(2), MaxLength(100)]
        public string Password { get; set; }
        
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string PasswordCheck { get; set; }

        public User ConvertToUser()
        {
            return new User()
            {
                Email = Email, Password = Password
            };
        }
    }
}