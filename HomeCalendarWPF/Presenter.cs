using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calendar;


namespace HomeCalendarWPF
{
    internal class Presenter
    {
        //private readonly ViewInterface view;

        private readonly IWelcomeViewInterface welcomeView;
        private readonly ICreateEventViewInterface createEventView;
        private readonly CategoryView categoryView;
        private readonly HomeCalendar model;
        public Presenter(IWelcomeViewInterface wv)
        {

            model = new HomeCalendar("example",true);
            view = v;

        }

    }
}
