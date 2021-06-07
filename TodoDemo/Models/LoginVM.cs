using System.ComponentModel.DataAnnotations;
using TodoDemo.Repositories;

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
    public class CheckLoginAttribute : ValidationAttribute
    {
        public CheckLoginAttribute()
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var loginVm =  value as LoginVm;
            
            User user = new UserRepository().CheckLogin(loginVm);
            if (user == null)
            {
                return new ValidationResult("Incorrect Email Password combination");
            }
            
            return ValidationResult.Success;
        }
    }
}