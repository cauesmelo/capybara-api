namespace capybara_api.Models;

public class TaskUnity : BaseModel {
    public string title { get; set; }
    public bool isComplete { get; set; }
    public int taskListId { get; set; }
}
