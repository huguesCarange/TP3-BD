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
    public class TableGroupTest
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ArtisteEntityFrameworkRepository<Groupe> _groupeRepository;

        public TableGroupTest()
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
                Nom = "Salut Pistols",
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

        [Fact]
        public void Delete_ExistingGroupe_CascadeDeleteAllMembres()
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



            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.AddRange(groupeToAdd);
                apiDbContext.Artistes.AddRange(artistesToAdd);
                apiDbContext.Membres.AddRange(membresToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var itemsCountBefore = membresToAdd.Count;
            _groupeRepository.Delete(groupeToAdd.ElementAt(0));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Membres.ToList().Count.Should().Be(0);
            }
        }

        [Fact]
        public void Delete_ExistingGroupe_CascadeDeleteAllArtistes()
        {
            //Arrange
            var artistesToAdd = new List<Artiste>()
            {
                new Artiste()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567",
                    NAS = "2BC-122-1G5",
                    NomDeScene = "Croustillant"
                },

                new Artiste()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654",
                    NAS = "2KC-142-1U5",
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



            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Groupes.AddRange(groupeToAdd);
                apiDbContext.Artistes.AddRange(artistesToAdd);
                apiDbContext.Membres.AddRange(membresToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var itemsCountBefore = artistesToAdd.Count;
            _groupeRepository.Delete(groupeToAdd.ElementAt(0));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Membres.ToList().Count.Should().Be(0);
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Groupes.RemoveRange(dbContext.Groupes);
            dbContext.SaveChanges();
        }
    }
}
