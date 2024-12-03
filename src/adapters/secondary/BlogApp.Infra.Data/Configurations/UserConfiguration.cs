using BlogApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Infra.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Username).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Password).HasMaxLength(255).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
    }
}