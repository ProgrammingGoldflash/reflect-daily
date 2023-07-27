using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.ServiceLayer.Interfaces;

namespace PA.Reflect.Daily.Web.Pages;

public partial class ReflectionDetails
{
  [Inject] public IReflectionService ReflectionService { get; set; } = default!;
  [Inject] public NavigationManager navigationManager { get; set; } = default!;
  [CascadingParameter] public User User { get; set; } = new();
  [Parameter] public string Date { get; set; } = string.Empty;

  private Snackbar snackbarPrimary = new();
  private Reflection reflection = new();
  private Modal modalRef = new();
  private bool isNew = true;
  private Note note = new();
  private DateOnly dateOnly;

  protected override async Task OnParametersSetAsync()
  {
    dateOnly = DateOnly.ParseExact(Date, "yyyy-MM-dd");
    reflection = new(default, dateOnly) { Id = default };

    var entity = await ReflectionService.GetReflectionAsync(User.Id, dateOnly);

    SetReflection(entity);

    StateHasChanged();
  }

  protected Task<string> PreviewRender(string plainText)
  {
    return Task.FromResult(Markdig.Markdown.ToHtml(note.MarkdownValue ?? string.Empty));
  }

  private void OpenModal() => modalRef.Show();
  private Task CancelModal() => modalRef.Hide();

  private void SetReflection(Reflection? entity)
  {
    if (entity != null)
    {
      reflection = entity;
      note = reflection.Notes.FirstOrDefault() ?? new();
      isNew = false;
    }
    else
    {
      reflection = new Reflection(User.Id, dateOnly);
      note = reflection.AddNote("Test");
    }
  }

  private async Task Delete()
  {
    await ReflectionService.DeleteReflectionAsync(reflection);

    await modalRef.Hide();

    navigationManager.NavigateTo("/");
  }

  private async Task Save()
  {
    if (isNew)
    {
      var entity = await ReflectionService.AddReflectionAsync(reflection);

      SetReflection(entity);
      StateHasChanged();
    }
    else
    {
      await ReflectionService.UpdateReflectionAsync(reflection);
    }

    await snackbarPrimary.Show();
  }

  Task OnMarkdownValueChanged(string value)
  {
    note.MarkdownValue = value;

    return Task.CompletedTask;
  }
}
