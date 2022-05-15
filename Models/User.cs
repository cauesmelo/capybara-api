using capybara_api.Models.Base;

namespace capybara_api.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Preference UserPreference { get; set; }
    }
}
