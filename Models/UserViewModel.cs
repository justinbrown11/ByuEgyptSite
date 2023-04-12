using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    public class UserViewModel
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public string[]? SelectedRoles { get; set; }
    }
}
