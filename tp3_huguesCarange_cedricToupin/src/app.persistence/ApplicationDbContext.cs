using app.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Artiste> Artistes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contrat> Contrats { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<Groupe> Groupes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Membre>()
                .HasKey(bc => new { bc.IdArtiste, bc.IdGroupe });

            modelBuilder.Entity<Membre>()
                .HasOne(bc => bc.Artiste)
                .WithMany(b => b.Membres)
                .HasForeignKey(bc => bc.IdArtiste);

            modelBuilder.Entity<Membre>()
                .HasOne(bc => bc.Groupe)
                .WithMany(c => c.Membres)
                .HasForeignKey(bc => bc.IdGroupe);

            modelBuilder
                .Entity<Artiste>()
                .HasIndex(t => t.NAS)
                .IsUnique();

            modelBuilder
                .Entity<Groupe>()
                .HasIndex(t => t.Nom)
                .IsUnique();
        }
    }
}
