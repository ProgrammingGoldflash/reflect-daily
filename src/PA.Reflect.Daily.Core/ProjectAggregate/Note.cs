using System.ComponentModel.DataAnnotations.Schema;

namespace PA.Reflect.Daily.Core.ProjectAggregate;
public class Note : EntityBase
{
  public string Title { get; set; } = "";
  public string Filepath { get; private set; } = "";
  [NotMapped] public string MarkdownValue { get; set; } = "";
  public Note() { }
  public Note(string title, string markdownValue, string filepath)
  {
    Title = title;
    Filepath = filepath;
    MarkdownValue = markdownValue;
  }
}
