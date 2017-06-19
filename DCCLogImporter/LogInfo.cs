using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DCCLogImporter
{
    class LogInfo
    {
        //fields properties methods and events go here...
        public string ProcessName{get;set;}
        public double ProcessIndex { get; set; }
        public string ProcessLogFileName{get;set;}
        public string PublicationFileName { get; set; }
        public string ProcessDate { get; set; }
        public string SourceIP { get; set; }
        public string ISBN { get; set; }
        public string Exception { get; set; }
        
        public int SaveToDatabase(List<LogInfo> logInfoList)
        {
            int recordsinserted = 0;

            List<LogInfo> lstInfoList = new List<LogInfo>();
            lstInfoList = logInfoList;

            //set up connection string
            string connectionString = ConfigurationManager.AppSettings["dbConnection"];
            //string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            //set up connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd =
                    new SqlCommand(
                        "INSERT INTO ProcessLogFiles (ProcessName, ProcessIndex, ProcessLogFile, PublicationFileName, ProcessDate, SourceIP, ISBN, Exception) " +
                        " VALUES (@ProcessName, @ProcessIndex, @ProcessLogFileName, @PublicationFileName, @ProcessDate, @SourceIP, @ISBN, @Exception )");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                try
                {
                    foreach (LogInfo li in lstInfoList)
                       {
                         cmd.Parameters.Clear();

                         cmd.Parameters.AddWithValue("@ProcessName", li.ProcessName);
                         cmd.Parameters.AddWithValue("@ProcessIndex", li.ProcessIndex);
                         cmd.Parameters.AddWithValue("@ProcessLogFileName", li.ProcessLogFileName);
                         cmd.Parameters.AddWithValue("@PublicationFileName", li.PublicationFileName);
                         cmd.Parameters.AddWithValue("@ProcessDate", li.ProcessDate);
                         cmd.Parameters.AddWithValue("@SourceIP", li.SourceIP);
                         cmd.Parameters.AddWithValue("@ISBN", li.ISBN);
                         cmd.Parameters.AddWithValue("@Exception", li.Exception);

                         cmd.ExecuteNonQuery();

                         recordsinserted++;
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
            return recordsinserted;
        }
    }
}
