using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingApplication
{
    class BindDataProvider
    {
        public static int autoChangeInterval = 1;
        private static bool autoChanged = true;

        public static bool AutoChanged
        {
            get { return BindDataProvider.autoChanged; }
            set { BindDataProvider.autoChanged = value; }
        }

    }
}
