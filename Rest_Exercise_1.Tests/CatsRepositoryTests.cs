using Rest_Exercise_1.Models;
using Rest_Exercise_1.Repositories;
using System;
using System.Linq;
using Xunit;

namespace Rest_Exercise_1.Tests
{
    public class CatsRepositoryTests
    {
        [Fact]
        public void Constructor_ShouldSeedThreeMockCats()
        {
            var repo = new CatsRepository();

            var all = repo.GetAll();

            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Add_ShouldAssignIncrementingId()
        {
            var repo = new CatsRepository();

            var created = repo.Add(new Cat { Name = "Whiskers", Age = 2, Weight = 3.5 });

            Assert.Equal(4, created.Id); // 3 seeded + 1
        }

        [Fact]
        public void GetById_ShouldReturnCat_WhenExists()
        {
            var repo = new CatsRepository();
            var created = repo.Add(new Cat { Name = "Whiskers", Age = 2, Weight = 3.5 });

            var found = repo.GetById(created.Id);

            Assert.NotNull(found);
            Assert.Equal("Whiskers", found!.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenCatDoesNotExist()
        {
            var repo = new CatsRepository();

            var result = repo.GetById(9999);

            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldModifyExistingCat()
        {
            var repo = new CatsRepository();
            var created = repo.Add(new Cat { Name = "Whiskers", Age = 2, Weight = 3.5 });

            var updated = repo.Update(created.Id, new Cat { Name = "Whiskers Updated", Age = 3, Weight = 4.0 });

            Assert.NotNull(updated);
            Assert.Equal("Whiskers Updated", updated!.Name);
            Assert.Equal(3, updated.Age);
            Assert.Equal(4.0, updated.Weight);
        }

        [Fact]
        public void Update_ShouldReturnNull_WhenCatDoesNotExist()
        {
            var repo = new CatsRepository();

            var result = repo.Update(9999, new Cat { Name = "Ghost" });

            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldRemoveCat_WhenExists()
        {
            var repo = new CatsRepository();
            var created = repo.Add(new Cat { Name = "Whiskers", Age = 2, Weight = 3.5 });

            var deleted = repo.Delete(created.Id);

            Assert.NotNull(deleted);
            Assert.Null(repo.GetById(created.Id));
        }

        [Fact]
        public void Delete_ShouldReturnNull_WhenCatDoesNotExist()
        {
            var repo = new CatsRepository();

            var result = repo.Delete(9999);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null, null, null, 3)]          // no filters -> all seeded cats
        [InlineData(5, null, null, 2)]              // Garfield (8.5) and Felix (5.0)
        [InlineData(null, 5, null, 2)]              // Tom (4.2) and Felix (5.0)
        [InlineData(null, null, "om", 1)]           // "Tom" matches
        public void GetAllCats_ShouldFilterCorrectly(int? min, int? max, string? nameFilter, int expectedCount)
        {
            var repo = new CatsRepository();

            var result = repo.GetAllCats(min, max, nameFilter);

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void GetAllCats_ShouldThrow_WhenMinimumGreaterThanMaximum()
        {
            var repo = new CatsRepository();

            Assert.Throws<ArgumentException>(() => repo.GetAllCats(10, 5, null));
        }

        [Fact]
        public void GetAllCats_NameFilter_ShouldBeCaseInsensitive()
        {
            var repo = new CatsRepository();

            var result = repo.GetAllCats(null, null, "GARFIELD");

            Assert.Single(result);
            Assert.Equal("Garfield", result.First().Name);
        }
    }
}
