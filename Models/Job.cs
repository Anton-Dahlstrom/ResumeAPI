using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeAPI.Models
{
	public class Job
	{
		[Key]
		public int ID { get; set; }
		public string Company { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		[ForeignKey("Person")]
		public int? PersonID_FK { get; set; }
		public virtual Person Person { get; set; }
	}
}
