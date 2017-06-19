using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ComponentModel;

namespace DCCLogImporter
{
    //DCC Manifest Log processing class
    public class DCCManifestProcessing : WorkerBase
    {
        #region Properties

        //fields properties methods and events go here...
        string line;
        string[] fileEntries;
        private string ProcessName = "DCCManifestProcess";
        double ProcessIndex = 3;
        private string LogFilter = "*.log";
        string ManifestGenProcessingIdentifier = "Start Manifest Generation";
        string FtpDownloadProcessingIdentifier = "Start FtpDownload";
        string StartIdentifier = "Start file: ";
        string PublisherNameIdentifier = ")";
        string m_SourceDirectory = "";
        string ExceptionIdentifier = "EXCEPTION:";
        string Exception = "";
        public static int dccManiFilesProcessed = 0;
        string ActualFileName = "";

        #endregion
        
        //constructor
        public DCCManifestProcessing(BackgroundWorker backgroundWorker, DoWorkEventArgs doWorkEventArgs, string SourceDirectory)
            : base(backgroundWorker, doWorkEventArgs)
        {
            m_SourceDirectory = SourceDirectory;
        }

        protected override object Process()
        {
            //place in log that it is processing the list of files found in the directory.
            ReportProgress(new ProgressStatus(ProcessName, string.Format("Processing directory: {0}", m_SourceDirectory)));

            //obtain a list of files that meet log filter specifications
            fileEntries = Directory.GetFiles(m_SourceDirectory, LogFilter);

            //place in log number of files found in directory
            ReportProgress(new ProgressStatus(ProcessName, string.Format("{0} files found.", dccManiFilesProcessed)));

            foreach (string fileName in fileEntries) 
            {                
                //increment files counters
                dccManiFilesProcessed++;

                //get process log file name from fileName string
                ActualFileName = fileName.Substring(m_SourceDirectory.Length + 1);

                //post to log the file being processed
                ReportProgress(new ProgressStatus(ProcessName, string.Format("Processing file:{0} ...", ActualFileName)));
                ReportProgress(new ProgressStatus(ProcessName, null, dccManiFilesProcessed, fileEntries.Length));

                //check to see if the current file has already processed
                if (!HasLogProcessed(ActualFileName))
                {
                    //if not then run import method
                    ImportFile(fileName);
                }
                else
                {
                    // otherwise log message that file already exists
                    ReportProgress(new ProgressStatus(ProcessName, string.Format("This log: {0} has already been processed and loaded to database", ActualFileName)));
                }

                Archiver compress = new Archiver();
                compress.Zip(fileName, m_SourceDirectory);
                compress.Zip(fileName.Replace(".log", ".Exceptions"), m_SourceDirectory);

                ReportProgress(new ProgressStatus(ProcessName, string.Format(" complete. {0} file(s) processed.", dccManiFilesProcessed), dccManiFilesProcessed, fileEntries.Length));
            }
            return null;
         }
         private void ImportFile(string fileName)
         {
            //create list for each file containing properties that will go to db
            List<LogInfo> LogInfoList = new List<LogInfo>();

            
            #region ReadFile
                       
            // read the file line by line and parse needed data into variables
            StreamReader file = new StreamReader(fileName);

            LogInfo li = null;
            Boolean blnProcessFtpRecords = false;
            Boolean blnProcessManifestRecords = false;

            while ((line = file.ReadLine()) != null)
            {
                //search for ftp records
                if (line.Contains(FtpDownloadProcessingIdentifier))
                {
                    blnProcessFtpRecords = true;
                    ProcessIndex = 3.1;
                }

                if (!blnProcessFtpRecords) continue;

                if (line.Contains(StartIdentifier))
                {

                    if (li != null)
                    {
                        //add loginfo li to loginfolist
                        LogInfoList.Add(li);
                    }
                    li = new LogInfo();

                    //create variables to hold parsed data
                    string tempString = line;
                    string ProcessDate = tempString.Substring(1, 23);
                    int firstCharacter = tempString.IndexOf(PublisherNameIdentifier);
                    string SourceFile = tempString.Substring(firstCharacter + 1);
                    string SourceIP = string.Empty;
                    string ISBN = "";
                    string pattern = @"\d{13}?";
                    Regex rgx = new Regex(pattern);
                    if (rgx.IsMatch(SourceFile))
                    {
                        Match match = rgx.Match(SourceFile);
                        ISBN = match.Value;
                    }

                    li.ProcessName = ProcessName;
                    li.ProcessIndex = ProcessIndex;
                    li.ProcessLogFileName = ActualFileName;
                    li.PublicationFileName = SourceFile;
                    li.SourceIP = SourceIP;
                    li.ISBN = ISBN;
                    li.ProcessDate = ProcessDate;
                    li.Exception = String.Empty;

                    continue;
                }

                //search for manifest records
                if (line.Contains(ManifestGenProcessingIdentifier))
                {
                    blnProcessManifestRecords = true;
                    ProcessIndex = 3.2;
                }
                if (!blnProcessManifestRecords) continue;

                if (line.Contains(StartIdentifier))
                {

                    if (li != null)
                    {
                        //add loginfo li to loginfolist
                        LogInfoList.Add(li);
                    }
                    li = new LogInfo();

                    //create variables to hold parsed data
                    string tempString = line;
                    string ProcessDate = tempString.Substring(1, 23);
                    int firstCharacter = tempString.IndexOf(PublisherNameIdentifier);
                    string SourceFile = tempString.Substring(firstCharacter + 1);
                    string SourceIP = string.Empty;
                    string ISBN = "";
                    string pattern = @"\d{13}?";
                    Regex rgx = new Regex(pattern);
                    if (rgx.IsMatch(SourceFile))
                    {
                        Match match = rgx.Match(SourceFile);
                        ISBN = match.Value;
                    }

                    li.ProcessName = ProcessName;
                    li.ProcessIndex = ProcessIndex;
                    li.ProcessLogFileName = ActualFileName;
                    li.PublicationFileName = SourceFile;
                    li.SourceIP = SourceIP;
                    li.ISBN = ISBN;
                    li.ProcessDate = ProcessDate;
                    li.Exception = String.Empty;

                    continue;
                }

                if (li != null && line.Contains(ExceptionIdentifier))
                {
                    int startingCharacter = line.IndexOf(ExceptionIdentifier);
                    Exception = line.Substring(startingCharacter + ExceptionIdentifier.Length);

                    li.Exception = Exception;
                }
               
            }   //end of file reading

            if (li != null)
            {
                //add loginfo li to loginfolist
                LogInfoList.Add(li);
            }

            file.Close();
            #endregion
                
            //if the list is populated, send to database
            if (LogInfoList.Count > 0)
            {
                //report to log the number of records being sent to database
                ReportProgress(new ProgressStatus(ProcessName, string.Format("Saving records to database: {0}", LogInfoList.Count)));

                //save list of data elements to database
                LogInfo saveLogInfo = new LogInfo();
                saveLogInfo.SaveToDatabase(LogInfoList);                        

                // report archiving to log 
                ReportProgress(new ProgressStatus(ProcessName, string.Format(": Compressing file:{0}...", fileName)));
            }
         }                

        Boolean HasLogProcessed(string filename)
        {
            Boolean blnFileExist = false;

            //set up connection string
            string connectionString = ConfigurationManager.AppSettings["dbConnection"];
           
            //set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM ProcessLogFiles WHERE ProcessLogFile = @fileName");
                    cmd.Parameters.AddWithValue("@fileName", filename);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        blnFileExist = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }     
            }

            return blnFileExist;
        }

}
}
