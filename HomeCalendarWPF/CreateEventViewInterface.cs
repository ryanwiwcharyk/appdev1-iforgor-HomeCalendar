using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface ICreateEventViewInterface
    {
        public string GetEventDetails();
        public DateTime GetEventStartTime();
        public double GetEventDurationInMinutes();
        public int GetEventCategory();
        public void AddCategoryToMenu(string categoryName);
        public void ShowSuccessPopup(string message);
        public void ShowErrorPopup(string message);
    }
}
