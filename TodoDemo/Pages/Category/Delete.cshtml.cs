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
            Category = new CategoryRepository().Get(categoryId);
        }
        
        public RedirectToPageResult OnPost(int categoryId)
        {
            Category = new CategoryRepository().Get(categoryId);
            int numRowEffected = new CategoryRepository().Delete(categoryId);

            StatusMessage = $"Category: '{Category.Name}' deleted and {numRowEffected} todos deleted";
            
            return RedirectToPage(nameof(Index));
        }
    }
}