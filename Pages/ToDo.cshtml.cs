
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using loginDemo.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace MyApp.Namespace
{
    [Authorize]
    public class ToDoModel : PageModel
    {
        [BindProperty]
        public TblTodo NewToDo { get; set; } = default!;

        public UserToDoDatabaseContext ToDoDb = new();
    
        public List<TblTodo> ToDoList { get;set; } = default!;
        public void OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId
            // LINQ query to retrieve items where IsDeleted is false
            ToDoList = (from item in ToDoDb.TblTodos
                          where item.IsDeleted == false
                          where item.UserId == userId
                          select item).ToList();

        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || NewToDo == null)
            {
                return Page();
            }
            NewToDo.IsDeleted = false;
            NewToDo.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ToDoDb.Add(NewToDo);
            ToDoDb.SaveChanges();
            return RedirectToAction("Get");
        }

        public IActionResult OnPostDelete(int id)
        {
           // var itemToUpdate = ToDoList.FirstOrDefault(item => item.Id == id);
            if (ToDoDb.TblTodos != null)
            {
                var todo = ToDoDb.TblTodos.Find(id);
                if (todo != null)
                {
                    todo.IsDeleted = true;
                    ToDoDb.SaveChanges();
                }
            }            

            return RedirectToAction("Get");
        }

    }
    
}
