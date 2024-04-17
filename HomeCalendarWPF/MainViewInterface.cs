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
        public void ShowUpcomingEvents();
        public void CloseApplication();
        public void ShowCalendarFileNameLocationForm();
        public void ShowNoUpcomingEvents(string message);
    }
}
