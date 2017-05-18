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
    public class TableContratTest
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ClientEntityFrameworkRepository<Contrat> _contratRepository;

        public TableContratTest()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _contratRepository = new ClientEntityFrameworkRepository<Contrat>(dbContext);
        }

        [Fact]
        public void Add_Contrat_AddsContratToDatabase()
        {
            //Arrange
            var client = new Client()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567"
            };
            var groupe = new Groupe()
            {
                Nom = "Sex Pistols",
                Cachet = 10000,
            };
            var contrat = new Contrat()
            {
                Groupe = groupe,
                IdGroupe = groupe.Id,
                Client = client,
                IdClient = client.Id,
                DateContrat = new DateTime(2017, 12, 12),
                DatePresentation = new DateTime(2017, 11, 11),
                HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                Depot = 11334,
                Cachet = 2344,
                MontantFinal = 23545
            };

            //Action
            _contratRepository.Add(contrat);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public void Delete_ExistingContrat_DeletesContratFromDatabase()
        {
            //Arrange
            var client = new Client()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567"
            };
            var groupe = new Groupe()
            {
                Nom = "Sexy Pistols",
                Cachet = 10000,
            };
            var contrats = new List<Contrat>()
            {
                new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 11334,
                    Cachet = 2344,
                    MontantFinal = 23545
                },
                 new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 114,
                    Cachet = 24,
                    MontantFinal = 235
                },
            };

            var itemsCountBefore = contrats.Count;
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.AddRange(contrats);
                apiDbContext.SaveChanges();
            }

            //Action
            _contratRepository.Delete(contrats.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        [Fact]
        public void Update_ExistingContrat_UpdateContratFromDatabase()
        {
            //Arrange
            var client = new Client()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567"
            };
            var groupe = new Groupe()
            {
                Nom = "Sex Pistols",
                Cachet = 10000,
            };
            var contratsToAdd = new List<Contrat>()
            {
                new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 11334,
                    Cachet = 2344,
                    MontantFinal = 23545
                },
                 new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 114,
                    Cachet = 24,
                    MontantFinal = 235
                },
            };

            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.AddRange(contratsToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var contrat = _contratRepository.GetById(contratsToAdd.ElementAt(0).Id);
            contrat.MontantFinal.Equals(2345);
            _contratRepository.Update(contrat);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.ToList().First().MontantFinal.Equals(2345);
            }
        }

        [Fact]
        public void Delete_ExistingContrat_CascadeDeleteFacture()
        {
            //Arrange
            var client = new Client()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567"
            };
            var groupe = new Groupe()
            {
                Nom = "tro11ll Pistols",
                Cachet = 10000,
            };
            var contrats = new List<Contrat>()
            {
                new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 11334,
                    Cachet = 2344,
                    MontantFinal = 23545
                },
                 new Contrat()
                {
                    Groupe = groupe,
                    IdGroupe = groupe.Id,
                    Client = client,
                    IdClient = client.Id,
                    DateContrat = new DateTime(2017, 12, 12),
                    DatePresentation = new DateTime(2017, 11, 11),
                    HeureDebut = new DateTime(2017, 11, 11, 12, 24, 43),
                    HeureFin = new DateTime(2017, 11, 11, 12, 24, 44),
                    Depot = 114,
                    Cachet = 24,
                    MontantFinal = 235
                },
            };
            var factures = new List<Facture>()
            {
                new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Comptant",
                    Contrat = contrats.ElementAt(0),
                    IdContrat = contrats.ElementAt(0).Id
                },
                new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Comptant",
                    Contrat = contrats.ElementAt(1),
                    IdContrat = contrats.ElementAt(1).Id
                }
            };
                

            var itemsCountBefore = factures.Count();
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.AddRange(contrats);
                apiDbContext.Factures.AddRange(factures);
                apiDbContext.SaveChanges();
            }

            //Action
            _contratRepository.Delete(contrats.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Contrats.RemoveRange(dbContext.Contrats);
            dbContext.Groupes.RemoveRange(dbContext.Groupes);
            dbContext.Clients.RemoveRange(dbContext.Clients);
            dbContext.SaveChanges();
        }
    }
}
