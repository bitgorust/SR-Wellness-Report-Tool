using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SR_Wellness_Report
{
    class MailMessager
    {
        List<ReportRecord> records;
        string teamEmailAddr;
        bool important = false;
        StringBuilder mailContent;

        public MailMessager(List<ReportRecord> records, string emailAddr, bool important)
        {
            this.records = records;
            this.teamEmailAddr = emailAddr;
            this.important = important;
        }

        private void buildReportTitle(int category, string alias)
        {
            this.mailContent = new StringBuilder();
            this.mailContent.AppendLine("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\" xmlns=\"http://www.w3.org/TR/REC-html40\">");
            this.mailContent.AppendLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            this.mailContent.Append("<style>");
            this.mailContent.Append("body{font-family:Verdana,Arial;font-size:.813em;color:#000;font-style:normal;padding-right:0;padding-left:0;word-wrap:break-word}");
            this.mailContent.Append("table{border-collapse:collapse;border-spacing:0;border-left:1px solid #888;border-top:1px solid #888;}");
            this.mailContent.Append("th,td{border-right:1px solid #888;border-bottom:1px solid #888;text-align:center;}");
            this.mailContent.Append("th{font-weight:bold;background:#aabbff;}");
            this.mailContent.Append("</style>");
            this.mailContent.AppendLine("</head><body>");
            this.mailContent.AppendLine("Hi,");
            this.mailContent.AppendLine(alias);
            this.mailContent.AppendLine("<br/><br/>");
            switch (category)
            {
                case 0:
                    this.mailContent.AppendLine("Below is your today’s idle case list. Please update as soon as possible. Thanks:)");
                    break;
                case 1:
                    this.mailContent.AppendLine("Below is your today’s high labor case list. Please pay more attention to the cases that reached trigger2.If there is still some technical issue, please escalate it or find some walk around as soon as possible.Thanks:)");
                    break;
                default:
                    break;
            }
            this.mailContent.AppendLine("<br/><br/>");
        }

        private void buildTableHead()
        {
            this.mailContent.Append("<table>");
            this.mailContent.Append("<tr>");
            this.mailContent.Append("<th>SR Number</th>");
            this.mailContent.Append("<th>SR Title</th>");
            this.mailContent.Append("<th>Primary Contact</th>");
            this.mailContent.Append("<th>Country</th>");
            this.mailContent.Append("<th>Age (DD:HH)</th>");
            this.mailContent.Append("<th>Total Labor (Mins)</th>");
            this.mailContent.Append("<th>Idle (DD:HH)</th>");
            this.mailContent.Append("<th>Latest Communication</th>");
            this.mailContent.Append("<th>Last Labor Log</th>");
            this.mailContent.Append("<th>Is CritSit</th>");
            this.mailContent.Append("<th>Service Level</th>");
            this.mailContent.Append("<th>SR Owner</th>");
            this.mailContent.Append("</tr>");
        }

        public bool sendEmails(string minIdleTime, ref HashSet<string> aliases, ref TextBox textBoxLog)
        {
            bool allSent = true;
            foreach (string alias in aliases)
            {
                Thread.Sleep(1000);
                if (records[0].isEmpty() && sendEmail(alias) || sendEmail(alias, minIdleTime))
                {
                    textBoxLog.AppendText("Successfully send to " + alias + ".\n");
                }
                else
                {
                    allSent = false;
                    textBoxLog.AppendText("Failed in sending to " + alias + ".\n");
                }
            }
            Thread.Sleep(1000);
            if (records[0].isEmpty() && sendEmail("") || sendEmail("", minIdleTime))
            {
                textBoxLog.AppendText("Successfully send to " + this.teamEmailAddr + ".\n");
            }
            else
            {
                allSent = false;
                textBoxLog.AppendText("Failed in sending to " + this.teamEmailAddr + ".\n");
            }
            return allSent;
        }

        public bool sendEmail(string alias, string idleTime)
        {
            string content = generateEmail(alias, idleTime);
            if (!content.Equals(string.Empty))
            {
                if (alias != null && alias != "")
                {
                    if (FormReport.debugMode)
                    {
                        alias = "t-jizeng";
                    }
                    return sendMessage("", alias + "@microsoft.com", "[Action Required] Idle case update" + "【" + DateTime.Now.ToLongDateString() + "】", content);
                }
                if (FormReport.debugMode)
                {
                    teamEmailAddr = "t-jizeng@microsoft.com";
                }
                return sendMessage("", teamEmailAddr, "[Action Required] Idle case update" + "【" + DateTime.Now.ToLongDateString() + "】", content);
            }
            return true;
        }

        public bool sendEmail(string alias)
        {
            string content = null;
            if (alias != null && alias != "")
            {
                content = generate_Email_HighLabor(alias);
                if (!content.Equals(string.Empty))
                {
                    if (FormReport.debugMode)
                    {
                        alias = "t-jizeng";
                    }
                    return sendMessage("", alias + "@microsoft.com", "[Action Required] High Labor Case Update" + "【" + DateTime.Now.ToLongDateString() + "】", content);
                }
                return true;
            }
            content = generate_Email_HighLabor();
            if (!content.Equals(string.Empty))
            {
                if (FormReport.debugMode)
                {
                    teamEmailAddr = "t-jizeng@microsoft.com";
                }
                return sendMessage("", teamEmailAddr, "[Action Required] High Labor Case Update" + "【" + DateTime.Now.ToLongDateString() + "】", content);
            }
            return true;
        }

        private bool sendMessage(string from, string to, string subject, string content)
        {
            try
            {
                Microsoft.Office.Interop.Outlook.Application outlook =
                    new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem mailItem =
                    (Microsoft.Office.Interop.Outlook.MailItem)
                    outlook.CreateItem(Microsoft.Office.Interop.Outlook
                    .OlItemType.olMailItem);
                mailItem.Subject = subject;
                mailItem.To = to;
                mailItem.HTMLBody = content;
                mailItem.Importance = important ? 
                    Microsoft.Office.Interop.Outlook.OlImportance.olImportanceHigh 
                    : Microsoft.Office.Interop.Outlook.OlImportance.olImportanceNormal;
                mailItem.BodyFormat = Microsoft.Office.Interop.Outlook
                    .OlBodyFormat.olFormatHTML;
                mailItem.Display(false);
                mailItem.Send();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string generateEmail(string alias, string idleTime)
        {
            if (records.Count == 0)
            {
                return string.Empty;
            }

            if (alias != null && alias != "")
                buildReportTitle(0, alias);
            else
                buildReportTitle(0, "team");

            buildTableHead();

            bool hasContent = false;
            if (alias != null && alias != "")
            {
                foreach (ReportRecord record in records)
                    if (alias.Equals(record.SROwner))
                    {
                        this.mailContent.AppendLine(record.toHTML(idleTime));
                        hasContent = true;
                    }
            }
            else
            {
                foreach (ReportRecord record in records)
                    if (RecordFilter.Compare(record.IdleMetric, idleTime, RecordFilter.ByIdleMetric) >= 0)
                    {
                        this.mailContent.AppendLine(record.toHTML(idleTime));
                        hasContent = true;
                    }
            }

            this.mailContent.Append("</table></body></html>");

            if (hasContent)
            {
                return this.mailContent.ToString();
            }
            return string.Empty;
        }

        public string generate_Email_HighLabor()
        {
            if (records.Count == 2)
            {
                return string.Empty;
            }

            buildReportTitle(1,"team");

            int i = 0;
            int trigger2 = records[i++].trigger;
            if (!records[i].isEmpty())
            {
                this.mailContent.AppendLine("<font size=\"2\" color=\"000055\"><b>The cases that have reached <b style=\"background:#ffff00\">trigger2(" + trigger2 + ")</b>:</b></font>");
                this.mailContent.AppendLine("<br/>");
                this.mailContent.AppendLine("<font size=\"2\" color=\"000055\"><b>Total Labor (Mins)[Premier cases(excluding advisory): <b style=\"background:#ffff00\">" + trigger2 + "+</b>]</b></font>");
                this.mailContent.AppendLine("<br/><br/>");

                buildTableHead();
                for (; i < records.Count; i++)
                {
                    if (records[i].isEmpty())
                    {
                        this.mailContent.Append("</table>");
                        break;
                    }
                    this.mailContent.Append(records[i].toHTML());
                }
                this.mailContent.AppendLine("<br/><br/>");
            }

            int trigger1 = records[i++].trigger;
            if (i < records.Count)
            {
                this.mailContent.AppendLine("<font size=\"2\" color=\"000055\"><b>The cases that have reached <b style=\"background:#ffff00\">trigger1(" + trigger1 + ")</b>:</b></font>");
                this.mailContent.AppendLine("<br/>");
                this.mailContent.AppendLine("<font size=\"2\" color=\"000055\"><b>Total Labor (Mins)[Premier Cases(excluding advisory): <b style=\"background:#ffff00\">" + trigger1 + "~" + trigger2 + "</b>]</b></font>");
                this.mailContent.AppendLine("<br/><br/>");

                buildTableHead();
                for (; i < records.Count; i++)
                {
                    this.mailContent.Append(records[i].toHTML());
                }
                this.mailContent.Append("</table>");
            }

            this.mailContent.Append("</body></html>");
            return this.mailContent.ToString();
        }

        public string generate_Email_HighLabor(string alias)
        {
            bool hasContent = false;

            buildReportTitle(1, alias);

            int i = 0;
            int trigger2 = records[i++].trigger;
            bool isAddTableHead = false;
            for (; i < records.Count; i++)
            {
                if (records[i].isEmpty())
                {
                    break;
                }
                if (alias.Equals(records[i].SROwner))
                {
                    if (!isAddTableHead)
                    {
                        this.mailContent.AppendLine("<font size=\"2\" color=\"000033\"><b>The cases that have reached <b style=\"background:#ffff00\">trigger2(" + trigger2 + ")</b>:</b></font>");
                        this.mailContent.AppendLine("<br/>");
                        this.mailContent.AppendLine("<font size=\"2\" color=\"000033\"><b>Total Labor (Mins)[Premier cases(excluding advisory): <b style=\"background:#ffff00\">" + trigger2 + "+</b>]</b></font>");
                        this.mailContent.AppendLine("<br/><br/>");
                        buildTableHead();
                        isAddTableHead = true;
                        hasContent = true;
                    }
                    this.mailContent.Append(records[i].toHTML());
                }
            }
            if (isAddTableHead)
            {
                this.mailContent.Append("</table>");
                this.mailContent.AppendLine("<br/><br/>");
            }

            int trigger1 = records[i++].trigger;
            isAddTableHead = false;
            for (; i < records.Count; i++)
            {
                if (alias.Equals(records[i].SROwner))
                {
                    if (!isAddTableHead)
                    {
                        this.mailContent.AppendLine("<font size=\"2\" color=\"000033\"><b>The cases that have reached <b style=\"background:#ffff00\">trigger1(" + trigger1 + ")</b>:</b></font>");
                        this.mailContent.AppendLine("<br/>");
                        this.mailContent.AppendLine("<font size=\"2\" color=\"000033\"><b>Total Labor (Mins)[Premier Cases(excluding advisory): <b style=\"background:#ffff00\">" + trigger1 + "~" + trigger2 + "</b>]</b></font>");
                        this.mailContent.AppendLine("<br/><br/>");
                        buildTableHead();
                        isAddTableHead = true;
                        hasContent = true;
                    }
                    this.mailContent.Append(records[i].toHTML());
                }
            }
            if (isAddTableHead)
            {
                this.mailContent.Append("</table>");
                this.mailContent.Append("</body></html>");
            }

            if (hasContent)
            {
                return this.mailContent.ToString();
            }
            return string.Empty;
        }
    }
}
