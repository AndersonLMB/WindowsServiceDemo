using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using ConsoleHttpListen;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace SvHttpListen
{
    public partial class TileService : ServiceBase
    {
        public TileService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {


            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\test\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Start.");
            }
            var oldProcessname = ConfigurationManager.AppSettings["ConsoleHttpListenExeFilename"].ToString().Split('.')[0];
            TryKillOldProcess(oldProcessname);

            var consoleHttpListenExeDirectory = ConfigurationManager.AppSettings["ConsoleHttpListenExeDirectory"].ToString();
            var consoleHttpListenExeFilename = ConfigurationManager.AppSettings["ConsoleHttpListenExeFilename"].ToString();
            var processName = Path.Combine(consoleHttpListenExeDirectory, consoleHttpListenExeFilename);
            TryStartProcess(processName);
        }

        protected override void OnStop()
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\test\\log.txt", true))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Stop.");
            }
            var processName = ConfigurationManager.AppSettings["ConsoleHttpListenExeFilename"].ToString().Split('.')[0];
            TryKillOldProcess(processName);
        }

        private void TryKillOldProcess(string processName)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\test\\log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Try Kill Processes");
                }
                var processes = Process.GetProcessesByName(processName);
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\test\\log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + String.Format("{0} process(es) found", processes.Length.ToString()));
                }

                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\test\\log.txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ex.Message);
                }
                throw;
            }
        }

        private void TryStartProcess(string processName)
        {
            Process.Start(processName);
        }

    }
}
