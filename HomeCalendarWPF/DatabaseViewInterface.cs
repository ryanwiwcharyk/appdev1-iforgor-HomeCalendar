using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeCalendarWPF
{
    public interface DatabaseViewInterface
    {
        //This method will dinstinguish between existing and new by the name param check validation in main 
        public void ConnectToDb(string location, string name);

    }
}
