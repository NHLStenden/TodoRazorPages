using System.ComponentModel.DataAnnotations;

namespace TodoDemo.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        [Required, MinLength(3), MaxLength(150)]
        public string Description { get; set; }

        public bool Done { get; set; }
        
        public int UserId { get; set; }
    }
}