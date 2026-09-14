using Rest_Exercise_1.Models;

using Rest_Exercise_1.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rest_Exercise_1.Repositories
{
    public class PersonsRepository : IPersonRepository
    {

        private int _nextId = 1;
        private readonly List<Person> _people = new();

        public PersonsRepository()
        {
            // Mock data til opstart
            Add(new Person { Name = "Garfield", Age = 5, Address = "123 Main St", PhoneNumber = "555-1234", IsEmployed = true });
            Add(new Person { Name = "Tom", Age = 3, Address = "456 Elm St", PhoneNumber = "555-5678", IsEmployed = false });
            Add(new Person { Name = "Felix", Age = 7, Address = "789 Oak St", PhoneNumber = "555-9012", IsEmployed = true });
        }

        // Read All (GET)
        public List<Person> GetAll()
        {
            return new List<Person>(_people);
        }

        // Read by ID (GET)
        public Person? GetById(int id)
        {
            return _people.FirstOrDefault(p => p.Id == id);
        }

        // Create (POST)
        public Person Add(Person newPerson)
        {
            newPerson.Id = _nextId++;
            _people.Add(newPerson);
            return newPerson;
        }


        // Update (PUT)
        public Person? Update(int id, Person updates)
        {
            Person? existingPerson = GetById(id);
            if (existingPerson == null) return null;

            existingPerson.Name = updates.Name;
            existingPerson.Age = updates.Age;
            existingPerson.Address = updates.Address;
            existingPerson.PhoneNumber = updates.PhoneNumber;
            existingPerson.IsEmployed = updates.IsEmployed;

            return existingPerson;
        }

        // Delete (DELETE)
        public Person? Delete(int id)
        {
            Person? existingPerson = GetById(id);
            if (existingPerson == null) return null;

            _people.Remove(existingPerson);
            return existingPerson;
        }
    }
}
