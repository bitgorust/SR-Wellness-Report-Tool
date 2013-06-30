using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SR_Wellness_Report
{
    public delegate List<ReportRecord> FilterDelegate(ref XmlNodeList xmlNodes, ref HashSet<string> aliases, string metric);

    public partial class FormReport : Form
    {
        [DllImport("shell32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

        public static bool debugMode = false;
        public static bool nameAsc = true;
        public static string LIST_TAG = "Tablix2";
        public static string ELEMENT_TAG = "Detail";

        public static Dictionary<string, string> IdleTimeConf = new Dictionary<string, string>();

        const string CONF_FILE = "cusIdle.txt";

        // Items in Registry
        const string XML_PATH = "XMLPath";
        const string MAIL_ADDR = "To";
        const string IDLE_METRIC = "IdleMetric";
        const string TOTAL_LABOR = "LaborTrigger";
        const string OWNER_ASC = "OwnerAsc";
        const string IMPORTANCE = "Importance";

        // Items in comboBoxProperty
        const string MIN_IDLE_TIME = "MinIdleTime(dd:hh)";
        const string MIN_TOTAL_LABOR = "MinTotalLabor(t1:t2)";

        public HashSet<string> aliases = new HashSet<string>();
        public XmlNodeList srList = null;

        private static bool autoMode = false;
        private static bool idleSent = false;
        private static bool tmpiSent = false;

        private static string MinIdleTime = "07:00";
        private static string MinTotalLabor = "400:800";
        private static string TeamMailAddr = "shepseds@microsoft.com";

        public FormReport()
        {
            InitializeComponent();
        }

        private void loadRegistryAndConf()
        {
            if (!ReportRegistry.readValue(MAIL_ADDR).Equals(string.Empty))
                TeamMailAddr = ReportRegistry.readValue(MAIL_ADDR);
            this.textBoxMailAddr.Text = TeamMailAddr;

            if (!ReportRegistry.readValue(IDLE_METRIC).Equals(string.Empty))
                MinIdleTime = ReportRegistry.readValue(IDLE_METRIC);
            this.textBoxTime.Text = MinIdleTime;

            if (!ReportRegistry.readValue(TOTAL_LABOR).Equals(string.Empty))
                MinTotalLabor = ReportRegistry.readValue(TOTAL_LABOR);

            if (autoMode)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(FormExport.PIDLMRU_PATH + "\\OpenSavePidlMRU\\xml", false);
                if (key != null && key.GetValue("0") != null)
                {
                    this.textBoxPath.Text = getPathFromPIDL((byte[])key.GetValue("0"));
                }
            }
            else
            {
                this.textBoxPath.Text = ReportRegistry.readValue(XML_PATH);
            }

            try
            {
                StreamReader confReader = new StreamReader(CONF_FILE);
                string line;
                while ((line = confReader.ReadLine()) != null)
                {
                    string[] keyValue = line.Split('=');
                    if (keyValue.Length != 2)
                    {
                        MessageBox.Show(line + " is invalid. Must written as key=value");
                    }
                    IdleTimeConf.Add(keyValue[0].Trim(), keyValue[1].Trim());
                }
                confReader.Close();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                this.textBoxLog.AppendText("Warning: " + CONF_FILE + " does not exist.\n");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Failed to load configuration in " + CONF_FILE);
            }
        }

        private void loadCommandArgs()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].Substring("--".Length).Equals("auto", StringComparison.OrdinalIgnoreCase))
                    {
                        autoMode = true;
                    }
                    if (args[i].Substring("--".Length).Equals("debug", StringComparison.OrdinalIgnoreCase))
                    {
                        debugMode = true;
                    }
                    if (args[i].Substring("--".Length).Equals("idle", StringComparison.OrdinalIgnoreCase))
                    {
                        idleSent = true;
                    }
                    if (args[i].Substring("--".Length).Equals("tmpi", StringComparison.OrdinalIgnoreCase))
                    {
                        tmpiSent = true;
                    }
                }
                if (!idleSent && !tmpiSent)
                {
                    idleSent = tmpiSent = true;
                }
            }
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            loadRegistryAndConf();
            loadCommandArgs();

            if (autoMode)
            {
                if (idleSent)
                {
                    this.comboBoxProperty.Text = MIN_IDLE_TIME;
                    this.textBoxTime.Text = MinIdleTime;
                    btnStart.PerformClick();
                }
                if (tmpiSent)
                {
                    this.comboBoxProperty.Text = MIN_TOTAL_LABOR;
                    this.textBoxTime.Text = MinTotalLabor;
                    btnStart.PerformClick();
                }
                btnExit.PerformClick();
            }
        }

        private string getPathFromPIDL(byte[] byteCode)
        {
            StringBuilder path = new StringBuilder(256);
            IntPtr ptr = IntPtr.Zero;
            GCHandle h0 = GCHandle.Alloc(byteCode, GCHandleType.Pinned);
            try
            {
                ptr = h0.AddrOfPinnedObject();
            }
            catch
            {
                h0.Free();
            }
            SHGetPathFromIDListW(ptr, path);
            return path.ToString();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            this.openReportDialog.ShowDialog(this);
        }

        private void openReportDialog_FileOk(object sender, CancelEventArgs e)
        {
            this.textBoxPath.Text = this.openReportDialog.FileName;
        }

        private void groupBoxOrder_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxTime_Enter(object sender, EventArgs e)
        {
            this.textBoxTime.ForeColor = Color.Black;
        }

        private void textBoxTime_Validated(object sender, EventArgs e)
        {
            switch (this.comboBoxProperty.Text)
            {
                case MIN_IDLE_TIME:
                    if (!validTime(this.textBoxTime.Text))
                    {
                        this.textBoxTime.ForeColor = Color.Red;
                        MessageBox.Show("Idle Time Format is dd:hh");
                    }
                    else
                    {
                        MinIdleTime = this.textBoxTime.Text;
                    }
                    break;
                case MIN_TOTAL_LABOR:
                    if (!validTrigger(this.textBoxTime.Text))
                    {
                        this.textBoxTime.ForeColor = Color.Red;
                        MessageBox.Show("Total Labor Format is t1:t2\nMake sure that t1 < t2");
                    }
                    else
                    {
                        MinTotalLabor = this.textBoxTime.Text;
                    }
                    break;
                default:
                    break;
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            this.textBoxLog.Clear();

            if (!validSettings())
            {
                MessageBox.Show("Please make sure that all settings are valid.");
                return;
            }

            if ((this.srList = parseXmlFile(this.textBoxPath.Text)) == null)
            {
                MessageBox.Show("Please make sure\n" + this.textBoxPath.Text + "\nis a valid XML file.");
                return;
            }

            nameAsc = radioBtnAsc.Checked;
            List<ReportRecord> records = null;
            switch (this.comboBoxProperty.Text)
            {
                case MIN_IDLE_TIME:
                    records = Filter(ref this.srList, ref aliases, this.textBoxTime.Text, RecordFilter.ByIdleMetric);
                    break;
                case MIN_TOTAL_LABOR:
                    records = Filter(ref this.srList, ref aliases, this.textBoxTime.Text, RecordFilter.ByTotalSRLabor);
                    break;
                default:
                    break;
            }

            if (records != null)
            {
                MailMessager messager = new MailMessager(records, this.textBoxMailAddr.Text, this.checkBoxImportance.Checked);
                if (messager.sendEmails(this.textBoxTime.Text, ref aliases, ref this.textBoxLog))
                {
                    textBoxLog.AppendText("All mails are successfully sent!\n");
                }
                else
                {
                    textBoxLog.AppendText("Some mails failed to send!\n");
                }
            }

            saveConfiguration();
        }

        private bool validTime(string time)
        {
            if (time == null || time == "")
                return true;

            try
            {
                string[] strs = time.Split(':');
                int dd = int.Parse(strs[0]);
                int hh = int.Parse(strs[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private bool validTrigger(string time)
        {
            if (time == null || time == "")
                return true;

            try
            {
                string[] strs = time.Split(':');
                int t1 = int.Parse(strs[0]);
                int t2 = int.Parse(strs[1]);
                if (t1 >= t2)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        private bool validSettings()
        {
            return true;
        }

        private XmlNodeList parseXmlFile(string file)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(file, FileMode.Open);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.textBoxLog.AppendText("Failed to open " + file + ".\n");
                return null;
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.textBoxLog.AppendText("Failed to load " + file + ".\n");
                return null;
            }
            fs.Close();
            this.textBoxLog.AppendText(file + " is successfully loaded.\n");

            XmlElement report = doc.DocumentElement;
            if (report == null)
            {
                this.textBoxLog.AppendText("Failed to get the root of " + file + ".\n");
                return null;
            }
            XmlNodeList list = report.GetElementsByTagName(LIST_TAG);
            if (list == null || list.Count == 0)
            {
                this.textBoxLog.AppendText("Failed to get " + LIST_TAG + " elements.\n");
                return null;
            }
            XmlNodeList details = list.Item(0).ChildNodes;
            if (details == null || details.Count == 0)
            {
                this.textBoxLog.AppendText("Failed to get child nodes of " + LIST_TAG + ".\n");
                return null;
            }

            return details.Item(0).ChildNodes;
        }

        private void saveConfiguration()
        {
            ReportRegistry.setValue(XML_PATH, this.textBoxPath.Text);
            ReportRegistry.setValue(MAIL_ADDR, this.textBoxMailAddr.Text);
            switch (this.comboBoxProperty.Text)
            {
                case MIN_IDLE_TIME:
                    ReportRegistry.setValue(IDLE_METRIC, MinIdleTime);
                    break;
                case MIN_TOTAL_LABOR:
                    ReportRegistry.setValue(TOTAL_LABOR, MinTotalLabor);
                    break;
                default:
                    break;
            }
            ReportRegistry.setValue(OWNER_ASC, nameAsc ? "True" : "False");
            ReportRegistry.setValue(IMPORTANCE, this.checkBoxImportance.Checked ? "True" : "False");
        }

        public static List<ReportRecord> Filter(ref XmlNodeList xmlNodes, ref HashSet<string> aliases, string metric, FilterDelegate FilterMetric)
        {
            return FilterMetric(ref xmlNodes, ref aliases, metric);
        }

        private void comboBoxProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxTime.ForeColor = Color.Black;
            switch (this.comboBoxProperty.Text)
            {
                case MIN_IDLE_TIME:
                    this.textBoxTime.Text = MinIdleTime;
                    break;
                case MIN_TOTAL_LABOR:
                    this.textBoxTime.Text = MinTotalLabor;
                    break;
                default:
                    break;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
