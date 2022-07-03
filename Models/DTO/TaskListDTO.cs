namespace capybara_api.Models.DTO;

public record TaskListCreate(string title, List<TaskUnityCreate> tasks);

public record TaskUnityCreate(string title);