using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Infrastructure.Data.Config;
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
  public void Configure(EntityTypeBuilder<Note> builder)
  {
    builder.Property(p => p.Title)
      .IsRequired();
    
    builder.Property(p => p.Filepath)
      .IsRequired();
  }
}
