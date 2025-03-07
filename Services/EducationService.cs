using Microsoft.EntityFrameworkCore;
using ResumeAPI.Data;
using ResumeAPI.DTO;
using ResumeAPI.Models;

namespace ResumeAPI.Services
{
    public class EducationService
    {
        private readonly ResumeDBContext context;

        public EducationService(ResumeDBContext _context)
        {
            context = _context;
        }
        public async Task<EducationDTO?> GetEducation(int id)
        {
            var education = await context.Educations.FirstOrDefaultAsync(e => e.ID == id);

            if (education == null)
            {
                return null;
            }

            return new EducationDTO
            {
                School = education.School,
                Field = education.Field,
                Description = education.Description,
                StartDate = education.StartDate,
                EndDate = education.EndDate
            };
        }

        public async Task<(int, int, List<EducationDTO>)> GetAllEducationsPagination(int page)
        {
            int pagesize = 10;
            var totalEducations = await context.Educations.CountAsync();

            var educationList = await context.Educations
            .OrderBy(p => p.ID)
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .Select(e => new EducationDTO
            {
                School = e.School,
                Field = e.Field,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            }).ToListAsync();
            return (page, totalEducations, educationList);
        }

        public async Task<Education> CreateEducation(EducationDTO educationDTO, int personID)
        {
            var education = new Education
            {
                School = educationDTO.School,
                Field = educationDTO.Field,
                Description = educationDTO.Description,
                StartDate = educationDTO.StartDate,
                EndDate = educationDTO.EndDate,
                PersonID_FK = personID
            };
            context.Add(education);
            await context.SaveChangesAsync();
            return education;
        }


        public async Task<EducationDTO?> PutEducation(EducationDTO educationDTO, int id)
        {
            var existingEducation = await context.Educations.FirstOrDefaultAsync(e => e.ID == id);

            existingEducation.School = educationDTO.School;
            existingEducation.Field = educationDTO.Field;
            existingEducation.Description = educationDTO.Description;
            existingEducation.StartDate = educationDTO.StartDate;
            existingEducation.EndDate = educationDTO.EndDate;
            await context.SaveChangesAsync();
            return educationDTO;
        }
        public async Task<int> DeleteEducation(int id)
        {
            int rowsAffected = await context.Educations.Where(e => e.ID == id).ExecuteDeleteAsync();
            return rowsAffected;
        }
    }
}
