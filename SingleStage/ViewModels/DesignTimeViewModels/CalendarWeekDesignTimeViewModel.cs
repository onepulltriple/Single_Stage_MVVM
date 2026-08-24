using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleStage.Entities;

namespace SingleStage.ViewModels.DesignTimeViewModels
{
    public class CalendarWeekDesignTimeViewModel : CalendarWeekViewModel
    {
        public CalendarWeekDesignTimeViewModel() : base(
        new DateTime(2026, 8, 3),
        CreateDesignShows())
        { }

        private static IEnumerable<Show> CreateDesignShows()
        {
            return new List<Show>
            {
                new Show
                {
                    Name = "Monday Rehearsal",
                    StartTime = new DateTime(2026, 8, 3, 10, 0, 0),
                    EndTime = new DateTime(2026, 8, 3, 16, 0, 0),
                    TicketPrice = 0,
                    SoldOut = false
                },

                new Show
                {
                    Name = "Open Mic Night",
                    StartTime = new DateTime(2026, 8, 4, 19, 0, 0),
                    EndTime = new DateTime(2026, 8, 4, 21, 30, 0),
                    TicketPrice = 8.50m,
                    SoldOut = false
                },

                new Show
                {
                    Name = "Comedy Night",
                    StartTime = new DateTime(2026, 8, 5, 19, 30, 0),
                    EndTime = new DateTime(2026, 8, 5, 22, 0, 0),
                    TicketPrice = 15.00m,
                    SoldOut = false
                },

                new Show
                {
                    Name = "Rock Concert",
                    StartTime = new DateTime(2026, 8, 7, 20, 0, 0),
                    EndTime = new DateTime(2026, 8, 7, 23, 0, 0),
                    TicketPrice = 25.00m,
                    SoldOut = true
                },

                new Show
                {
                    Name = "Sunday Jazz",
                    StartTime = new DateTime(2026, 8, 9, 18, 0, 0),
                    EndTime = new DateTime(2026, 8, 9, 20, 0, 0),
                    TicketPrice = 12.00m,
                    SoldOut = false
                }
            };
        }
    }
}
