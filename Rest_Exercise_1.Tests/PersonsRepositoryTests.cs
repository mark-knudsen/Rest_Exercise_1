using Rest_Exercise_1.Models;
using Rest_Exercise_1.Repositories;
using Xunit;

namespace Rest_Exercise_1.Tests
{
    public class PersonsRepositoryTests
    {
        [Fact]
        public void Constructor_ShouldSeedThreeMockPersons()
        {
            var repo = new PersonsRepository();

            var all = repo.GetAll();

            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Add_ShouldAssignIncrementingId()
        {
            var repo = new PersonsRepository();

            var created = repo.Add(new Person { Name = "Alice", Age = 30 });

            Assert.Equal(4, created.Id); // 3 seeded + 1
        }

        [Fact]
        public void Add_ShouldStorePersonInRepository()
        {
            var repo = new PersonsRepository();

            var created = repo.Add(new Person { Name = "Alice", Age = 30 });

            Assert.Contains(repo.GetAll(), p => p.Id == created.Id && p.Name == "Alice");
        }

        [Fact]
        public void GetById_ShouldReturnPerson_WhenExists()
        {
            var repo = new PersonsRepository();
            var created = repo.Add(new Person { Name = "Alice", Age = 30 });

            var found = repo.GetById(created.Id);

            Assert.NotNull(found);
            Assert.Equal("Alice", found!.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            var repo = new PersonsRepository();

            var result = repo.GetById(9999);

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldModifyExistingPerson()
        {
            var repo = new PersonsRepository();
            var created = repo.Add(new Person { Name = "Alice", Age = 30, Address = "Old Address", PhoneNumber = "000", IsEmployed = false });

            var updated = repo.Update(created.Id, new Person
            {
                Name = "Alice Updated",
                Age = 31,
                Address = "New Address",
                PhoneNumber = "111",
                IsEmployed = true
            });

            Assert.NotNull(updated);
            Assert.Equal("Alice Updated", updated!.Name);
            Assert.Equal(31, updated.Age);
            Assert.Equal("New Address", updated.Address);
            Assert.Equal("111", updated.PhoneNumber);
            Assert.True(updated.IsEmployed);
        }

        [Fact]
        public void Update_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            var repo = new PersonsRepository();

            var result = repo.Update(9999, new Person { Name = "Ghost" });

            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldRemovePerson_WhenExists()
        {
            var repo = new PersonsRepository();
            var created = repo.Add(new Person { Name = "Bob", Age = 25 });

            var deleted = repo.Delete(created.Id);

            Assert.NotNull(deleted);
            Assert.Null(repo.GetById(created.Id));
        }

        [Fact]
        public void Delete_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            var repo = new PersonsRepository();

            var result = repo.Delete(9999);

            Assert.Null(result);
        }
    }
}
