using app.domain;
using app.persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace app.console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = dbContextFactory.Create(new DbContextFactoryOptions());

            var companiesRepositories = new ArtisteEntityFrameworkRepository(dbContext);

        }
    }
}