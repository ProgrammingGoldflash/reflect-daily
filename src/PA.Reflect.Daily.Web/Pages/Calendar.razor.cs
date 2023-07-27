using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PA.Reflect.Daily.Core.Aggregates;
using PA.Reflect.Daily.Core.Interfaces;
using PA.Reflect.Daily.Core.ProjectAggregate;
using PA.Reflect.Daily.Web.Interfaces;
using PA.Reflect.Daily.Web.Options;
using System.Globalization;

namespace PA.Reflect.Daily.Web.Pages;

public partial class Calendar
{
  [CascadingParameter] public User User { get; set; } = new();
  [Inject] public IUserService UserService { get; set; } = default!;
  [Inject] public IUnitOfWork UnitOfWork { get; set; } = default!;
  [Inject] public ILocalStorageService LocalStorage { get; set; } = default!;

  private int weeksInMonth => (int)Math.Ceiling((startOfMonth.AddMonths(1) - startOfMonth.AddDays(-(int)startOfMonth.DayOfWeek)).TotalDays / 7);
  private List<Reflection> reflections = new();
  private DateTime startOfMonth { get; set; }

  private int[] months = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
  private LocalStorageOptions options = default!;
  private int currentMonth = DateTime.Now.Month;
  private int currentYear = DateTime.Now.Year;
  private DayOfWeek[] days = default!;

  protected override async Task OnParametersSetAsync()
  {
    startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    days = Enum.GetValues<DayOfWeek>();

    reflections = await UnitOfWork.ReflectionRepository.ListAsync(new ReflectionsByUserId(User.Id));

    StateHasChanged();
    return;
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender && await LocalStorage.ContainsOptionsAsync())
    {
      options = await LocalStorage.GetOptionsAsync();

      // move sunday to last index
      if (options.UseSundayAsFirstDayOfWeek is false)
      {
        days = days.Skip(1).Concat(days.Take(1)).ToArray();
        StateHasChanged();
      }
    }
  }

  private void PreviousYear()
  {
    startOfMonth = startOfMonth.AddYears(-1);
    currentMonth = startOfMonth.Month;
    currentYear = startOfMonth.Year;
  }

  private void NextYear()
  {
    startOfMonth = startOfMonth.AddYears(1);
    currentMonth = startOfMonth.Month;
    currentYear = startOfMonth.Year;
  }

  private void SetMonth(int month)
  {
    startOfMonth = new DateTime(startOfMonth.Year, month, 1);
    currentMonth = startOfMonth.Month;
  }

  private int GetWeekOfYear(DateTime dt)
  {
    var calendar = CultureInfo.InvariantCulture.Calendar;
    return calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
  }
}
