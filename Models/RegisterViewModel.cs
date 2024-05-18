using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeNest.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public List<SelectListItem> Roles { get; set; }

        [Required]
        public string SelectedRole { get; set; }
    }
}
