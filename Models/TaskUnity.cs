namespace capybara_api.Models;

public class TaskUnity : BaseModel {
    public string title { get; set; } = String.Empty;
    public bool isComplete { get; set; }
    public int taskListId { get; set; }
}
