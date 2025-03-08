
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.DTO
{
	public class PersonGetAllDTO
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
		public List<JobDTO> Jobs { get; set; }
		public List<EducationDTO> Educations { get; set; }
	}
}
