using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.Web.Options;

namespace PA.Reflect.Daily.Web.Pages;

public partial class Profile
{
  [CascadingParameter] public User User { get; set; } = default!;
  [Inject] public ILocalStorageService Localstorage { get; set; } = default!;

  private LocalStorageOptions localOptions = new LocalStorageOptions();
  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender && await Localstorage.ContainsOptionsAsync())
    {
      localOptions = await Localstorage.GetOptionsAsync();
      StateHasChanged();
    }
  }

  private async Task Save()
  {
    await Localstorage.SetOptionsAsync(localOptions);
  }
}
