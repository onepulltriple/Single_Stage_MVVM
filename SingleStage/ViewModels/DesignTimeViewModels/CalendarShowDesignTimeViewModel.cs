using System;
using SingleStage.Entities;

namespace SingleStage.ViewModels.DesignTimeViewModels
{
    public class CalendarShowDesignTimeViewModel : CalendarShowViewModel
    {
        public CalendarShowDesignTimeViewModel()
            : base(new Entities.Show
            {
                Name = "Comedy Night",
                StartTime = new DateTime(2026, 8, 5, 19, 30, 0),
                EndTime = new DateTime(2026, 8, 5, 22, 0, 0),
                TicketPrice = 15.00m,
                SoldOut = false
            })
            { }
    }
}
