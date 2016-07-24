using System;
using System.Collections.Generic;
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
            public ScoreType GetSiteScoreType(string sname)
            {
                //TODO: implement for sites
                throw new NotImplementedException();
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
            
        }
    }
}
