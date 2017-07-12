using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Specialized;
//using System.IO.Compression.FileSystem;

namespace DCCLogImporter
{
    class Archiver : WorkerBase
    {
        //set variables 
     //   string zipSavePath = ConfigurationManager.AppSettings["PkzipSavePath"];
        string zipExecutablePath = ConfigurationManager.AppSettings["PkzipExecutablePath"];
        string zipSavePath = ConfigurationManager.AppSettings.Get("PkzipSavePath");
 

        //constructor
        public Archiver(BackgroundWorker backgroundWorker, DoWorkEventArgs doWorkEventArgs)
            : base(backgroundWorker, doWorkEventArgs)
        {

        }
        //constructor
        public Archiver() 
        {
        
        }
        public Archiver(string fileName, string m_SourceDirectory)
        {

        }


        //archive method
        public void Zip(string logfile, string ziplocation)
        {
            string fileName = "";
            string file = Path.GetFileNameWithoutExtension(logfile);
            string file2 = file.Substring(0, 3);

            //switch statement to get fileName
            switch (file2)
            {
                case "u_e":

                    fileName = file.Substring(0, 8);
                    break;

                case "EBS":
                    fileName = file.Substring(0, 16);
                    break;

                case "RNP":
                    fileName = file.Substring(0, 16);
                    break;

                case "BT.":
                    fileName = file.Substring(0, 24);
                    break;

                case "DCC":
                    fileName = file.Substring(0, 16);
                    break;

                default:
                    break;
            }

            string zipCMDParameters = @" -add -silent -move -max " + zipSavePath + fileName + ".zip " + logfile;
            ProcessStartInfo startZip = new ProcessStartInfo(zipExecutablePath, zipCMDParameters);
            startZip.WindowStyle = ProcessWindowStyle.Minimized;
            // Execute the process
            Process zipProcess = System.Diagnostics.Process.Start(startZip);
            zipProcess.WaitForExit();
        }
        //not used
        protected override object Process()
        {
            
            return null;
        }

    }
}
