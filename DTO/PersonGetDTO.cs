
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.DTO
{
    public class PersonGetDTO
    {
        [Length(2, 200)]
        public string Name { get; set; }

        [Length(0, 2000)]
        public string Description { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
