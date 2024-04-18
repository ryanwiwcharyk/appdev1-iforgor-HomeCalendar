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
        void ShowWarning(string warning);
        void ShowSuccess(string success);
        void RefreshPage();
        void FillDropDown(List<Category.CategoryType> types);




    }
}
