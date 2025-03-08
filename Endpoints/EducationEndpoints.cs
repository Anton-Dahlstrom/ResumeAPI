using ResumeAPI.DTO;
using ResumeAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.Endpoints
{
	class EducationEndpoints
	{
		public static void RegisterEndpoints(WebApplication app)
		{
			app.MapGet("/education", async (EducationService educationService, int page = 1) =>
			{
				try
				{
					page = Math.Max(1, page);
					var educations = await educationService.GetAllEducationsPagination(page);
					if (educations.Item3.Count < 1)
					{
						return Results.NoContent();
					}
					return Results.Ok(educations.Item3);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});

			app.MapGet("/education/{id}", async (EducationService educationService, int id) =>
			{
				var education = await educationService.GetEducation(id);
				if (education == null)
				{
					return Results.NotFound("Person not found");
				}
				return Results.Ok(education);
			});

			app.MapPost("/education", async (EducationService educationService, EducationDTO educationDTO, int personID) =>
			{
				var validationContext = new ValidationContext(educationDTO);
				var validationResult = new List<ValidationResult>();

				bool isValid = Validator.TryValidateObject(educationDTO, validationContext, validationResult, true);

				if (!isValid)
				{
					return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
				}

				try
				{
					var education = await educationService.CreateEducation(educationDTO, personID);
					return Results.Created($"/education/{education.ID}", educationDTO);
				}
				catch (InvalidOperationException ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});


			app.MapPut("/education/{id}", async (EducationService educationService, int id, EducationDTO educationDTO) =>
			{
				var validationContext = new ValidationContext(educationDTO);
				var validationResult = new List<ValidationResult>();

				bool isValid = Validator.TryValidateObject(educationDTO, validationContext, validationResult, true);

				if (!isValid)
				{
					return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
				}
				try
				{
					await educationService.PutEducation(educationDTO, id);
					return Results.Created($"/person/{id}", educationDTO);
				}
				catch (InvalidOperationException ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});

			app.MapDelete("/education/{id}", async (EducationService educationService, int id) =>
			{
				try
				{
					int rowsAffected = await educationService.DeleteEducation(id);
					if (rowsAffected == 0)
					{
						return Results.NotFound("Education not found");
					}
					return Results.Ok($"Education with id {id} was deleted.");
				}
				catch (Exception ex)
				{
					return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
				}
			});
		}
	}
}
