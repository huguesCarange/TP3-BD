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
    public class GroupeRepositoryTests
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ArtisteEntityFrameworkRepository<Groupe> _groupeRepository;

        public GroupeRepositoryTests()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _groupeRepository = new ArtisteEntityFrameworkRepository<Groupe>(dbContext);
        }

        [Fact]
        public void Add_Groupe_AddsGroupeToDatabase()
        {
            //Arrange
            var groupe = new Groupe()
            {
                Nom = "Sex Pistols",
                Cachet = 10000,
            };

            //Action
            _groupeRepository.Add(groupe);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public void Delete_ExistingGroupe_DeletesGroupeFromDatabase()
        {
            //Arrange
            var groupes = new List<Groupe>()
            {
                new Groupe()
                {
                    Nom = "Jimmi Hendrix",
                    Cachet = 10000,
                },
                 new Groupe()
                {
                    Nom = "The Animals",
                    Cachet = 5000,
                },
            };

            var itemsCountBefore = groupes.Count;
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.AddRange(groupes);
                apiDbContext.SaveChanges();
            }

            //Action
            _groupeRepository.Delete(groupes.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        [Fact]
        public void Update_ExistingGroupe_UpdateGroupeFromDatabase()
        {
            //Arrange
            var groupesToAdd = new List<Groupe>()
            {
                new Groupe()
                {
                    Nom = "Jimmi Hendrix",
                    Cachet = 10000,
                },
                 new Groupe()
                {
                    Nom = "The Animals",
                    Cachet = 5000,
                },
            };

            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.AddRange(groupesToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var groupe = _groupeRepository.GetById(groupesToAdd.ElementAt(0).Id);
            groupe.Cachet.Equals("6969");
            _groupeRepository.Update(groupe);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.ToList().First().Cachet.Equals("6969");
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Groupes.RemoveRange(dbContext.Groupes);
            dbContext.SaveChanges();
        }
    }
}
