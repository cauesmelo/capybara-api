namespace capybara_api.Models;

public class TaskList : BaseModel {
    public string title { get; set; }
    public List<TaskUnity> tasks { get; set; } = new();
    public string userId { get; set; }
}
