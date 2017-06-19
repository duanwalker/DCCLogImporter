using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace DCCLogImporter
{

     

    public partial class Form1 : Form
    {
        //initialize variables
        string FTPfileEntries;
        string EBSfileEntries;
        string DCCManifileEntries;
        string DCCLoaderfileEntries;
        string RNfileEntries;
        int totalFail = 0;
        int totalSuccess = 0;
        string[] FTPfiles ;
        string[] EBSfiles;
        string[] DCCManifiles;
        string[] DCCLoaderfiles;
        string[] RNfiles;

        private Logging m_logging = new Logging();

        //BackgroundWorker m_backgroundWorker;
        BackgroundWorker m_backgroundWorkerFTP;
        BackgroundWorker m_backgroundWorkerEBS;
        BackgroundWorker m_backgroundWorkerRN;
        BackgroundWorker m_backgroundWorkerDCCManifest;
        BackgroundWorker m_backgroundWorkerDCCLoader;
        BackgroundWorker m_backgroundWorkerArchiver;

        //List<BackgroundWorker> lstBackgroundWorker = new List<BackgroundWorker>();

        public Form1(string[] args)
        {
            InitializeComponent();

            //string FTPLogFileSourceDirectory = ConfigurationManager.AppSettings["FTPLogFileSourceDirectory"];

            this.Text = string.Format("{0} (v{1})", Application.ProductName, Application.ProductVersion);

            // Create a background worker thread that ReportsProgress and SupportsCancellation and hook up the appropriate events.
            m_backgroundWorkerFTP = new BackgroundWorker();
            m_backgroundWorkerFTP.DoWork += new DoWorkEventHandler(BackgroundWorkerFTP_DoWork);
            m_backgroundWorkerFTP.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerFTP.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerFTP.WorkerReportsProgress = true;
            m_backgroundWorkerFTP.WorkerSupportsCancellation = true;

            m_backgroundWorkerEBS = new BackgroundWorker();
            m_backgroundWorkerEBS.DoWork += new DoWorkEventHandler(BackgroundWorkerEBS_DoWork);
            m_backgroundWorkerEBS.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerEBS.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerEBS.WorkerReportsProgress = true;
            m_backgroundWorkerEBS.WorkerSupportsCancellation = true;

            m_backgroundWorkerRN = new BackgroundWorker();
            m_backgroundWorkerRN.DoWork += new DoWorkEventHandler(BackgroundWorkerRN_DoWork);
            m_backgroundWorkerRN.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerRN.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerRN.WorkerReportsProgress = true;
            m_backgroundWorkerRN.WorkerSupportsCancellation = true;

            m_backgroundWorkerDCCManifest = new BackgroundWorker();
            m_backgroundWorkerDCCManifest.DoWork += new DoWorkEventHandler(BackgroundWorkerDCCManifest_DoWork);
            m_backgroundWorkerDCCManifest.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerDCCManifest.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerDCCManifest.WorkerReportsProgress = true;
            m_backgroundWorkerDCCManifest.WorkerSupportsCancellation = true;

            m_backgroundWorkerDCCLoader = new BackgroundWorker();
            m_backgroundWorkerDCCLoader.DoWork += new DoWorkEventHandler(BackgroundWorkerDCCLoader_DoWork);
            m_backgroundWorkerDCCLoader.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerDCCLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerDCCLoader.WorkerReportsProgress = true;
            m_backgroundWorkerDCCLoader.WorkerSupportsCancellation = true;

            m_backgroundWorkerArchiver = new BackgroundWorker();
            m_backgroundWorkerArchiver.DoWork += new DoWorkEventHandler(BackgroundWorkerArchiver_DoWork);
            m_backgroundWorkerArchiver.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
            m_backgroundWorkerArchiver.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
            m_backgroundWorkerArchiver.WorkerReportsProgress = true;
            m_backgroundWorkerArchiver.WorkerSupportsCancellation = true;

            if (args.Length >= 2 && args[1] == "/Start")
            {
                btnStart_Click(this, new EventArgs());
            }
        }

        #region FormEvents
        private void Form1_Load(object sender, System.EventArgs e)
        {
            m_logging.Start();
            formLoad();
                  
        }

        private void Form1_Closed(object sender, System.EventArgs e)
        {
            m_logging.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;

                m_logging.Write(System.Reflection.MethodBase.GetCurrentMethod().Name);

                m_backgroundWorkerFTP.RunWorkerAsync();
                lblFTPStatus.Visible = true;
                m_backgroundWorkerEBS.RunWorkerAsync();
                lblEBSLoaderStatus.Visible = true;
                m_backgroundWorkerRN.RunWorkerAsync();
                lblRNPackageStatus.Visible = true;
                m_backgroundWorkerDCCManifest.RunWorkerAsync();
                lblDCCManifestStatus.Visible = true;
                m_backgroundWorkerDCCLoader.RunWorkerAsync();
                lblDCCLoaderStatus.Visible = true;

                //m_backgroundWorkerArchiver.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                m_logging.Write("{0} {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void formLoad(){
            m_logging.Write(System.Reflection.MethodBase.GetCurrentMethod().Name);

            string FTPLogFilter = "u_ex*.log";
            string EBSLogFilter = "EBSLoader*.log";
            string DCCManiLogFilter = "BT.DCCManifestGen_*.log";
            string DCCLoaderLogFilter = "DCCLoader_*.log";
            string RNLogFilter = "RNPackage_*.log";

            if (Object.ReferenceEquals(null, ConfigurationManager.AppSettings["FTPLogFileSourceDirectory"]) != true)
            {
                FTPfileEntries = ConfigurationManager.AppSettings.Get("FTPLogFileSourceDirectory");
            }
            if (Object.ReferenceEquals(null, ConfigurationManager.AppSettings["DCCLoaderLogSourceDirectory"]) != true)
            {
                DCCLoaderfileEntries = ConfigurationManager.AppSettings.Get("DCCLoaderLogSourceDirectory");
            }
            if (Object.ReferenceEquals(null, ConfigurationManager.AppSettings["DCCManifestLogSourceDirectory"]) != true)
            {
                DCCManifileEntries = ConfigurationManager.AppSettings.Get("DCCManifestLogSourceDirectory");
            }
            if (Object.ReferenceEquals(null, ConfigurationManager.AppSettings["RNPackageLogFileDirectory"]) != true)
            {
                RNfileEntries = ConfigurationManager.AppSettings.Get("RNPackageLogFileDirectory");
            }
            if (Object.ReferenceEquals(null, ConfigurationManager.AppSettings["EBSLoaderLogFileSource"]) != true)
            {
                EBSfileEntries = ConfigurationManager.AppSettings.Get("EBSLoaderLogFileSource");
            }
            FTPfiles = Directory.GetFiles(FTPfileEntries, FTPLogFilter);
            EBSfiles = Directory.GetFiles(EBSfileEntries, EBSLogFilter);
            DCCManifiles = Directory.GetFiles(DCCManifileEntries, DCCManiLogFilter);
            DCCLoaderfiles = Directory.GetFiles(DCCLoaderfileEntries, DCCLoaderLogFilter);
            RNfiles = Directory.GetFiles(RNfileEntries, RNLogFilter);

            lblFTP.Text = "FTP Log Process has : " + (FTPfiles.Length).ToString() + " files to process.";
            m_logging.Write("{0}",lblFTP.Text);
            lblEBSLoader.Text = "EBS Loader Log Process has : " + (EBSfiles.Length).ToString() + " files to process.";
            m_logging.Write("{0}", lblEBSLoader.Text);
            lblDCCLoader.Text = "DCC Loader Log Process has : " + (DCCLoaderfiles.Length).ToString() + " files to process.";
            m_logging.Write("{0}", lblDCCLoader.Text);
            lblDCCManifest.Text = "DCC Manifest Log Process has : " + (DCCManifiles.Length).ToString() + " files to process.";
            m_logging.Write("{0}", lblDCCManifest.Text);
            lblRNPackage.Text = "RN Package Log Process has : " + (RNfiles.Length).ToString() + " files to process.";
            m_logging.Write("{0}", lblRNPackage.Text);

            int totals = FTPfiles.Length + EBSfiles.Length + DCCLoaderfiles.Length + DCCManifiles.Length + RNfiles.Length;

            lblTotalCompletion.Text = "Total number of files to process: " + (totals).ToString();
        }
        #endregion


        #region BackgroundWorker Events
        void BackgroundWorkerFTP_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseFTP=null;
            try
            {
                BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                workerBaseFTP = new FTPProcessing(backgroundWorker, e, ConfigurationManager.AppSettings["FTPLogFileSourceDirectory"]);
                workerBaseFTP.DoWork();
            }
            catch (Exception ex)
            {
                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseFTP.GetType(), ex));
                totalFail++;
            } 
        }

        void BackgroundWorkerEBS_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseEBS = null;
            try
            {
                 BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                 workerBaseEBS = new EBSLoaderProcessing(backgroundWorker, e, ConfigurationManager.AppSettings["EBSLoaderLogFileSource"]);
                 workerBaseEBS.DoWork();
            }
            catch (Exception ex)
            {

                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseEBS.GetType(), ex));
                totalFail++;
            }
        }

        void BackgroundWorkerRN_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseRN = null;
            try
            {
                BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                workerBaseRN = new RNPackageProcessing(backgroundWorker, e, ConfigurationManager.AppSettings["RNPackageLogFileDirectory"]);
                workerBaseRN.DoWork();
            }
            catch (Exception ex)
            {

                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseRN.GetType(), ex));
                totalFail++;
            }
        }

        void BackgroundWorkerDCCManifest_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseDCCManifest = null;
            try
            {
                BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                workerBaseDCCManifest = new DCCManifestProcessing(backgroundWorker, e, ConfigurationManager.AppSettings["DCCManifestLogSourceDirectory"]);
                workerBaseDCCManifest.DoWork();
            }
            catch (Exception ex)
            {

                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseDCCManifest.GetType(), ex));
                totalFail++;
            }
        }

        void BackgroundWorkerDCCLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseDCCLoader = null;
            try
            {
                BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                workerBaseDCCLoader = new DCCLoaderProcessing(backgroundWorker, e, ConfigurationManager.AppSettings["DCCLoaderLogSourceDirectory"]);
                workerBaseDCCLoader.DoWork();
            }
            catch (Exception ex)
            {

                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseDCCLoader.GetType(), ex));
                totalFail++;
            }            
        }

        void BackgroundWorkerArchiver_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerBase workerBaseArchiver = null;
            try
            {
                BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
                workerBaseArchiver = new Archiver(backgroundWorker, e);
                workerBaseArchiver.DoWork();
            }
            catch (Exception ex)
            {

                m_logging.Write(string.Format("{0} failed at {1}.", workerBaseArchiver.GetType(), ex));
                totalFail++;
            }
        }

        void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;

            ProgressStatus ps = (ProgressStatus)e.UserState;

            
           

            #region process name switch
            switch (ps.ProcessName)
            {
                case "FTPProcess":

                        lblFTP.Text = "FTPProcess: " + ps.Message;

                        progressBarFTP.Maximum = FTPfiles.Length;

                        if (ps.CurrentValue > 0)
                        {
                        progressBarFTP.Value = ps.CurrentValue;
                        lblFTPStatus.Text = string.Format("Processing File: {0} of {1}", progressBarFTP.Value, progressBarFTP.Maximum);
                        }                
                       
                        progressBarFTP.Refresh();
                    break;

                case "EBSLoaderProcess":
                        lblEBSLoader.Text = "EBSLoaderProcess: " + ps.Message;
                        progressBarEBSLoader.Maximum = EBSfiles.Length;

                    if (ps.CurrentValue > 0)
                    {
                        progressBarEBSLoader.Value = ps.CurrentValue;
                        lblEBSLoaderStatus.Text = string.Format("Processing File: {0} of {1}", progressBarEBSLoader.Value, progressBarEBSLoader.Maximum);
                    }                  
                    progressBarEBSLoader.Refresh();
                    break;

                case "RNPackageProcess":
                        lblRNPackage.Text = "RNPackageProcess: " + ps.Message;
                        progressBarRNPackage.Maximum = RNfiles.Length;

                    if (ps.CurrentValue > 0)
                    {
                        progressBarRNPackage.Value = ps.CurrentValue;
                        lblRNPackageStatus.Text = string.Format("Processing File: {0} of {1}", progressBarRNPackage.Value, progressBarRNPackage.Maximum);
                    }         
                    progressBarRNPackage.Refresh();
                    break;

                case "DCCManifestProcess":
                    lblDCCManifest.Text = "DCCManifestProcess: " + ps.Message;
                    progressBarDCCManifest.Maximum = DCCManifiles.Length;
                    if (ps.CurrentValue > 0)
                    {
                        progressBarDCCManifest.Value = ps.CurrentValue;
                        lblDCCManifestStatus.Text = string.Format("Processing File: {0} of {1}", progressBarDCCManifest.Value, progressBarDCCManifest.Maximum);
                    }
                    progressBarDCCManifest.Refresh();
                    break;

                case "DCCLoaderProcess":
                    lblDCCLoader.Text = "DCCLoaderProcess: " + ps.Message;
                    progressBarDCCLoader.Maximum = DCCLoaderfiles.Length;
                    if (ps.CurrentValue > 0)
                    {
                        progressBarDCCLoader.Value = ps.CurrentValue;
                        lblDCCLoaderStatus.Text = string.Format("Processing File: {0} of {1}", progressBarDCCLoader.Value, progressBarDCCLoader.Maximum);
                    }
                    progressBarDCCLoader.Refresh();
                    break;

                default:
                    break;
            }
            
            #endregion 

            if (ps.CurrentValue > 0)
            {
                m_logging.Write("{0} {1} {2} {3}", ps.ProcessName, ps.CurrentValue, ps.MaxValue, ps.Message);
            }
            else
            {
                m_logging.Write("{0} {1}", ps.ProcessName, ps.Message);
            }
                


            //update total progress bar and status labels
            double progressBarPercentage;

            double ProgressBarTotalMax = FTPfiles.Length + EBSfiles.Length + DCCLoaderfiles.Length + DCCManifiles.Length + RNfiles.Length;
            double ProgressBarTotalCurrent = progressBarDCCLoader.Value + progressBarDCCManifest.Value + progressBarRNPackage.Value + progressBarEBSLoader.Value + progressBarFTP.Value;
            if (ProgressBarTotalCurrent < ProgressBarTotalMax)
            {
                progressBarPercentage = (ProgressBarTotalCurrent / ProgressBarTotalMax) * 100.00;
            }
            else progressBarPercentage = 100;
            if (progressBarPercentage == 100)
            {
                
                totalSuccess = FTPProcessing.ftpFilesProcessed + RNPackageProcessing.rnPackageFilesProcessed + EBSLoaderProcessing.ebsFilesProcessed + DCCLoaderProcessing.dccLoaderFilesProcessed + DCCManifestProcessing.dccManiFilesProcessed;

                lblFinalCompletion.Text = string.Format("Total number of new files successfully processed: {0} ",totalSuccess );
                lblFinalFails.Text = string.Format("Total number of files not processed: {0} ", totalFail);

               // if (!m_backgroundWorkerArchiver.IsBusy)
                 //   m_backgroundWorkerArchiver.RunWorkerAsync();
                
                
               // btnStart.Enabled = true;

                m_logging.Close();
            }
            progressBarTotalCompletion.Maximum = (int)ProgressBarTotalMax;
            progressBarTotalCompletion.Value = (int)ProgressBarTotalCurrent;


            //lblTotalCompletion.Text = string.Format("Total Completion: {0} of {1}", ProgressBarTotalCurrent, ProgressBarTotalMax);
            lblTotalCompletion.Text = string.Format("Total Completion: {0} %", progressBarPercentage.ToString("##.##"));
         
            this.Refresh();
            
        }

        void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
            //btnStart.Enabled = true;
        }
        #endregion

        //public string[] FTPfileEntries { get; set; }
    }
}
