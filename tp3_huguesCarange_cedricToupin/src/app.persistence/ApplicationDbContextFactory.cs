using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace app.persistence
{
    public class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {

        // Design-time services will automatically discover implementations of this interface that are in the same assembly as the derived context.
        // Exemple de Design-time services: Update-Database
        // https://docs.efproject.net/en/latest/miscellaneous/configuring-dbcontext.html#use-idbcontextfactory

        public ApplicationDbContext Create(DbContextFactoryOptions options)
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            //Aussi, base de données hardcoded
            optionsBuilder.UseSqlite("Filename='C:/tempo/bd.sqlite'");
            //optionsBuilder.UseSqlite(configuration["DATABASE_CONNECTIONSTRING"]);      // Variable d'environnement

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        public ApplicationDbContext Create()
        {
            return Create(null);
        }
    }
}
