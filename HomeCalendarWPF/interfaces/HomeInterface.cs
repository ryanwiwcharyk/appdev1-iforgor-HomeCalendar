using Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeCalendarWPF.interfaces
{
    public interface HomeInterface
    {
        public void ShowUpcomingEvents(List<CalendarItem> upcomingEvents);
        public void ShowUpcomingEventsByCategory(List<CalendarItemsByCategory> upcomingEventsByCategory);
        public void ShowUpcomingEventsByMonth(List<CalendarItemsByMonth> upcomingEventsByMonth);
        public void ShowUpcomingEventsByMonthAndCategory(List<Dictionary<string, object>> items, List<Category> categories);
        public void ShowNoUpcomingEvents();
        public void AddCategoriesToDropdown(List<Category> categories);
    }
}
