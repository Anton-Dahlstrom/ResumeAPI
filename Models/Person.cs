using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.Models
{
	public class Person
	{
		[Key]
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }

		public virtual List<Job> Jobs { get; set; }
		public virtual List<Education> Educations { get; set; }
	}
}
