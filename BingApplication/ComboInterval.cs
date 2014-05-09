using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingApplication
{
    class ComboInterval
    {
        private int CARDINAL_NUMBER = 1000 * 60;  //1分钟为单位
        private int interval;


        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

    }
}
