using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoDemo.Repositories;

namespace TodoDemo.Pages.Category
{
    public class EditOrCreate : BasePageModel
    {
        [BindProperty]
        public Models.Category Category { get; set; }
        
        public EditOrCreateMode Mode { get ; set; }
        
        public enum EditOrCreateMode
        {
            Edit, Create
        }
        
        public void OnGet(int? categoryId)
        {
            if (categoryId != null)
            {
                Category = new CategoryRepository().Get(categoryId.Value);
                Mode = EditOrCreateMode.Edit;
            }
            else
            {
                Category = new Models.Category();
                Mode = EditOrCreateMode.Create;
            }
        }

        public IActionResult OnPostUpdate()
        {
            Mode = EditOrCreateMode.Edit;
            
            if (!ModelState.IsValid)
                return Page();

            var category = new CategoryRepository().GetCategoryByName(Category.Name, UserId);
            if (category != null) //name is already in use
            {
                ModelState.AddModelError("Category.Name", $"Category with Name: '{category.Name}' already exists!");
                return Page();
            }
            
            new CategoryRepository().Update(Category, UserId);
            
            return RedirectToPage(nameof(Index));
        }

        public IActionResult OnPostCreate()
        {
            Mode = EditOrCreateMode.Create;
            
            if (!ModelState.IsValid)
                return Page();
            
            var category = new CategoryRepository().GetCategoryByName(Category.Name, UserId);
            if (category != null) //name is already in use
            {
                ModelState.AddModelError("Category.Name", $"Category with Name: '{category.Name}' already exists!");
                return Page();
            }

            new CategoryRepository().Add(Category, UserId);
            
            return RedirectToPage(nameof(Index)); 
        }
    }
}