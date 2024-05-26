using System.Security.Claims;
using loginDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [Authorize]
    public class UserRatesModel : PageModel
    {
        [BindProperty]
        public TblTodo NewToDo { get; set; } = default!;

        public UserToDoDatabaseContext ToDoDb = new();

        public List<TblTodo> ToDoList { get; set; } = default!;

        public List<AverageRating> AverageRatings { get; set; } = default!;

        public List<float> Ratings { get; set; } = default!;


        public string Keyword { get; set; }

        public void OnGet(string difficultyLevel, string category, string sortOrder)
        {
            Keyword = Request.Query["keywords"];
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId

            // LINQ query to retrieve items where IsDeleted is false and other users' ToDos
            ToDoList = (from item in ToDoDb.TblTodos
                        where item.IsDeleted == false
                        where item.UserId != userId
                        select item).ToList();

            // Calculate average rating for each product
            AverageRatings = (from todo in ToDoDb.TblTodos
                              join rate in ToDoDb.UserRates on todo.Id equals rate.TodoId into gj
                              from subRate in gj.DefaultIfEmpty()
                              group subRate by todo into g
                              select new AverageRating
                              {
                                  TodoId = g.Key.Id,
                                  Average = g.Any() ? (float)g.Average(r => r.Rate ?? 0) : 0
                              }).ToList();

            if (!string.IsNullOrEmpty(Keyword))
            {
                string keywordLower = Keyword.ToLower();
                ToDoList = ToDoList.Where(item =>
                item.Title.ToLower().Contains(keywordLower) ||
                item.Description.ToLower().Contains(keywordLower) ||
                item.DifficultyLevel.ToLower().Contains(keywordLower) ||
                item.Category.ToLower().Contains(keywordLower)).ToList();
            }

            if (!string.IsNullOrEmpty(difficultyLevel))
            {
                ToDoList = ToDoList.Where(item => item.DifficultyLevel == difficultyLevel).ToList();
            }


            if (!string.IsNullOrEmpty(category))
            {
                ToDoList = ToDoList.Where(item => item.Category == category).ToList();
            }

            // Sort ToDoList based on SortOrder
            switch (sortOrder)
            {
                case "easy_to_hard":
                    ToDoList = ToDoList.OrderBy(item => GetDifficultyPriority(item.DifficultyLevel)).ToList();
                    break;
                case "hard_to_easy":
                    ToDoList = ToDoList.OrderByDescending(item => GetDifficultyPriority(item.DifficultyLevel)).ToList();
                    break;
                default:
                    // Do nothing or handle other cases
                    break;
            }

            Ratings = AverageRatings.Select(x => x.Average).ToList();
        }

        // Function to get priority for difficulty level
        private int GetDifficultyPriority(string difficultyLevel)
        {
            switch (difficultyLevel)
            {
                case "Easy":
                    return 1;
                case "Medium":
                    return 2;
                case "Hard":
                    return 3;
                default:
                    return int.MaxValue; // Set a high value for unknown difficulty levels
            }
        }

        // Add this method to handle saving a ToDo item
        public IActionResult OnPostSave(int todoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRate = ToDoDb.UserRates.FirstOrDefault(ur => ur.TodoId == todoId && ur.UserId == userId);

            if (userRate == null)
            {
                userRate = new UserRate
                {
                    UserId = userId,
                    TodoId = todoId,
                    IsSaved = true
                };
                ToDoDb.UserRates.Add(userRate);
            }
            else
            {
                userRate.IsSaved = true;
            }

            ToDoDb.SaveChanges();
            return RedirectToPage();
        }



        /*
        public IActionResult OnGetSearch(string keywords, string difficultyLevel, string category, DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId
            
            // LINQ query to retrieve items where IsDeleted is false and other users' ToDos
            var query = from item in ToDoDb.TblTodos
                        where item.IsDeleted == false
                        where item.UserId != userId
                        select item;
            
            // Apply filters based on user input
            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(item => item.Title.Contains(keywords) || item.Description.Contains(keywords));
            }
            if (!string.IsNullOrEmpty(difficultyLevel))
            {
                query = query.Where(item => item.DifficultyLevel == difficultyLevel);
            }
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(item => item.Category == category);
            }
            if (startDate != null)
            {
                query = query.Where(item => item.EndDate >= startDate);
            }
            if (endDate != null)
            {
                query = query.Where(item => item.EndDate <= endDate);
            }

            // Order by difficulty level
            switch (SortOrder)
            {
                case "easy_to_hard":
                    query = query.OrderBy(item => item.DifficultyLevel);
                    break;
                case "hard_to_easy":
                    query = query.OrderByDescending(item => item.DifficultyLevel);
                    break;
                default:
                    // Do nothing or handle other cases
                    break;
            }
            
            ToDoList = [.. query];
            
            // Calculate average rating for each product
            // This part may need adjustments based on your requirements
            
            // Redirect to the same page with the filtered results
            return Page();
        }*/

    }
}

