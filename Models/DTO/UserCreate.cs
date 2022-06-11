namespace capybara_api.Models.DTO;

public record UserCreate(string email, string password, string name);