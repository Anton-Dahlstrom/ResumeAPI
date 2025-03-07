
using Microsoft.EntityFrameworkCore;
using ResumeAPI.Data;

namespace ResumeAPI.Models
{
    public class PersonService
    {
        private readonly ResumeDBContext context;

        public PersonService(ResumeDBContext _context)
        {
            context = _context;
        }
        public async Task<PersonCreateDTO?> GetPerson(int id)
        {
            var person = await context.Persons.FirstOrDefaultAsync(p => p.ID == id);

            if(person == null)
            {
                 return null;
            }

            return new PersonCreateDTO
            {
                Name = person.Name,
                Description = person.Description,
                Phone = person.Phone,
                Email = person.Email
            };
        }

        public async Task<(int, int, List<PersonCreateDTO>)> GetAllPersonsPagination(int page)
        {
            int pagesize = 10;
            var totalPersonsTask = await context.Persons.CountAsync();

            var personListTask = await context.Persons
            .OrderBy(p => p.ID)
            .Skip((page-1) * pagesize)
            .Take(pagesize)
            .Select( p => new PersonCreateDTO
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

        public async Task<Person> CreatePerson(PersonCreateDTO personDTO)
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


        public async Task<PersonCreateDTO?> PutPerson(PersonCreateDTO personDTO)
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
