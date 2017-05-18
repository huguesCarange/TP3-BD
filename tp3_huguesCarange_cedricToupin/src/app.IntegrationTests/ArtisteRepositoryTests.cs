using System;
using System.Collections.Generic;
using System.Linq;
using app.domain.Entities;
using app.persistence;
using Bogus.DataSets;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace app.IntegrationTests
{
    public class ArtisteRepositoryTests
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ArtisteEntityFrameworkRepository<Artiste> _artisteRepository;

        public ArtisteRepositoryTests()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _artisteRepository = new ArtisteEntityFrameworkRepository<Artiste>(dbContext);
        }

        [Fact]
        public void Add_Artiste_AddsArtisteToDatabase()
        {
            //Arrange
            var artiste = new Artiste()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567",
                NAS = "ABC-1D2-1G5",
                NomDeScene = "Croustillant"
            };

            //Action
            _artisteRepository.Add(artiste);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Artistes.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public void Remove_ExistingArtiste_DeletesArtisteFromDatabase()
        {
            //Arrange
            var artistes = new List<Artiste>()
            {
                new Artiste()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567",
                    NAS = "ABC-1D2-1G5",
                    NomDeScene = "Croustillant"
                },
                 new Artiste()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654",
                    NAS = "AL2-1G4-1A8",
                    NomDeScene = "Sauce"
                },
            };

            var itemsCountBefore = artistes.Count;
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Artistes.AddRange(artistes);
                apiDbContext.SaveChanges();
            }

            //Action
            _artisteRepository.Delete(artistes.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Artistes.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        [Fact]
        public void Update_ExistingArtiste_UpdateArtisteFromDatabase()
        {
            //Arrange
            var artistesToAdd = new List<Artiste>()
            {
                new Artiste()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567",
                    NAS = "ABC-1D2-1G5",
                    NomDeScene = "Croustillant"
                },

                new Artiste()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654",
                    NAS = "JKC-1F2-1U5",
                    NomDeScene = "Sauce"
                }
            };
            var artisteToUpdate = new Artiste
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-1234", //modification ici
                NAS = "ABC-1D2-1G5",
                NomDeScene = "Croustillant"
            };
            

            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Artistes.AddRange(artistesToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var artiste = _artisteRepository.GetById(artistesToAdd.ElementAt(0).Id);
            artiste.Telephone.
            _artisteRepository.Update(artisteToUpdate);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Artistes.ToList().First().Equals(artisteToUpdate);
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Artistes.RemoveRange(dbContext.Artistes);
            dbContext.SaveChanges();
        }
    }
}
