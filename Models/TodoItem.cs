

using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    public string Description { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);

    public string? FilePath { get; set; }
}
