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
        private readonly IReadOnlyList<LogItem> log;

        private WorkReport(List<LogItem> log)
        {
            this.log = log;
            //calc some data
            CalcWorkedSeconds();
        }
                    const int WORK_PRICE = 2;
            const int NEUTRAL_PRICE = 1;
            const int NONWORK_PRICE = 0;
            const int UNKNOWN_PRICE = 0;
            const int FINAL_DIVIDER = 2;
        private void CalcWorkedSeconds()
        {
            int workedDoubledCounter = 0;

            foreach (var logItem in log)
            {
                workedDoubledCounter += logItem.GetWorkedPrice();
            }
            WorkedSeconds = workedDoubledCounter/FINAL_DIVIDER;
        }

        public static int GetWorkedPrice(LogItem logItem)
        {
            if (!logItem.IsActive())
            {
                return NONWORK_PRICE;

            }
            if (logItem.IsBrowserProcess)
            {
                //this is browser
                var sitetype = logItem.GetSiteScoreType();
                switch (sitetype)
                {
                    case Settings.Lists.ScoreType.Nonwork:
                        return NONWORK_PRICE;

                    case Settings.Lists.ScoreType.Neutral:
                        return NEUTRAL_PRICE;

                    case Settings.Lists.ScoreType.Work:
                        return WORK_PRICE;

                    case Settings.Lists.ScoreType.Unknown:
                        return UNKNOWN_PRICE;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            var proctype = logItem.GetProcesScoreType();
            switch (proctype)
            {
                case Settings.Lists.ScoreType.Nonwork:
                    return NONWORK_PRICE;

                case Settings.Lists.ScoreType.Neutral:
                    return NEUTRAL_PRICE;

                case Settings.Lists.ScoreType.Work:
                    return WORK_PRICE;

                case Settings.Lists.ScoreType.Unknown:
                    return UNKNOWN_PRICE;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Seconds spent on work
        /// </summary>
        public int WorkedSeconds { get; private set; }

        /// <summary>
        /// Total handled seconds
        /// </summary>
        public int TotalSeconds => log.Count;

        public int NonWorkedSeconds => TotalSeconds - WorkedSeconds;

        public static WorkReport GenerateReport(List<LogItem> items)
        {
            return new WorkReport(items);
        }
    }
}
