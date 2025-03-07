
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.Models
{
    class PersonEndpoints 
    {
        public static void RegisterEndpoints(WebApplication app) 
        {
            app.MapGet("/person", async (PersonService personService, int page = 1) =>
            {
                try
                {
                    page = Math.Max(1, page);
                    var persons = await personService.GetAllPersonsPagination(page);
                    if (persons.Item3.Count < 1)
                    {
                        return Results.NoContent();
                    }
                    return Results.Ok(persons.Item3);
                }
                catch (Exception ex) { 
                    Console.WriteLine(ex);
                    return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
                }
            });

            app.MapGet("/person/{id}", async (PersonService personService, int id) =>
            {
                var person = await personService.GetPerson(id);
                if (person == null)
                {
                    return Results.NotFound("Person not found");
                }
                return Results.Ok(person);
            });

            app.MapPost("/person", async (PersonService personService, PersonCreateDTO personDTO) =>
            {
                var validationContext = new ValidationContext(personDTO);
                var validationResult = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(personDTO, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
                }

                try 
                { 
                    var person = await personService.CreatePerson(personDTO); 
                    return Results.Created($"/person/{person.ID}", personDTO);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
                }
            });


            app.MapPut("/person/{id}", async (PersonService personService, int id, PersonCreateDTO personDTO) =>
            {
                var validationContext = new ValidationContext(personDTO);
                var validationResult = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(personDTO, validationContext, validationResult, true);

                if (!isValid)
                {
                    return Results.BadRequest(validationResult.Select(v => v.ErrorMessage));
                }
                try
                {
                    await personService.PutPerson(personDTO); 
                    return Results.Created($"/person/{id}", personDTO);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
                }
            });

            app.MapDelete("/person/{id}", async (PersonService personService, int id) =>
            {
                try
                {
                    int rowsAffected = await personService.DeletePerson(id); 
                    if(rowsAffected == 0)
                    {
                        return Results.NotFound("Person not found");
                    }
                    return Results.Ok($"A person with id {id} was deleted.");
                }
                catch (Exception ex)
                {
                    return Results.Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
                }
            });
        }
    }
}