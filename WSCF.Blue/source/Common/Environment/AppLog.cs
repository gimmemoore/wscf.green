using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Thinktecture.Tools.Web.Services.Wscf.Environment
{
    public static class AppLog
    {
        public static void LogMessage(string message)
        {
            string t = WscfConfiguration.AppSettings["trace"];

            if ((t != null) && (t == "1" || t.ToLower() == "true"))
            {
                Mutex flock = null;
                try
                {
                    flock = new Mutex(false, "WSCFLOG");

                    if (flock.WaitOne())
                    {

                        string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string logFile = directory + "\\" + "WSCF.log";
                        StreamWriter writer = new StreamWriter(logFile, true, Encoding.UTF8);
                        writer.WriteLine(DateTime.Now.ToString("M-dd-yyyy H:mm"));
                        writer.WriteLine(message);
                        writer.WriteLine();
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("WSCF Log",
                                        string.Format("An error occrred while trying to write the message {0} to the log. Details: {1}",
                                                      message, ex.Message), EventLogEntryType.Error);
                }
                finally
                {
                    if (flock != null)
                    {
                        flock.ReleaseMutex();
                    }
                }
            }
        }       
    }
}