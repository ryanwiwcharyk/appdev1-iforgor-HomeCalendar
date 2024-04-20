using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface DatabaseViewInterface
    {
        public void NewDatabase(string location, string name);

        public void ExistingDatabase(string location);
    }
}
