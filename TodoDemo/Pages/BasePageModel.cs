using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoDemo.Pages
{
    
    public abstract class BasePageModel : PageModel
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
                
                return -1;
            }
        }
        
        public string StatusMessage
        {
            get
            {
                return TempData["StatusMessage"]?.ToString();
            } 
            set
            {
                TempData["StatusMessage"] = value;
            }
        }
    }
}