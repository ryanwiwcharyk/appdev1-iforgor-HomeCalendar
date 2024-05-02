using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF.interfaces
{
    public interface UpdateEventViewInterface
    {
        public void ShowPopulatedFields(string details, double duration, DateTime startDate, int hours, int minutes, Category category);
        public void AddCategoriesToDropdown(List<Category> cats);
        public void ShowErrorPopup(string message);
        public void ShowSuccessPopup(string message);
    }
}
