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
    public class FactureRepositoryTests
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ClientEntityFrameworkRepository<Facture> _factureRepository;

        public FactureRepositoryTests()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _factureRepository = new ClientEntityFrameworkRepository<Facture>(dbContext);
        }

        [Fact]
        public void Add_Facture_AddsFactureToDatabase()
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
            var facture = new Facture()
            {
                DateProduction = new DateTime(2017, 12, 12),
                DatePaiement = new DateTime(2017, 11, 11),
                ModePaiement = "Comptant",
                Contrat = contrat,
                IdContrat = contrat.Id
            };

            //Action
            _factureRepository.Add(facture);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public void Delete_ExistingFacture_DeletesFactureFromDatabase()
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
            var factures = new List<Facture>()
            {
                new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Comptant",
                    Contrat = contrat,
                    IdContrat = contrat.Id
                },
                 new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Debit",
                    Contrat = contrat,
                    IdContrat = contrat.Id
                },
            };

            var itemsCountBefore = factures.Count;
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.AddRange(factures);
                apiDbContext.SaveChanges();
            }

            //Action
            _factureRepository.Delete(factures.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        [Fact]
        public void Update_ExistingFacture_UpdateFactureFromDatabase()
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
            var facturesToAdd = new List<Facture>()
            {
                new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Comptant",
                    Contrat = contrat,
                    IdContrat = contrat.Id
                },
                 new Facture()
                {
                    DateProduction = new DateTime(2017, 12, 12),
                    DatePaiement = new DateTime(2017, 11, 11),
                    ModePaiement = "Debit",
                    Contrat = contrat,
                    IdContrat = contrat.Id
                },
            };

            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.AddRange(facturesToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var facture = _factureRepository.GetById(facturesToAdd.ElementAt(0).Id);
            var dateTime = new DateTime(2017, 1, 9);
            facture.DatePaiement.Equals(dateTime);
            _factureRepository.Update(facture);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Factures.ToList().First().DatePaiement.Equals(dateTime);
            }
        }

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Clients.RemoveRange(dbContext.Clients);
            dbContext.Groupes.RemoveRange(dbContext.Groupes);
            dbContext.Factures.RemoveRange(dbContext.Factures);
            dbContext.Contrats.RemoveRange(dbContext.Contrats);
            dbContext.SaveChanges();
        }
    }
}
