using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl
{
    interface ILog
    {
        /// <summary>
        /// Add range of items to log
        /// </summary>
        /// <param name="items"></param>
        void AddRangeOfItems(List<LogItem> items);
        /// <summary>
        /// Add item to collection
        /// </summary>
        /// <param name="item"></param>
        void PutItem(LogItem item);

        /// <summary>
        /// Count the number of secondss
        /// </summary>
        /// <returns></returns>
        int GetWorkedSeconds();
        /// <summary>
        /// Get all the handled seconds
        /// </summary>
        /// <returns></returns>
        int GetTotalSeconds();
    }
}
