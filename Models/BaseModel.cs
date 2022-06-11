using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace capybara_api.Models;

public abstract class BaseModel {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}
