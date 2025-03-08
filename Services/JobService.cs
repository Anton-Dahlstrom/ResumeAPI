
using Microsoft.EntityFrameworkCore;
using ResumeAPI.Data;
using ResumeAPI.DTO;
using ResumeAPI.Models;

namespace ResumeAPI.Services
{
	public class JobService
	{
		private readonly ResumeDBContext context;

		public JobService(ResumeDBContext _context)
		{
			context = _context;
		}
		public async Task<JobDTO?> GetJob(int id)
		{
			var job = await context.Jobs.FirstOrDefaultAsync(e => e.ID == id);

			if (job == null)
			{
				return null;
			}

			return new JobDTO
			{
				Company = job.Company,
				Title = job.Title,
				Description = job.Description,
				StartDate = job.StartDate,
				EndDate = job.EndDate
			};
		}

		public async Task<(int, int, List<JobDTO>)> GetAllJobsPagination(int page)
		{
			int pagesize = 10;
			var totalJobs = await context.Jobs.CountAsync();

			var jobList = await context.Jobs
			.OrderBy(p => p.ID)
			.Skip((page - 1) * pagesize)
			.Take(pagesize)
			.Select(e => new JobDTO
			{
				Company = e.Company,
				Title = e.Title,
				Description = e.Description,
				StartDate = e.StartDate,
				EndDate = e.EndDate
			}).ToListAsync();
			return (page, totalJobs, jobList);
		}

		public async Task<Job> CreateJob(JobDTO jobDTO, int personID)
		{
			var job = new Job
			{
				Company = jobDTO.Company,
				Title = jobDTO.Title,
				Description = jobDTO.Description,
				StartDate = jobDTO.StartDate,
				EndDate = jobDTO.EndDate,
				PersonID_FK = personID
			};
			context.Add(job);
			await context.SaveChangesAsync();
			return job;
		}


		public async Task<JobDTO?> PutJob(JobDTO jobDTO, int id)
		{
			var existingJob = await context.Jobs.FirstOrDefaultAsync(e => e.ID == id);

			existingJob.Company = jobDTO.Company;
			existingJob.Title = jobDTO.Title;
			existingJob.Description = jobDTO.Description;
			existingJob.StartDate = jobDTO.StartDate;
			existingJob.EndDate = jobDTO.EndDate;
			await context.SaveChangesAsync();
			return jobDTO;
		}
		public async Task<int> DeleteJob(int id)
		{
			int rowsAffected = await context.Jobs.Where(e => e.ID == id).ExecuteDeleteAsync();
			return rowsAffected;
		}
	}
}
