using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad Match.
    /// </summary>
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        /// <summary>
        /// Configura el mapeo de la entidad Match.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");
            
            builder.HasKey(m => m.Id);
            
            builder.Property(m => m.Round)
                .IsRequired()
                .HasColumnType("int");
                
            builder.Property(m => m.MatchDate)
                .IsRequired()
                .HasColumnType("datetime2");
                
            // Relación con Tournament
            builder.HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Relación con Player1
            builder.HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Relación con Player2
            builder.HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);
                
            // Relación con Winner (opcional)
            builder.HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}