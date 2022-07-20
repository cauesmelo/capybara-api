namespace capybara_api.Models;

public class Note : BaseModel {
    public string content { get; set; }
    public string userId { get; set; }
}
