using System.Globalization;
using Blazored.LocalStorage;

namespace PA.Reflect.Daily.Web.Options;

public class LocalStorageOptions
{
  public const string StorageKey = nameof(LocalStorageOptions);
  public bool UseSundayAsFirstDayOfWeek { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek == DayOfWeek.Sunday;
}

public static class StorageExtensions
{
  public static async Task<bool> ContainsOptionsAsync(this ILocalStorageService localStorage)
  {
    return await localStorage.ContainKeyAsync(LocalStorageOptions.StorageKey);
  }

  public static async Task<LocalStorageOptions> GetOptionsAsync(this ILocalStorageService localStorage)
  {
    return await localStorage.GetItemAsync<LocalStorageOptions>(LocalStorageOptions.StorageKey);
  }

  public static async Task SetOptionsAsync(this ILocalStorageService localStorage, LocalStorageOptions options)
  {
    await localStorage.SetItemAsync<LocalStorageOptions>(LocalStorageOptions.StorageKey, options);
  }
}
