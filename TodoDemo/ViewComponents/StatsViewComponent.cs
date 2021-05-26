using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TodoDemo.Models;
using TodoDemo.Pages;
using TodoDemo.Repositories;

namespace TodoDemo.ViewComponents
{
    public class StatsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var todos = new TodoRepository().Get(null, userId);
            var stats = new Stats()
            {
                CompletedCount = todos.Count(x => x.Done),
                NotCompletedCount = todos.Count(x => !x.Done)
            };
            
            return View(stats);
        }
    }
}