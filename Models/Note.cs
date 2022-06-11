namespace capybara_api.Models;

public class Note : BaseModel {
    public string content { get; set; } = String.Empty;
    public string userId { get; set; } = String.Empty;
}
