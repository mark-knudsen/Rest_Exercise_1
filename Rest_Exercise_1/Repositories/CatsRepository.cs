using Rest_Exercise_1.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rest_Exercise_1.Repositories
{
    public class CatsRepository : ICatRepository
    {
        private int _nextId = 1;
        private readonly List<Cat> _cats = new();

        public CatsRepository()
        {
            // Mock data til opstart
            Add(new Cat { Name = "Garfield", Age = 5, Weight = 8.5 });
            Add(new Cat { Name = "Tom", Age = 3, Weight = 4.2 });
            Add(new Cat { Name = "Felix", Age = 7, Weight = 5.0 });
        }


        // Read All (GET)
        public List<Cat> GetAll()
        {
            return new List<Cat>(_cats);
        }

        // Read by ID (GET)
        public Cat? GetById(int id)
        {
            return _cats.FirstOrDefault(c => c.Id == id);
        }

        // Create (POST)
        public Cat Add(Cat newCat)
        {
            newCat.Id = _nextId++;
            _cats.Add(newCat);
            return newCat;
        }

        // Update (PUT)
        public Cat? Update(int id, Cat updates)
        {
            Cat? existingCat = GetById(id);
            if (existingCat == null) return null;

            existingCat.Name = updates.Name;
            existingCat.Age = updates.Age;
            existingCat.Weight = updates.Weight;

            return existingCat;
        }

        // Delete (DELETE)
        public Cat? Delete(int id)
        {
            Cat? existingCat = GetById(id);
            if (existingCat == null) return null;

            _cats.Remove(existingCat);
            return existingCat;
        }
    }
}
