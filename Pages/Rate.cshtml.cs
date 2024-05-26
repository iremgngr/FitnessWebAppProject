using System.Security.Claims;
using loginDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Authorize]
    public class RateModel : PageModel
    {
        public UserToDoDatabaseContext ToDoDb = new();

        public List<string> Comments { get; set; } = new();
        
        bool flag = false;

        public void OnGet(int id)
        {
          LoadComments(id);
        }
    
        public void OnPostRate(int id, int _rate)
        {
            var rating = new UserRate();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId
            if (userId!=null && id!=0 && _rate!=0)
            {
                rating.UserId = userId;
                rating.Rate = (short?)_rate;
                rating.TodoId = id;
                ToDoDb.UserRates.Add(rating);
                ToDoDb.SaveChanges();
                flag=true;
            }
            LoadComments(id);
            if (flag)
            {
                Response.Redirect("/UserRates");
            }
            else{
                Page();
            }    
        }

        public IActionResult OnPostAddComment(int id, string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                var todo = ToDoDb.TblTodos.FirstOrDefault(t => t.Id == id);
                if (todo != null)
                {
                    todo.Comment = string.IsNullOrEmpty(todo.Comment) ? comment : $"{todo.Comment}||{comment}";
                    ToDoDb.SaveChanges();
                }
            }
            //Response.Redirect($"/Rate/{id}");
            LoadComments(id);
            return RedirectToPage(new { id });
        }

        private void LoadComments(int id)
        {
            var todo = ToDoDb.TblTodos.FirstOrDefault(t => t.Id == id);
            if (todo != null && !string.IsNullOrEmpty(todo.Comment))
            {
                Comments = todo.Comment.Split(new[] { "||" }, StringSplitOptions.None).ToList();
            }
        }
    }
}
