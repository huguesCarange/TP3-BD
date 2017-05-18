using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using app.persistence;

namespace app.persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170518162929_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.2");

            modelBuilder.Entity("app.domain.Entities.Artiste", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NAS")
                        .IsRequired();

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("NomDeScene");

                    b.Property<string>("Prenom")
                        .IsRequired();

                    b.Property<string>("Telephone");

                    b.HasKey("Id");

                    b.HasIndex("NAS")
                        .IsUnique();

                    b.ToTable("Artistes");
                });

            modelBuilder.Entity("app.domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("Prenom")
                        .IsRequired();

                    b.Property<string>("Telephone")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("app.domain.Entities.Contrat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cachet");

                    b.Property<DateTime>("DateContrat");

                    b.Property<DateTime>("DatePresentation");

                    b.Property<int>("Depot");

                    b.Property<DateTime>("HeureDebut");

                    b.Property<DateTime>("HeureFin");

                    b.Property<int>("IdClient");

                    b.Property<int>("IdGroupe");

                    b.Property<int>("MontantFinal");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdGroupe");

                    b.ToTable("Contrats");
                });

            modelBuilder.Entity("app.domain.Entities.Facture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DatePaiement");

                    b.Property<DateTime>("DateProduction");

                    b.Property<int>("IdContrat");

                    b.Property<string>("ModePaiement");

                    b.HasKey("Id");

                    b.HasIndex("IdContrat");

                    b.ToTable("Factures");
                });

            modelBuilder.Entity("app.domain.Entities.Groupe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cachet")
                        .IsRequired();

                    b.Property<string>("Nom");

                    b.HasKey("Id");

                    b.HasIndex("Nom")
                        .IsUnique();

                    b.ToTable("Groupes");
                });

            modelBuilder.Entity("app.domain.Entities.Membre", b =>
                {
                    b.Property<int>("IdArtiste");

                    b.Property<int>("IdGroupe");

                    b.HasKey("IdArtiste", "IdGroupe");

                    b.HasIndex("IdArtiste");

                    b.HasIndex("IdGroupe");

                    b.ToTable("Membres");
                });

            modelBuilder.Entity("app.domain.Entities.Contrat", b =>
                {
                    b.HasOne("app.domain.Entities.Client", "Client")
                        .WithMany("Contrats")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("app.domain.Entities.Groupe", "Groupe")
                        .WithMany("Contrats")
                        .HasForeignKey("IdGroupe")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("app.domain.Entities.Facture", b =>
                {
                    b.HasOne("app.domain.Entities.Contrat", "Contrat")
                        .WithMany()
                        .HasForeignKey("IdContrat")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("app.domain.Entities.Membre", b =>
                {
                    b.HasOne("app.domain.Entities.Artiste", "Artiste")
                        .WithMany("Membres")
                        .HasForeignKey("IdArtiste")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("app.domain.Entities.Groupe", "Groupe")
                        .WithMany("Membres")
                        .HasForeignKey("IdGroupe")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
