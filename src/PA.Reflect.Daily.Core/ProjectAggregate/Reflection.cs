using Ardalis.GuardClauses;
using PA.Reflect.Daily.Core.Interfaces;

namespace PA.Reflect.Daily.Core.ProjectAggregate;
public class Reflection : EntityBase, IAggregateRoot
{
  public int UserId { get; private set; }
  public DateOnly Date { get; private set; }
  private readonly List<Note> _notes = new();
  public IEnumerable<Note> Notes => _notes.AsReadOnly();

  public Reflection() { }
  public Reflection(int userId, DateOnly date)
  {
    UserId = userId;
    Date = date;
  }

  public Note AddNote(string title)
  {
    var note = new Note(title, "", $"/{UserId}/{this.Id}/{Guid.NewGuid()}.md");
    _notes.Add(note);

    return note;
  }

  public void UpdateDate(DateOnly newDate)
  {
    Date = newDate;
  }
}
