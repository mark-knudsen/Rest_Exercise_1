using Rest_Exercise_1.Models;
using System.Collections.Generic;

namespace Rest_Exercise_1.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();
        Person? GetById(int id);
        Person Add(Person newPerson);
        Person? Update(int id, Person updates);
        Person? Delete(int id);
    }
}