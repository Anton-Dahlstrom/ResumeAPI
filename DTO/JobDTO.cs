
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.DTO
{
	public class JobDTO
	{
		[MaxLength(256)]
		public string Company { get; set; }
		[MaxLength(256)]
		public string Title { get; set; }
		[MaxLength(2500)]
		public string Description { get; set; }
		[DataType(DataType.Date)]
		public DateOnly StartDate { get; set; }
		[DataType(DataType.Date)]
		public DateOnly? EndDate { get; set; }
	}
}
