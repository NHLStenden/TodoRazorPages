using System.ComponentModel.DataAnnotations;

namespace TodoDemo.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required, MinLength(2), MaxLength(100)]
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}