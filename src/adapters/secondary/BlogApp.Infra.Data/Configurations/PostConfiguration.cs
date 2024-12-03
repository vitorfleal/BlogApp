using BlogApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Infra.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(nameof(Post));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Title).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Content).IsRequired();

        builder.HasOne(x => x.User)
                 .WithMany(x => x.Posts)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.ClientCascade);
    }
}