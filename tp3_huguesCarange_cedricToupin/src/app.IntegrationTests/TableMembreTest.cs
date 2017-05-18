using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Entities;
using app.persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace app.IntegrationTests
{
    public class TableMembreTest
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ArtisteEntityFrameworkRepository<Groupe> _membreRepository;

        public TableMembreTest()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _membreRepository = new ArtisteEntityFrameworkRepository<Groupe>(dbContext);
        }

        [Fact]
        public void Add_Membre_AddMembreToDatabase()
        {
            //Arrange
            var artistesToAdd = new List<Artiste>()
            {
                new Artiste()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567",
                    NAS = "2BC-1D2-1G5",
                    NomDeScene = "Croustillant"
                },

                new Artiste()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654",
                    NAS = "2KC-1F2-1U5",
                    NomDeScene = "Sauce"
                }
            };

            var groupeToAdd = new List<Groupe>()
            {
                new Groupe()
                {
                    Nom = "2pac",
                    Cachet = 100,
                }
            };

            var membresToAdd = new List<Membre>()
            {
                new Membre()
                {
                    Artiste = artistesToAdd.ElementAt(0),
                    Groupe = groupeToAdd.ElementAt(0),
                    IdArtiste = artistesToAdd.ElementAt(0).Id,
                    IdGroupe = groupeToAdd.ElementAt(0).Id,
                    Role = "Chanteur"
                },

                new Membre()
                {
                    Artiste = artistesToAdd.ElementAt(1),
                    Groupe = groupeToAdd.ElementAt(0),
                    IdArtiste = artistesToAdd.ElementAt(0).Id,
                    IdGroupe = groupeToAdd.ElementAt(0).Id,
                    Role = "Sifflet"
                }
            };


            //Action
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.AddRange(groupeToAdd);
                apiDbContext.Artistes.AddRange(artistesToAdd);
                //apiDbContext.Membres.AddRange(membresToAdd);
                apiDbContext.SaveChanges();
            }

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Membres.ToList().Count.Should().Be(1);
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Groupes.RemoveRange(dbContext.Groupes);
            dbContext.Artistes.RemoveRange(dbContext.Artistes);
            dbContext.Membres.RemoveRange(dbContext.Membres);
            dbContext.SaveChanges();
        }
    }
}
