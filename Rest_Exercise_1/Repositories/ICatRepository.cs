using Rest_Exercise_1.Models;
using System.Collections.Generic;

namespace Rest_Exercise_1.Repositories
{
    public interface ICatRepository
    {
        List<Cat> GetAll();
        Cat? GetById(int id);
        Cat Add(Cat newCat);
        Cat? Update(int id, Cat updates);
        Cat? Delete(int id);
    }
}
