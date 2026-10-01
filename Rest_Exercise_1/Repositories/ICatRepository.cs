using Rest_Exercise_1.Models;
using System.Collections.Generic;
using Rest_Exercise_1.Models;
using System;

namespace Rest_Exercise_1.Repositories
{
    public interface ICatRepository
    {
        List<Cat> GetAll();
        IEnumerable<Cat> GetAllCats(int? minimumweight, int? maximumweight, string? nameFilter);
        Cat? GetById(int id);
        Cat Add(Cat newCat);
        Cat? Update(int id, Cat updates);
        Cat? Delete(int id);
    }
}
