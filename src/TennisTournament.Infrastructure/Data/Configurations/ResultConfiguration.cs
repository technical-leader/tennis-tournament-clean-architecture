using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad Result.
    /// </summary>
    public class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        /// <summary>
        /// Configura el mapeo de la entidad Result.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.ToTable("Results");
            
            builder.HasKey(r => r.Id);
            
            // Relación con Tournament
            builder.HasOne(r => r.Tournament)
                .WithOne()
                .HasForeignKey<Result>(r => r.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Relación con Winner
            builder.HasOne(r => r.Winner)
                .WithMany()
                .HasForeignKey(r => r.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}