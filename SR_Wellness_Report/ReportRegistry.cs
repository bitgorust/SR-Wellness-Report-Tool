using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace SR_Wellness_Report
{
    class ReportRegistry
    {
        const string KEY_PATH = "Software\\Microsoft\\SRWelnessReport";

        public static string readValue(string name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(KEY_PATH, false);
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(KEY_PATH);

            string value = string.Empty;
            if (key.GetValue(name) != null)
                value = key.GetValue(name).ToString();

            key.Close();

            return value;
        }

        public static void setValue(string name, string value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(KEY_PATH, true);
            if (key == null)
                key = Registry.CurrentUser.CreateSubKey(KEY_PATH);

            try
            {
                key.SetValue(name, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                key.Close();
                throw;
            }

            key.Close();
        }
    }
}
