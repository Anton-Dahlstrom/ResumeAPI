
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

        public async Task<List<PersonCreateDTO>> GetAllPersons()
        {
            var personList = await context.Persons.Select( p => new PersonCreateDTO
            {
                Name = p.Name,
                Description = p.Description,
                Phone = p.Phone,
                Email = p.Email
            }).ToListAsync();
            return personList;
        }
        public async Task<PersonCreateDTO?> CreatePerson(PersonCreateDTO personDTO)
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

            context.Add(personDTO);
            await context.SaveChangesAsync();
            return personDTO;
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
        public async Task<PersonGetDTO?> DeletePerson(int id)
        {
            var person = await context.Persons.FirstOrDefaultAsync(p => p.ID == id);
            if(person == null)
            {
                throw new InvalidOperationException("A person with this id doesn't exist.");
            }
            var personDTO = new PersonGetDTO
            {
                Name = person.Name,
                Description = person.Description,
                Phone = person.Phone,
                Email = person.Email
            };
            await context.SaveChangesAsync();
            return personDTO;
        } 
    }
}
