using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace WorkControl.Reports
{

    class WorkReport
    {
        private readonly List<LogItem> log;

        private WorkReport(List<LogItem> log)
        {
            this.log = log;


        }

        public static WorkReport GenerateReport(List<LogItem> items)
        {
            return new WorkReport(items);
        }


        
    }
}
