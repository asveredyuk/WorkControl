using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl
{
    class Settings
    {

        private static Settings _self;
        public static Settings Self
        {
            get
            {
                if(_self==null)
                    _self = new Settings();
                return _self;
            }
        }

        private Settings()
        {
            //load settings here
            ScoreLists = Lists.Load();
        }

        public Lists ScoreLists { get; }


        public class Lists
        {
            const string FNAME = "lists.txt";

            public enum ScoreType
            {
                Nonwork, Neutral, Work, Unknown
            }

            public Dictionary<string, ScoreType> processTypes;
            public Dictionary<string, ScoreType> siteTypes;

            private Lists()
            {
                processTypes = new Dictionary<string, ScoreType>();
                siteTypes = new Dictionary<string, ScoreType>();
            }

            private Lists(Dictionary<string, ScoreType> processTypes, Dictionary<string, ScoreType> siteTypes)
            {
                this.processTypes = processTypes;
                this.siteTypes = siteTypes;
            }

            public ScoreType GetProceScoreType(string pname)
            {
                string pnamelower = pname.ToLower();
                if(!processTypes.ContainsKey(pnamelower))
                    return ScoreType.Unknown;
                return processTypes[pnamelower];
            }
            public ScoreType GetSiteScoreType(string hname)
            {
                string host = GetKnownHostName(hname);
                if (host != null)
                    return siteTypes[host];
                return ScoreType.Unknown;
            }
            /// <summary>
            /// Get known domain
            /// </summary>
            /// <param name="host">host name like www.example.com</param>
            /// <returns></returns>
            public string GetKnownHostName(string host)
            {
                if (host == null)
                    return null;
                host = host.ToLower();
                //domain is going up until it is 1st level
                //ex. : we have www.travel.domain.com
                //1 - try to find www.travel.domain.com, if not found
                //2 - try to find travel.domain.com, if not found
                //3 - try to find domain.com, if not found
                //this domain is unknown
                while (host.Contains("."))
                {
                    if (siteTypes.ContainsKey(host))
                    {
                        return host;
                    }
                    //go to the upper level
                    int pos = host.IndexOf(".");
                    host = host.Substring(pos + 1);
                }
                return null;
            }
            public void Save()
            {
                StreamWriter sw = new StreamWriter(FNAME);
                sw.WriteLine("--Processes");
                foreach (var pair in processTypes)
                {
                    sw.WriteLine($"{pair.Key}:{pair.Value}");
                }
                sw.WriteLine("--Sites");
                foreach (var pair in siteTypes)
                {
                    sw.WriteLine($"{pair.Key}:{pair.Value}");
                }
                sw.Close();
            }
            public static Lists Load()
            {
                if(!File.Exists(FNAME))
                    return new Lists();
                string[] lines = File.ReadAllLines(FNAME);
                var  processTypes = new Dictionary<string, ScoreType>();
                var  siteTypes = new Dictionary<string, ScoreType>();
                //addself
                processTypes[Process.GetCurrentProcess().ProcessName.ToLower()]= ScoreType.Neutral;
                //add from settings
                Dictionary<string, ScoreType> now = null;
                foreach (var line in lines)
                {
                    if (line.StartsWith("--"))
                    {
                        //this is header
                        switch (line.Replace("--",""))
                        {
                            case "processes":
                                now = processTypes;
                                break;
                            case "sites":
                                now = siteTypes;
                                break;
                        }
                        continue;
                    }
                    if (now == null)
                    {
                        throw new FileLoadException("no heading for items found!");
                    }
                    string[] arr = line.Split(':');
                    now[arr[0].ToLower()] = (ScoreType) int.Parse(arr[1]);
                }
                return new Lists(processTypes, siteTypes);
            }
            /// <summary>
            /// Check if given process is browser
            /// </summary>
            /// <param name="procname"></param>
            /// <returns></returns>
            public bool IsBrowserProcess(string procname)
            {
                return procname == "chrome";
            }
            
        }
    }
}
