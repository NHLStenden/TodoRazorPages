using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoDemo.Models
{
    public class Todo
    {
        public int TodoId { get; set; }

        [Required, MinLength(3), MaxLength(150)]
        public string Description { get; set; }

        [Display(Name = "Completed")]
        public bool Done { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required, Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
        
        public IList<User> AssignedUsers { get; set; }
        
        [Display(Name = "Assigned Users")]
        public IList<int> AssignedUserIds { get; set; }
    }
}