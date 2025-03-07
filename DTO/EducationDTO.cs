using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.DTO
{
    public class EducationDTO
    {
        [MaxLength(256)]
        public string School { get; set; }
        [MaxLength(256)]
        public string Field { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }
    }
}
