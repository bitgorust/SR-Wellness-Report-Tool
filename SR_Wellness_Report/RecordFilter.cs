using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace SR_Wellness_Report
{
    public delegate int CompareDelegate(string var1, string var2);

    class RecordFilter
    {
        public static int Compare(string var1, string var2, CompareDelegate compareBy)
        {
            return compareBy(var1, var2);
        }

        // Compare by IdleMetric
        public static int ByIdleMetric(string var1, string var2)
        {
            try
            {
                string[] strs1 = var1.Split(':');
                string[] strs2 = var2.Split(':');
                int dd1 = int.Parse(strs1[0]);
                int hh1 = int.Parse(strs1[1]);
                int dd2 = int.Parse(strs2[0]);
                int hh2 = int.Parse(strs2[1]);
                return dd1* 24 + hh1 - dd2 * 24 - hh2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Compare by TotalSRLabor
        public static int ByTotalSRLabor(string var1, string var2)
        {
            try
            {
                return int.Parse(var1) - int.Parse(var2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // Filter by IdleMetric
        public static List<ReportRecord> ByIdleMetric(ref XmlNodeList xmlNodes, ref HashSet<string> aliases, string metric)
        {
            List<ReportRecord> records = new List<ReportRecord>();

            aliases.Clear();

            foreach (XmlElement detail in xmlNodes)
            {
                if (!detail.Name.Equals(FormReport.ELEMENT_TAG))
                    continue;

                ReportRecord record = new ReportRecord(detail);
                string minIdleTime = FormReport.IdleTimeConf.Keys.Contains(record.SROwner) ? FormReport.IdleTimeConf[record.SROwner] : metric;
                if (Compare(record.IdleMetric, minIdleTime, ByIdleMetric) >= 0)
                {
                    records.Add(record);
                    aliases.Add(record.SROwner);
                }
            }

            if (records.Count != 0)
            {
                sortByOwner(ref records, FormReport.nameAsc, 0);
            }

            return records;
        }

        // Filter by TotalSRLabor
        public static List<ReportRecord> ByTotalSRLabor(ref XmlNodeList xmlNodes, ref HashSet<string> aliases, string metric)
        {
            List<ReportRecord> records = new List<ReportRecord>();
            List<ReportRecord> r1 = new List<ReportRecord>();
            List<ReportRecord> r2 = new List<ReportRecord>();

            aliases.Clear();

            string[] triggers = metric.Split(':');

            foreach (XmlElement detail in xmlNodes)
            {
                if (!detail.Name.Equals(FormReport.ELEMENT_TAG))
                    continue;

                ReportRecord record = new ReportRecord(detail);
                if (record.ServiceLevel == "Premier" && record.SRType != "Advisory")
                {
                    if (Compare(record.TotalSRLabor, triggers[1], ByTotalSRLabor) >= 0)
                    {
                        r2.Add(record);
                    }
                    else if (Compare(record.TotalSRLabor, triggers[0], ByTotalSRLabor) >= 0)
                    {
                        r1.Add(record);
                    }
                    aliases.Add(record.SROwner);
                }
            }

            if (r2.Count != 0)
            {
                sortByOwner(ref r2, FormReport.nameAsc, 1);
            }

            if (r1.Count != 0)
            {
                sortByOwner(ref r1, FormReport.nameAsc, 1);
            }

            records.Add(new ReportRecord(int.Parse(triggers[1])));
            records.AddRange(r2);
            records.Add(new ReportRecord(int.Parse(triggers[0])));
            records.AddRange(r1);

            return records;
        }

        private static void sortByOwner(ref List<ReportRecord> records, bool asc, int category)
        {
            IComparer<ReportRecord> com1 = new RecordOwnerComparer(FormReport.nameAsc);
            IComparer<ReportRecord> com2 = null;
            switch (category)
            {
                case 0:
                    com2 = new RecordIdleComparer();
                    break;
                case 1:
                    com2 = new RecordLaborComparer();
                    break;
                default:
                    break;
            }
            
            records.Sort(com2);
            records.Sort(com1);
            ReportRecord temp = null;
            int i = 0, j = 0;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (ReportRecord r in records)
            {
                if (temp == null)
                    temp = r;
                else if (!r.SROwner.Equals(temp.SROwner))
                {
                    dic.Add(i, j - i);
                    temp = r;
                    i = j;
                }
                j++;
            }
            dic.Add(i, j - i);
            foreach (int x in dic.Keys)
            {
                int y = x;
                dic.TryGetValue(x, out y);
                records.Sort(x, y, com2);
            }
        }

        class RecordOwnerComparer : IComparer<ReportRecord>
        {
            bool asc = true;
            public RecordOwnerComparer(bool asc)
            {
                this.asc = asc;
            }

            int IComparer<ReportRecord>.Compare(ReportRecord x, ReportRecord y)
            {
                int res = ((new CaseInsensitiveComparer()).Compare(x.SROwner, y.SROwner));
                if (asc)
                    return res;
                else return -res;
            }
        }

        class RecordIdleComparer : IComparer<ReportRecord>
        {
            int IComparer<ReportRecord>.Compare(ReportRecord x, ReportRecord y)
            {
                return -Compare(x.IdleMetric, y.IdleMetric, ByIdleMetric);
            }
        }

        class RecordLaborComparer : IComparer<ReportRecord>
        {
            int IComparer<ReportRecord>.Compare(ReportRecord x, ReportRecord y)
            {
                return -Compare(x.TotalSRLabor, y.TotalSRLabor, ByTotalSRLabor);
            }
        }

    }
}
