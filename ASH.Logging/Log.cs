using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace ASH.Logging
{
    public class Log
    {
        private static string logFile = ConfigurationManager.AppSettings["LogFile"].ToString();
        private static StreamWriter sw = null;
        public static void LogMessage(Exception ex)
        {
            sw = new StreamWriter(logFile + "Log" + DateTime.Now.ToString("yyMMdd"), true);
            sw.WriteLine(DateTime.Now.ToString() + ":" + ex.Message);
            sw.Flush();
            sw.Close();
        }
        public static void LogMessage(string message)
        {
            sw = new StreamWriter(logFile + "Log" + DateTime.Now.ToString("yyMMdd"), true);
            sw.WriteLine(DateTime.Now.ToString() + ":" + message);
            sw.Flush();
            sw.Close();
        }
    }
}
