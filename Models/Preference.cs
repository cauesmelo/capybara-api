using capybara_api.Models.Base;

namespace capybara_api.Models
{
    public class Preference : Entity
    {
        public string Theme { get; set; }
        public string NotificationEmail { get; set; }   
    }
}
