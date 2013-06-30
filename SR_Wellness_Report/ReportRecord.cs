using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SR_Wellness_Report
{
    public class ReportRecord
    {
        public string SRNumber;
        public string SRTitle;
        public string AGE;
        public string TotalSRLabor;
        public string IdleMetric;
        public string LatestCommunication;
        public string LastLaborLog;
        public string SROwner;
        public string Contact;
        public string Country;
        public string Area;
        public string IsCritSit;
        public string ServiceLevel;
        public string SRType;

        public int trigger;
        private bool empty;

        public ReportRecord(int t)
        {
            trigger = t;
            empty = true;
        }

        public ReportRecord(XmlElement detailElement)
        {
            this.IdleMetric = detailElement.GetAttribute("IdleMetric");
            this.SRNumber = detailElement.GetAttribute("SRTitle1");
            this.SRTitle = detailElement.GetAttribute("SRTitle");
            this.AGE = detailElement.GetAttribute("AGE");
            this.TotalSRLabor = detailElement.GetAttribute("TotalSRLabor");
            this.LatestCommunication = detailElement.GetAttribute("LatestCommunication");
            this.LastLaborLog = detailElement.GetAttribute("LastLaborLog");
            this.SROwner = detailElement.GetAttribute("SROwner");
            this.Contact = detailElement.GetAttribute("PrimaryContact");
            this.Country = detailElement.GetAttribute("CustomerCountry");
            this.IsCritSit = detailElement.GetAttribute("IsCritSit");
            this.ServiceLevel = detailElement.GetAttribute("ServiceLevel");
            this.Area = detailElement.GetAttribute("CustomerArea");
            this.SRType = detailElement.GetAttribute("SRType");
            this.empty = false;
        }

        public ReportRecord(string SRNumber, string SRTitle,
            string AGE, string TotalSRLabor, string IdleMetric,
            string LatestCommunication, string LastLaborLog,
            string SROwner)
        {
            this.AGE = AGE;
            this.IdleMetric = IdleMetric;
            this.LastLaborLog = LastLaborLog;
            this.LatestCommunication = LatestCommunication;
            this.SRNumber = SRNumber;
            this.SROwner = SROwner;
            this.SRTitle = SRTitle;
            this.TotalSRLabor = TotalSRLabor;
            this.empty = false;
        }

        public ReportRecord(string SRNumber, string SRTitle,
            string AGE, string TotalSRLabor, string IdleMetric,
            string LatestCommunication, string LastLaborLog,
            string SROwner, string Contact, string Country, string IsCritSit, string ServiceLevel)
            : this(SRNumber, SRTitle, AGE, TotalSRLabor, IdleMetric, LatestCommunication, LastLaborLog, SROwner)
        {
            this.Contact = Contact;
            this.Country = Country;
            this.IsCritSit = IsCritSit;
            this.ServiceLevel = ServiceLevel;
        }

        public ReportRecord(string SRNumber, string SRTitle,
            string AGE, string TotalSRLabor, string IdleMetric,
            string LatestCommunication, string LastLaborLog,
            string SROwner, string Contact, string Country, string Area, string IsCritSit, string ServiceLevel, string SRType)
            : this(SRNumber, SRTitle, AGE, TotalSRLabor, IdleMetric, LatestCommunication, LastLaborLog, SROwner, Contact, Country, IsCritSit, ServiceLevel)
        {
            this.Area = Area;
            this.SRType = SRType;
        }

        public int ByIdleMetric(string idleTime)
        {
            try
            {
                string[] strs1 = this.IdleMetric.Split(':');
                string[] strs2 = idleTime.Split(':');
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

        public int compare_TotalSRLabor(string labor)
        {
            int labor1 = int.Parse(TotalSRLabor);
            int labor2 = int.Parse(labor);
            return labor1 - labor2;
        }

        public string tdEntry(string color)
        {
            DateTime LastLaborLogTime;
            DateTime.TryParse(LastLaborLog, out LastLaborLogTime);

            DateTime LatestCommunicationTime;
            DateTime.TryParse(LastLaborLog, out LatestCommunicationTime);

            return "<tr bgcolor=\"" + color
                + "\"><td><a tabindex=\"33\" href=\"mssv://sr/?" + this.SRNumber
                + "\" style=\"color:Blue\" TARGET=\"_top\">" + this.SRNumber
                +"</a></td><td>" + this.SRTitle
                + "</td><td>" + this.Contact
                + "</td><td>" + this.Country
                + "</td><td>" + this.AGE
                + "</td><td>" + this.TotalSRLabor
                + "</td><td>" + this.IdleMetric
                + "</td><td>" + LatestCommunicationTime
                + "</td><td>" + LastLaborLogTime.ToLocalTime()
                + "</td><td>" + this.IsCritSit
                + "</td><td>" + this.ServiceLevel
                + "</td><td>" + this.SROwner
                + "</td></tr>";
        }

        public string toHTML(string idleTime) 
        {
            string color = "#bbccff";
            int delta = ByIdleMetric(idleTime);
            if (delta >= 0)
                if (delta < 168)
                    color = "#eeee00";
                else if (delta < 336)
                    color = "#ff8800";
                else color = "#ff0000";

            return tdEntry(color);
        }

        public string toHTML()
        {
            string color = "#eeee00";
            int delta = 0;
            if ((delta = compare_TotalSRLabor("400")) >= 0)
                if (delta < 200)
                    color = "eeee00";
                else if (delta < 400)
                    color = "#ffcc33";
                else if (delta < 600)
                    color = "#ff8800";
                else color = "#ff0000";

            return tdEntry(color);
        }

        public bool isEmpty()
        {
            return this.empty;
        }
    }
}
