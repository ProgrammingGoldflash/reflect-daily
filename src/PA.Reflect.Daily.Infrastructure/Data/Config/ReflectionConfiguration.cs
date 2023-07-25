using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.Infrastructure.TypeHandlers;

namespace PA.Reflect.Daily.Infrastructure.Data.Config;
public class ReflectionConfiguration : IEntityTypeConfiguration<Reflection>
{
  public void Configure(EntityTypeBuilder<Reflection> builder)
  {
    builder.Property(p => p.Date)
      .HasConversion<DateOnlyConverter, DateOnlyComparer>()
      .IsRequired();

    builder.Property(p => p.UserId)
      .IsRequired();
  }
}
