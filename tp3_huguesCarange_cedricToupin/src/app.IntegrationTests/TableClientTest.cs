using System;
using System.Collections.Generic;
using System.Linq;
using app.domain;
using app.domain.Entities;
using app.persistence;
using Bogus.DataSets;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace app.IntegrationTests
{
    public class TableClientTest
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ClientEntityFrameworkRepository<Client> _clientRepository;

        public TableClientTest()
        {
            _dbContextFactory = new ApplicationDbContextFactory();

            var dbContext = _dbContextFactory.Create();
            ClearAllTables(dbContext);
            _clientRepository = new ClientEntityFrameworkRepository<Client>(dbContext);
        }

        [Fact]
        public void Add_Client_AddsClientToDatabase()
        {
            //Arrange
            var client = new Client()
            {
                Nom = "Carange",
                Prenom = "Hugues",
                Telephone = "418-123-4567"
            };

            //Action
            _clientRepository.Add(client);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Clients.ToList().Count.Should().Be(1);
            }
        }

        [Fact]
        public void Remove_ExistingClient_DeletesClientFromDatabase()
        {
            //Arrange
            var clients = new List<Client>()
            {
                new Client()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567"
                },
                 new Client()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654"
                },
            };

            var itemsCountBefore = clients.Count;
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Clients.AddRange(clients);
                apiDbContext.SaveChanges();
            }

            //Action
            _clientRepository.Delete(clients.ElementAt(1));

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Clients.ToList().Count.Should().Be(itemsCountBefore - 1);
            }
        }

        [Fact]
        public void Update_ExistingClient_UpdateClientFromDatabase()
        {
            //Arrange
            var clientsToAdd = new List<Client>()
            {
                new Client()
                {
                    Nom = "Carange",
                    Prenom = "Hugues",
                    Telephone = "418-123-4567"
                },

                new Client()
                {
                    Nom = "Bob",
                    Prenom = "Dylan",
                    Telephone = "418-098-7654"
                }
            };

            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Clients.AddRange(clientsToAdd);
                apiDbContext.SaveChanges();
            }

            //Action
            var client = _clientRepository.GetById(clientsToAdd.ElementAt(0).Id);
            client.Telephone.Equals("418-123-1234");
            _clientRepository.Update(client);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Clients.ToList().First().Telephone.Equals("418-123-1234");
            }
        }

        [Fact]
        public void Delete_ExistingClient_CascadeDeleteAllContrats()
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


            var itemsCountBefore = contrats.Count();
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.AddRange(contrats);
                apiDbContext.Factures.AddRange(factures);
                apiDbContext.SaveChanges();
            }

            //Action
            _clientRepository.Delete(client);

            // Assert
            using (var apiDbContext = _dbContextFactory.Create())
            {
                apiDbContext.Contrats.ToList().Count.Should().Be(0);
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
