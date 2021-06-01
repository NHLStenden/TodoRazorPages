using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Repositories;

namespace TodoDemo.Pages.Category
{
    public class Delete : BasePageModel
    {
        public Models.Category Category { get; set; }
        
       
        public void OnGet(int categoryId)
        {
            Category = new CategoryRepository().Get(categoryId, UserId);
        }
        
        public RedirectToPageResult OnPost(int categoryId)
        {
            Category = new CategoryRepository().Get(categoryId, UserId);
            int numRowEffected = new CategoryRepository().Delete(categoryId, UserId);

            StatusMessage = $"Category: '{Category.Name}' deleted and {numRowEffected} todos deleted";
            
            return RedirectToPage(nameof(Index));
        }
    }
}