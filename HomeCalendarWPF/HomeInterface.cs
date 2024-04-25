using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface HomeInterface
    {
        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents);
        public void ShowEventsByCategory();
        public void ShowEventsByMonth();
        public void ShowEventsByMonthAndCategory();
        public void ShowNoUpcomingEvents(string message);
    }
}
