namespace capybara_api.Models.DTO;

public class LoginResponse {
    public string token { get; set; }
    public string theme { get; set; }
    public string emailNotification { get; set; }
    public string email { get; set; }
    public string name { get; set; }
}
