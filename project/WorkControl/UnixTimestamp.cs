using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl
{
    static class UnixTimestamp
    {
        public static int GetFromDatatime(DateTime dt)
        {
            return(int)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime ConvertToDatetime(int time)
        {
            return new DateTime(1970, 1, 1).AddSeconds(time);
        }

        public static DateTime ConvertIntervalToDateTime(int interval)
        {
            return new DateTime().AddSeconds(interval);
        }
    }
}
