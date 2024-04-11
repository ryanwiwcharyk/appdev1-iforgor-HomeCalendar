using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface MainViewInterface
    {
        void ShowRecentFiles();
        void ShowUpcomingEvents();
        public void ShowAddCategoryTab();
        public void ShowAddEventTab();
        public void CloseApplication();
        public void ShowCalendarFileNameLocationForm();
    }
}
