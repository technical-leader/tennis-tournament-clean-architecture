using Microsoft.EntityFrameworkCore;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data
{
    public class TournamentDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Tournament> Tournaments { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Result> Results { get; set; } = null!;
        
        public DbSet<MalePlayer> MalePlayers { get; set; } = null!;
        public DbSet<FemalePlayer> FemalePlayers { get; set; } = null!;

        public TournamentDbContext(DbContextOptions<TournamentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para Player
            modelBuilder.Entity<Player>()
                .HasDiscriminator(p => p.PlayerType)
                .HasValue<MalePlayer>(Domain.Enums.PlayerType.Male)
                .HasValue<FemalePlayer>(Domain.Enums.PlayerType.Female);

            modelBuilder.Entity<Player>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configuración para Tournament
            modelBuilder.Entity<Tournament>()
                .Property(t => t.Type)
                .IsRequired();

            // Configuración para acceder a las colecciones privadas
            var playersNavigation = modelBuilder.Entity<Tournament>()
                .Metadata.FindNavigation(nameof(Tournament.Players));
            
            if (playersNavigation != null)
            {
                playersNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            }

            var matchesNavigation = modelBuilder.Entity<Tournament>()
                .Metadata.FindNavigation(nameof(Tournament.Matches));
            
            if (matchesNavigation != null)
            {
                matchesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            }

            // Configuración para Result
            var resultMatchesNavigation = modelBuilder.Entity<Result>()
                .Metadata.FindNavigation(nameof(Result.Matches));
            
            if (resultMatchesNavigation != null)
            {
                resultMatchesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            }

            // Configuración para Match
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}