using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Infrastructure.Data.Config;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.Property(t => t.Username)
        .IsRequired();
    builder.Property(t => t.ProfilePictureUri)
        .IsRequired(false);
  }
}
