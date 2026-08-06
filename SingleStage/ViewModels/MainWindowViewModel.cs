using System;
using System.Collections.Generic;
using SingleStage.Entities;
using SingleStage.DAC;

namespace SingleStage.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ShowDAC _showDAC;

        public CalendarWeekViewModel? CalendarWeek { get; private set; }

        public MainWindowViewModel(ShowDAC showDAC)
        {
            _showDAC = showDAC;
        }

        public async Task InitializeAsync()
        {
            List<Show> shows = await _showDAC.GetAllAsync();

            DateTime weekStart = GetMonday(DateTime.Today);

            CalendarWeek = new CalendarWeekViewModel(
                weekStart,
                shows
                );

            OnPropertyChanged(nameof(CalendarWeek));
        }

        private static DateTime GetMonday(DateTime date)
        {
            int day = (int)date.DayOfWeek;

            // Convert Sunday (0) to 7
            if (day == 0)
            {
                day = 7;
            }

            return date.Date.AddDays(-(day - 1));
        }
    }
}