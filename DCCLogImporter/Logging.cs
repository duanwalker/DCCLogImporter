using System;
using System.Text;
using System.IO;

namespace DCCLogImporter
{
    public class Logging
    {
        #region Properties

        private string m_dateTimeFormat;
        private StreamWriter m_streamWriter;
        private DateTime m_start;

        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public string DateTimeFormat
        {
            set { m_dateTimeFormat = value; }
            get { return m_dateTimeFormat; }
        }

        public bool IsClosed
        {
            get { return (m_streamWriter == null); }
        }

        public bool IsOpen
        {
            get { return !IsClosed; }
        }

        public string FilePath
        {
            get { return (IsClosed) ? null : ((FileStream)m_streamWriter.BaseStream).Name; }
        }
        #endregion

        #region Constructors

        public Logging()
        {
            DateTimeFormat = DefaultDateTimeFormat;
        }

        public Logging(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
        #endregion

        #region Public Methods

        public void Start()
        {
            m_start = DateTime.Now;

            string executablePath = System.Windows.Forms.Application.ExecutablePath;

            string executableName = Path.GetFileNameWithoutExtension(executablePath);
            string dateTime = m_start.ToString("yyyyMMddHHmmss");
            string fileName = string.Format("{0}_{1}.log", executableName, dateTime);

            string directoryPath = Path.GetDirectoryName(executablePath);
            string filePath = Path.Combine(directoryPath, fileName);
            filePath = ValidateOutputFile(filePath);

            m_streamWriter = new StreamWriter(filePath, true, Encoding.Default);
            
            Write("Application Start");
        }

        public void Close()
        {
            try
            {
                if (m_streamWriter != null)
                {
                    Write("Application End ({0})", DateTime.Now.Subtract(m_start).ToString());
                    m_streamWriter.Close();
                }
            }
            catch
            {
            }
            finally
            {
                m_streamWriter = null;
            }
        }

        public void Write(string value)
        {
            if (IsClosed)
                return;

            m_streamWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(m_dateTimeFormat), value);
            m_streamWriter.Flush();
        }

        public void Write(Exception exception)
        {
            Write("EXCEPTION: " + exception.Message);
            while ((exception = exception.InnerException) != null)
            {
                Write("InnerEXCEPTION: " + exception.Message);
            }
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        public void WriteLine()
        {
            if (IsClosed)
                return;

            m_streamWriter.WriteLine();
            m_streamWriter.Flush();
        }
        #endregion

        #region Private Methods
        private static string ValidateOutputFile(string filePath)
        {
            return ValidateOutputFile(filePath, true);
        }

        private static string ValidateOutputFile(string filePath, bool createPath)
        {
            filePath = TrimEmpty(filePath);
            if (filePath.Length == 0)
                throw new ApplicationException("File path was not specified");

            filePath = Path.GetFullPath(filePath);
            string directoryName = Path.GetDirectoryName(filePath);

            if (createPath)
                Directory.CreateDirectory(directoryName);

            if (!Directory.Exists(directoryName))
                throw new ApplicationException("The specified path does not exist");

            return filePath;
        }

        private static string TrimEmpty(string value)
        {
            return (value == null) ? string.Empty : value.Trim();
        }

        #endregion
    }
}
