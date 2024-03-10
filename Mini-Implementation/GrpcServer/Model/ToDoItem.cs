using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcServer.Model;

public class ToDoItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ToDoStatus { get; set; } = "New"; // Default value setting
}
