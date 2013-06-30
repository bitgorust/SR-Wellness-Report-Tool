using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SR_Wellness_Report
{
    public partial class FormExport : Form
    {
        public const string PIDLMRU_PATH = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ComDlg32";

        private static bool autoMode = false;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        private const string lpLoginName = "Windows Security";
        private const string lpDwnldName = "File Download";
        private IntPtr loginHwnd = IntPtr.Zero;

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private extern static IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClass, string lpWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int GetWindowText(IntPtr hwnd, StringBuilder lpText, int nCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr parameter);
        private const int btnOK = 6;
        private const int txtPasswd = 14;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static bool SetWindowText(IntPtr hwnd, String lpString);

        //[return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true/*, CharSet = CharSet.Auto*/)]
        private extern static IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private const uint BM_CLICK = 0x00F5;
        //private const uint WM_LBUTTONDOWN = 0x0201;
        //private const uint WM_LBUTTONUP = 0x0202;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private extern static bool PostMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private extern static void keybd_event(byte bvk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const string LINK_PATH = "SSolveLink";
        private const string PASSWD_PATH = "Password";

        private const string ViewerAddr = "https://detego.partners.extranet.microsoft.com/Pages/ReportViewer.aspx";
        private const string ReportAddr = "https://detego.partners.extranet.microsoft.com/Pages/Reserved.ReportViewerWebControl.axd?";

        private const string REPORT_SESSION = "ReportSession";
        //private static string CONTROL_ID = "ControlID";
        private const string REPORT_PAGE_KEY = "Format=";

        //private static string SavePath = System.IO.Directory.GetCurrentDirectory() + "\\SRWellnessReport.xml";

        private static int waitSeconds = 12;
        private static bool downloaded = false;
        private static bool saved = false;
        private static bool founded = false;
        private static bool recording = false;

        public FormExport()
        {
            InitializeComponent();
        }

        private void clearPidlMRU()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(PIDLMRU_PATH + "\\LastVisitedPidlMRU", true);
            if (key != null)
            {
                Registry.CurrentUser.DeleteSubKey(PIDLMRU_PATH + "\\LastVisitedPidlMRU");
            }
            key = Registry.CurrentUser.OpenSubKey(PIDLMRU_PATH + "\\OpenSavePidlMRU", true);
            if (key != null)
            {
                Registry.CurrentUser.DeleteSubKeyTree(PIDLMRU_PATH + "\\OpenSavePidlMRU");
            }
        }

        private void exportForm_Load(object sender, EventArgs e)
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
                }
            }

            // TODO: load urls from database
            string sSolveAddr = ReportRegistry.readValue(LINK_PATH);
            addrComboBox.Items.Add(sSolveAddr);
            addrComboBox.Text = sSolveAddr;

            if (autoMode)
            {
                clearPidlMRU();
                exportBtn.PerformClick();
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
        {
            webBrowser.Navigate(addrComboBox.Text);
            exportBtn.Enabled = false;
            logTextBox.AppendText("Visiting " + addrComboBox.Text + "\n");
            backgroundSearcher.RunWorkerAsync();
        }

        private void nextStep()
        {
            this.Close();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string content = webBrowser.DocumentText;

            if (content.Contains(REPORT_PAGE_KEY) && !downloaded)
            {
                logTextBox.AppendText("Team Wellness Report is finally generated!\n");
                int end = content.IndexOf(REPORT_PAGE_KEY) + REPORT_PAGE_KEY.Length;
                int start = content.LastIndexOf(REPORT_SESSION, end);
                string downloadPath = ReportAddr + content.Substring(start, end - start) + "XML";
                webBrowser.Navigate(downloadPath);
                downloaded = true;
                founded = true;

                if (autoMode)
                {
                    new Thread(new ThreadStart(WaitToSave)).Start();
                }
            }
            else if (!downloaded)
            {
                backgroundWorker.RunWorkerAsync();
                logTextBox.AppendText("Waiting for response...\n");
                logTextBox.AppendText("Counting down ");
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            waitSeconds--;
            Thread.Sleep(1000);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (waitSeconds > 0)
            {
                logTextBox.AppendText(waitSeconds + " ");
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                logTextBox.AppendText("\r\n");
                logTextBox.AppendText("Team Wellness Report MAYBE generated.\n");
                webBrowser.Navigate(ViewerAddr);
                logTextBox.AppendText("Visiting " + ViewerAddr + "\n");
            }
        }

        private void backgroundSearcher_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            if (!founded)
            {
                loginHwnd = FindWindow(null, lpLoginName);
            }
        }

        private void backgroundSearcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!autoMode)
            {
                if (loginHwnd == IntPtr.Zero || GetChildWindows(loginHwnd).Count != 15)
                {
                    recording = false;
                }
                else if (!recording)
                {
                    List<IntPtr> list = GetChildWindows(loginHwnd);
                    /*
                    for (int i = 0; i < list.Count; i++)
                    {
                        StringBuilder sb = new StringBuilder(100);
                        GetWindowText(list[i], sb, 100);
                        MessageBox.Show(sb.ToString() + ":" + i);
                    }
                     * */
                    SetWindowText(list[txtPasswd], string.Empty);
                    new Thread(new ThreadStart(recordPasswd)).Start();
                    recording = true;
                }
                backgroundSearcher.RunWorkerAsync();
            }
            else if (!saved)    // auto mode
            {
                if (loginHwnd != IntPtr.Zero && GetChildWindows(loginHwnd).Count == 15)
                {
                    List<IntPtr> list = GetChildWindows(loginHwnd);
                    SetWindowText(list[txtPasswd], ReportRegistry.readValue(PASSWD_PATH));
                    SendMessage(list[btnOK], BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                }
                backgroundSearcher.RunWorkerAsync();
            }
            else
            {
                autoContinue();
            }
        }

        private static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                {
                    listHandle.Free();
                }
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            return true;
        }

        private void WaitToSave()
        {
            IntPtr downWnd = FindWindow(null, "File Download"); // Capture the File Download dialog
            while (downWnd == IntPtr.Zero)
            {
                Thread.Sleep(1000);
                downWnd = FindWindow(null, "File Download");
            }
            while (downWnd != IntPtr.Zero)
            {
                Thread.Sleep(1000);
                SendMessage(FindWindowEx(downWnd, IntPtr.Zero, "Button", "&Save"), BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                downWnd = FindWindow(null, "File Download");
            }

            IntPtr saveWnd = FindWindow(null, "Save As");   // Capture the Save As dialog
            while (saveWnd == IntPtr.Zero)
            {
                Thread.Sleep(1000);
                saveWnd = FindWindow(null, "Save As");
            }
            while (saveWnd != IntPtr.Zero)
            {
                Thread.Sleep(1000);
                List<IntPtr> list = GetChildWindows(saveWnd);
                if (list.Count == 0)
                {
                    break;
                }
                SendMessage(list[19], BM_CLICK, IntPtr.Zero, IntPtr.Zero);  // list[19] is the Save btn
                saveWnd = FindWindow(null, "Save As");
            }
            /*
            IntPtr cmplWnd = FindWindow(null, "Download complete");   // Capture the Save As dialog
            while (cmplWnd == IntPtr.Zero)
            {
                Thread.Sleep(1000);
                cmplWnd = FindWindow(null, "Download complete");
            }
            while (cmplWnd != IntPtr.Zero)
            {
                Thread.Sleep(1000);
                List<IntPtr> list = GetChildWindows(saveWnd);
                for (int i = 0; i < list.Count; i++)
                {
                    StringBuilder sb = new StringBuilder(100);
                    GetWindowText(list[i], sb, 100);
                    MessageBox.Show(sb.ToString() + ":" + i);
                }
                //SendMessage(list[19], BM_CLICK, IntPtr.Zero, IntPtr.Zero);  // list[19] is the Save btn
                cmplWnd = FindWindow(null, "Download complete");
            }
            */

            keybd_event(0x0D, 0x9C, 0, 0);      // Press down Enter
            keybd_event(0x0D, 0x9C, 0x0002, 0); // Release Enter

            saved = true;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            nextStep();
        }

        private void autoContinue()
        {
            this.Close();
        }

        private void recordPasswd()
        {
            StringBuilder passwd = new StringBuilder(128);
            string strPasswd = string.Empty;
            while (loginHwnd != IntPtr.Zero && GetChildWindows(loginHwnd).Count == 15)
            {
                passwd.Clear();
                List<IntPtr> list = GetChildWindows(loginHwnd);
                GetWindowText(list[txtPasswd], passwd, 127);
                if (passwd.ToString().Length >= strPasswd.Length)
                {
                    strPasswd = passwd.ToString();
                }
            }
            ReportRegistry.setValue(PASSWD_PATH, strPasswd);
            recording = false;
        }
    }
}
