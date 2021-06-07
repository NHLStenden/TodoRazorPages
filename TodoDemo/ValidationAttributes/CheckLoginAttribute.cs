using System.ComponentModel.DataAnnotations;
using TodoDemo.Models;
using TodoDemo.Repositories;

namespace TodoDemo.ValidationAttributes
{
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