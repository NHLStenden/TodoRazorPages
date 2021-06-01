using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Repositories;
using TodoDemo.Models;

namespace TodoDemo.Pages.Category
{
    public class Index : BasePageModel
    {
        public IEnumerable<Models.Category> Categories
        {
            get
            {
                return new CategoryRepository().Get(null, UserId);
            }
        }

        public void OnGet()
        {
            
        }

        
    }
}