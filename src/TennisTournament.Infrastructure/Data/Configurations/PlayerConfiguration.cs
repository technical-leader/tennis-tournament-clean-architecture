using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisTournament.Domain.Entities;

namespace TennisTournament.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuración de Entity Framework Core para la entidad Player.
    /// </summary>
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        /// <summary>
        /// Configura el mapeo de la entidad Player.
        /// </summary>
        /// <param name="builder">Builder para la configuración de la entidad.</param>
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("Players");
            
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(p => p.SkillLevel)
                .IsRequired()
                .HasColumnType("int");
                
            // Configuración para TPH (Table-per-Hierarchy)
            builder.HasDiscriminator<string>("PlayerType")
                .HasValue<MalePlayer>("Male")
                .HasValue<FemalePlayer>("Female");
        }
    }
}