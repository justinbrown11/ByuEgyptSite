using System.ComponentModel.DataAnnotations;

namespace ByuEgyptSite.Models
{
    public class UserViewModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; } // comma-separated list of roles, array of strings
    }
}
