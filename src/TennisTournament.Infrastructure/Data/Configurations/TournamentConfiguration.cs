using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad Tournament.
    /// </summary>
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        /// <summary>
        /// Configura el mapeo de la entidad Tournament.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.ToTable("Tournaments");
            
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.Type)
                .IsRequired()
                .HasColumnType("int");
                
            builder.Property(t => t.StartDate)
                .IsRequired()
                .HasColumnType("datetime2");
                
            builder.Property(t => t.EndDate)
                .HasColumnType("datetime2");
                
            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("int");
                
            // Relación muchos a muchos con Player
            builder.HasMany(t => t.Players)
                .WithMany()
                .UsingEntity(j => j.ToTable("TournamentPlayers"));
                
            // Relación uno a muchos con Match
            builder.HasMany(t => t.Matches)
                .WithOne(m => m.Tournament)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}