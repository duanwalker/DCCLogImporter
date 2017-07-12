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
    //EBSLoaderProcessing class
    public class EBSLoaderProcessing : WorkerBase
    {
        #region Properties

        //fields properties methods and events go here...
        int lineCounter = 0;
        string line;
        string[] fileEntries;
        string ProcessName = "EBSLoaderProcess";
        int ProcessIndex = 4;
        string LogFilter = "*.log";
        string identifier = "Processing: ";
        public static int ebsFilesProcessed = 0;
        string m_SourceDirectory = "";
        string ActualFileName = "";
        public static Boolean ebsFilesComplete = false;
        #endregion
        
        //constructor
        public EBSLoaderProcessing(BackgroundWorker backgroundWorker, DoWorkEventArgs doWorkEventArgs, string SourceDirectory)
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
            ReportProgress(new ProgressStatus(ProcessName, string.Format("{0} files found.", fileEntries.Length)));
            if (fileEntries.Length != 0)
            {
                foreach (string fileName in fileEntries) 
                {             
                    ActualFileName = fileName.Substring(m_SourceDirectory.Length + 1);
                
                    //post to log the file being processed
                    ReportProgress(new ProgressStatus(ProcessName, string.Format("Processing file:{0} ...", ActualFileName)));

                    //check to see if current file has already processed
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
                    //increment file counters
                    ebsFilesProcessed++;
                    ReportProgress(new ProgressStatus(ProcessName, string.Format(" complete. {0} of {1} file(s) processed.", ebsFilesProcessed, fileEntries.Length), ebsFilesProcessed, fileEntries.Length));
                    if (ebsFilesProcessed == fileEntries.Length)
                    {
                        ebsFilesComplete = true;
                    }
                    else
                    {
                        ebsFilesComplete = false;
                    }
                }
            }
            else
            {
                ebsFilesComplete = true;
            }
            return null;
        }

        private void ImportFile(string fileName)
        {
            //create list for each file containing properties that will go to db
            List<LogInfo> LogInfoList = new List<LogInfo>();

            // read the file line by line and parse needed data into variables
            #region ReadFile

            StreamReader file = new StreamReader(fileName);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains(identifier))
                {
                    //create variables to hold parsed data
                    string tempString = line;
                    string ProcessDate = tempString.Substring(1, 23);
                    //string fileNameStart = "/";
                    //int fileNameCharacter = tempString.IndexOf(fileNameStart);
                    string SourceFile = tempString.Substring(38);
                    //string SourceIP = tempString.Substring(20,15);
                    string ISBN = "";
                    string pattern = @"\d{13}?";
                    Regex rgx = new Regex(pattern);
                    if (rgx.IsMatch(SourceFile))
                    {
                        Match match = rgx.Match(SourceFile);
                        ISBN = match.Value;
                    }

                    //instantiate loginfo class
                    LogInfo li = new LogInfo();
                    li.ProcessName = ProcessName;
                    li.ProcessIndex = ProcessIndex;
                    li.ProcessLogFileName = ActualFileName;
                    li.PublicationFileName = SourceFile;
                    li.ISBN = ISBN;
                    li.ProcessDate = ProcessDate;
                    li.SourceIP = string.Empty;
                    li.Exception = string.Empty;

                    //add loginfo li to loginfolist
                    LogInfoList.Add(li);
                            
                    //increment line counter
                    lineCounter++;

                }

            }   //end of file reading

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
