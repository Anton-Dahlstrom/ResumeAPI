using ResumeAPI.DTO;
using ResumeAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.Endpoints
{
	class JobEndpoints
	{
		public static void RegisterEndpoints(WebApplication app)
		{
			app.MapGet("/job", async (JobService jobService, int page = 1) =>
			{
				try
				{
					page = Math.Max(1, page);
					var jobs = await jobService.GetAllJobsPagination(page);
					if (jobs.Item3.Count < 1)
					{
						return Results.NoContent();
					}
					return Results.Ok(jobs.Item3);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});

			app.MapGet("/job/{id}", async (JobService jobService, int id) =>
			{
				var job = await jobService.GetJob(id);
				if (job == null)
				{
					return Results.NotFound("Person not found");
				}
				return Results.Ok(job);
			});

			app.MapPost("/job", async (JobService jobService, JobDTO jobDTO, int personID) =>
			{
				var validationContext = new ValidationContext(jobDTO);
				var validationResult = new List<ValidationResult>();

				bool isValid = Validator.TryValidateObject(jobDTO, validationContext, validationResult, true);

				if (!isValid)
				{
					return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
				}

				try
				{
					var job = await jobService.CreateJob(jobDTO, personID);
					return Results.Created($"/job/{job.ID}", jobDTO);
				}
				catch (InvalidOperationException ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});


			app.MapPut("/job/{id}", async (JobService jobService, int id, JobDTO jobDTO) =>
			{
				var validationContext = new ValidationContext(jobDTO);
				var validationResult = new List<ValidationResult>();

				bool isValid = Validator.TryValidateObject(jobDTO, validationContext, validationResult, true);

				if (!isValid)
				{
					return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
				}
				try
				{
					await jobService.PutJob(jobDTO, id);
					return Results.Created($"/person/{id}", jobDTO);
				}
				catch (InvalidOperationException ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});

			app.MapDelete("/job/{id}", async (JobService jobService, int id) =>
			{
				try
				{
					int rowsAffected = await jobService.DeleteJob(id);
					if (rowsAffected == 0)
					{
						return Results.NotFound("Job not found");
					}
					return Results.Ok($"Job with id {id} was deleted.");
				}
				catch (Exception ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});
		}
	}
}
