using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF.interfaces
{
    public interface ICreateEventViewInterface
    {
        public void AddCategoriesToDropdown(List<Category> categories);
        public void ShowSuccessPopup(string message);
        public void ShowErrorPopup(string message);
    }
}
