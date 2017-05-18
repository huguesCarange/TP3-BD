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
    public class ClientRepositoryTests
    {
        private ApplicationDbContextFactory _dbContextFactory;
        private readonly DbContextOptionsBuilder<ApplicationDbContext> _dbContextOptionsBuilder;
        private readonly ClientEntityFrameworkRepository<Client> _clientRepository;

        public ClientRepositoryTests()
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

        private void ClearAllTables(ApplicationDbContext dbContext)
        {
            dbContext.Clients.RemoveRange(dbContext.Clients);
            dbContext.SaveChanges();
        }
    }
}
