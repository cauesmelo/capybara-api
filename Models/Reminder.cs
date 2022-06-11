namespace capybara_api.Models;

public class Reminder : BaseModel {
    public string title { get; set; } = String.Empty;
    public DateTime reminderDate { get; set; }
    public string userId { get; set; } = String.Empty;
}
