using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad FemalePlayer.
    /// </summary>
    public class FemalePlayerConfiguration : IEntityTypeConfiguration<FemalePlayer>
    {
        /// <summary>
        /// Configura el mapeo de la entidad FemalePlayer.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<FemalePlayer> builder)
        {
            builder.Property(p => p.ReactionTime)
                .IsRequired()
                .HasColumnType("int");
        }
    }
}