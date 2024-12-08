using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GraphQLProject.DatabaseConnections.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("menu");
        builder.HasKey(e => e.Id).HasName("pk_menu");
        builder.Property(x => x.Id).IsUnicode(false);
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.Description).HasMaxLength(500).IsUnicode(false);
        builder.Property(e => e.Price).HasColumnType("decimal(18,2)");// Example for precision: 18 digits, 2 decimal places
    }
}
