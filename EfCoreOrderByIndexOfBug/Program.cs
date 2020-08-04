using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCoreOrderByIndexOfBug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            dbContextOptionsBuilder.UseSqlServer("Data Source = .\\SQLEXPRESS; Database = EfCoreOrderByIndexOfBug; Integrated Security = True;");

            var applicationDbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);
            applicationDbContext.Database.EnsureCreated();

            if (!applicationDbContext.Categories.Any())
            {
                for (var i = 0; i < 20; i++)
                {
                    applicationDbContext.Categories.Add(new Category { Name = $"Name {i}" });
                }
                applicationDbContext.SaveChanges();
            }

            var searchTerm = "Na";

            var names = applicationDbContext.Categories
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x.IndexOf(searchTerm))
                .ThenBy(x => x)
                .Take(10)
                .ToList();
        }
    }
}