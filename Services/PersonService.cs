using Microsoft.EntityFrameworkCore;
using ResumeAPI.Data;
using ResumeAPI.DTO;
using ResumeAPI.Models;

namespace ResumeAPI.Services
{
	public class PersonService
	{
		private readonly ResumeDBContext context;

		public PersonService(ResumeDBContext _context)
		{
			context = _context;
		}
		public async Task<PersonGetAllDTO?> GetPerson(int id)
		{
			var person = await context.Persons
				.Where(p => p.ID == id)
				.Select(p => new PersonGetAllDTO
				{
					Name = p.Name,
					Description = p.Description,
					Phone = p.Phone,
					Email = p.Email,
					Jobs = p.Jobs.Select(j => new JobDTO
					{
						Company = j.Company,
						Title = j.Title,
						Description = j.Description,
						StartDate = j.StartDate,
						EndDate = j.EndDate
					}).ToList(),
					Educations = p.Educations.Select(e => new EducationDTO
					{
						School = e.School,
						Field = e.Field,
						Description = e.Description,
						StartDate = e.StartDate,
						EndDate = e.EndDate
					}).ToList()
				})
				.FirstOrDefaultAsync();
			return person;
		}

		public async Task<(int, int, List<PersonDTO>)> GetAllPersonsPagination(int page)
		{
			int pagesize = 10;
			var totalPersonsTask = await context.Persons.CountAsync();

			var personListTask = await context.Persons
			.OrderBy(p => p.ID)
			.Skip((page - 1) * pagesize)
			.Take(pagesize)
			.Select(p => new PersonDTO
			{
				Name = p.Name,
				Description = p.Description,
				Phone = p.Phone,
				Email = p.Email
			})
			.ToListAsync();

			Console.WriteLine(totalPersonsTask);
			Console.WriteLine(personListTask);
			return (page, totalPersonsTask, personListTask);
		}

		public async Task<Person> CreatePerson(PersonDTO personDTO)
		{
			var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == personDTO.Email);

			if (existingPerson != null)
			{
				throw new InvalidOperationException("A person with this email already exists.");
			}


			var person = new Person
			{
				Name = personDTO.Name,
				Description = personDTO.Description,
				Phone = personDTO.Phone,
				Email = personDTO.Email
			};

			context.Add(person);
			await context.SaveChangesAsync();
			return person;
		}


		public async Task<PersonDTO?> PutPerson(PersonDTO personDTO)
		{
			var existingPerson = await context.Persons.FirstOrDefaultAsync(p => p.Email == personDTO.Email);

			if (existingPerson == null)
			{
				throw new InvalidOperationException("A person with this id doesn't exist.");
			}

			existingPerson.Name = personDTO.Name;
			existingPerson.Description = personDTO.Description;
			existingPerson.Phone = personDTO.Phone;
			existingPerson.Email = personDTO.Email;
			await context.SaveChangesAsync();
			return personDTO;
		}
		public async Task<int> DeletePerson(int id)
		{
			int rowsAffected = await context.Persons.Where(p => p.ID == id).ExecuteDeleteAsync();
			return rowsAffected;
		}
	}
}
