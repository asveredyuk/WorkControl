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

        private void CalcWorkedSeconds()
        {
            int workedDoubledCounter = 0;
            const int WORK_PRICE = 2;
            const int NEUTRAL_PRICE = 1;
            const int NONWORK_PRICE = 0;
            const int UNKNOWN_PRICE = 0;
            const int FINAL_DIVIDER = 2;
            foreach (var logItem in log)
            {
                if (!logItem.IsActive())
                {
                    workedDoubledCounter += NONWORK_PRICE;
                    continue;
                }
                if (logItem.IsBrowserProcess)
                {
                    //this is browser
                    var sitetype = logItem.GetSiteScoreType();
                    switch (sitetype)
                    {
                        case Settings.Lists.ScoreType.Nonwork:
                            workedDoubledCounter += NONWORK_PRICE;
                            break;
                        case Settings.Lists.ScoreType.Neutral:
                            workedDoubledCounter += NEUTRAL_PRICE;
                            break;
                        case Settings.Lists.ScoreType.Work:
                            workedDoubledCounter += WORK_PRICE;
                            break;
                        case Settings.Lists.ScoreType.Unknown:
                            workedDoubledCounter += UNKNOWN_PRICE;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    continue;
                }
                var proctype = logItem.GetProcesScoreType();
                switch (proctype)
                {
                    case Settings.Lists.ScoreType.Nonwork:
                        workedDoubledCounter += NONWORK_PRICE;
                        break;
                    case Settings.Lists.ScoreType.Neutral:
                        workedDoubledCounter += NEUTRAL_PRICE;
                        break;
                    case Settings.Lists.ScoreType.Work:
                        workedDoubledCounter += WORK_PRICE;
                        break;
                    case Settings.Lists.ScoreType.Unknown:
                        workedDoubledCounter += UNKNOWN_PRICE;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            WorkedSeconds = workedDoubledCounter/FINAL_DIVIDER;
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
