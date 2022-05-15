using capybara_api.Models.Base;

namespace capybara_api.Models
{
    public class Entry : Entity
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
    }
}
