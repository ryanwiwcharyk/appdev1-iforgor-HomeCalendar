using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface IWelcomeViewInterface
    {
        public string GetCalendarName();
        public string GetExistingCalendarFileLocation();
        public string GetNewClendarFileLocation();
    }
}
