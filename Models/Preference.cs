namespace capybara_api.Models;

public class Preference : BaseModel {
    public string notificationEmail { get; set; }
    public string theme { get; set; }
    public string userId { get; set; }
}
