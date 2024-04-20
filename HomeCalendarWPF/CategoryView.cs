using Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface CategoryView
    {
        public void ShowWarning(string warning);

        public void FillDropDown(List<Category.CategoryType> types);

        public void ShowSuccessPopup(string message);




    }
}
