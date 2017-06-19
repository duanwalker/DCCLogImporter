using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DCCLogImporter
{
    public abstract class WorkerBase
    {
        private BackgroundWorker m_backgroundWorker;
        private DoWorkEventArgs m_doWorkEventArgs;
        //private Logging m_logging;
        //private Stopwatch m_stopwatch;

        //protected abstract void WriteHeader();
         protected abstract object Process();

        public WorkerBase(BackgroundWorker backgroundWorker, DoWorkEventArgs doWorkEventArgs)
        {
            m_backgroundWorker = backgroundWorker;
            m_doWorkEventArgs = doWorkEventArgs;

            //m_logging = new Logging();
            //m_stopwatch = new Stopwatch();
        }

        public WorkerBase() { }

        protected void ReportProgress(object userState)
        {
            m_backgroundWorker.ReportProgress(-1, userState);
        }
        protected void ReportProgress(int percentProgress, object userState)
        {
            m_backgroundWorker.ReportProgress(percentProgress, userState);
        }

        protected void LogWrite(string value)
        {
            //m_logging.Write(value);
        }
        protected void LogWrite(string format, params object[] args)
        {
            //m_logging.Write(format, args);
        }

        protected void StopwatchRestart()
        {
            //m_stopwatch.Restart();
        }
        protected bool IsStopwatchUpdateInterval
        {
            //get { return (m_stopwatch.Elapsed.TotalSeconds >= Properties.Settings.Default.ScreenUpdateSecondsInterval); }
            get { return (true); }
        }

        protected bool IsCancellationPending
        {
            get { return m_backgroundWorker.CancellationPending; }
        }
        protected void ThrowCancelProcessException()
        {
            throw new CancelProcessException();
        }
        protected void CancelIfPending()
        {
            if (IsCancellationPending)
                ThrowCancelProcessException();
        }

        public void DoWork()
        {
            try
            {
                //m_logging.Start();

                //WriteHeader();

                m_doWorkEventArgs.Result = Process();
            }
            catch (CancelProcessException)
            {
                //m_logging.Write("Process cancelled by user");
                m_doWorkEventArgs.Cancel = true;
            }
            catch (Exception exception)
            {
                //m_logging.Write(exception);
                throw exception;
            }
            finally
            {
                //m_logging.Close();
            }
        }
    }


    public class CancelProcessException : ApplicationException
    {
        public CancelProcessException()
        {
        }
    }
}
