using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad MalePlayer.
    /// </summary>
    public class MalePlayerConfiguration : IEntityTypeConfiguration<MalePlayer>
    {
        /// <summary>
        /// Configura el mapeo de la entidad MalePlayer.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<MalePlayer> builder)
        {
            builder.Property(p => p.Strength)
                .IsRequired()
                .HasColumnType("int");
                
            builder.Property(p => p.Speed)
                .IsRequired()
                .HasColumnType("int");
        }
    }
}