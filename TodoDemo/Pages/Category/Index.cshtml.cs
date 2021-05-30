using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Repositories;
using TodoDemo.Models;

namespace TodoDemo.Pages.Category
{
    public class Index : PageModel
    {
        public int UserId
        {
            get
            {
                string userIdStr = HttpContext.Session.GetString("userid");
                if (!string.IsNullOrWhiteSpace(userIdStr))
                {
                    int userId = int.Parse(userIdStr);
                    return userId;
                }
                
                HttpContext.Response.Redirect(nameof(Index));
                
                return -1;
            }
        }
        
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