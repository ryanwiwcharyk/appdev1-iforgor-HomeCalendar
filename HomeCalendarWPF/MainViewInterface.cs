using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface MainViewInterface
    {
        public void ShowRecentFiles();
        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents);
        public void CloseApplication();
        public void ShowNoUpcomingEvents(string message);
    }
}
