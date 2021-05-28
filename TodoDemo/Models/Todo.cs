using System.ComponentModel.DataAnnotations;

namespace TodoDemo.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        [Required, MinLength(3), MaxLength(150)]
        public string Description { get; set; }

        public bool Done { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
    }
}