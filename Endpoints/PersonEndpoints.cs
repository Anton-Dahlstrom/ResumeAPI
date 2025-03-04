
using System.ComponentModel.DataAnnotations;

namespace ResumeAPI.Models
{
    class PersonEndpoints 
    {
        public static void RegisterEndpoints(WebApplication app) 
        {
            app.MapGet("/person", async (PersonService personService) =>
            {
                var persons = await personService.GetAllPersons();
                return Results.Ok(persons);
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
                var person = new Person
                {
                    Name = personDTO.Name,
                    Description = personDTO.Description,
                    Phone = personDTO.Phone,
                    Email = personDTO.Email
                };

                try 
                { 
                    await personService.CreatePerson(personDTO); 
                    return Results.Created($"/person/{person.ID}", person);
                }
                catch (InvalidOperationException err)
                {
                    return Results.BadRequest(err);
                }
            });


            app.MapPut("/person/{id}", async (PersonService personService, int id, PersonCreateDTO personDTO) =>
            {
                try
                {
                    await personService.PutPerson(personDTO); 
                    return Results.Created($"/person/{id}", personDTO);
                }
                catch (InvalidOperationException err)
                {
                    return Results.BadRequest(err);
                }
            });

            app.MapDelete("/person/{id}", async (PersonService personService, int id) =>
            {
                try
                {
                    var personDTO = await personService.DeletePerson(id); 
                    return Results.Ok(personDTO);
                }
                catch (InvalidOperationException err)
                {
                    return Results.BadRequest(err);
                }
            });
        }
    }
}