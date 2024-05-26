using System;
using System.Collections.Generic;

namespace loginDemo.Models;

public partial class TblTodo
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? UserId { get; set; }

    public string? Category { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? EndDate { get; set; }

    public string? DifficultyLevel { get; set; }  // New property

    public string? Comment { get; set; } // New property
}
