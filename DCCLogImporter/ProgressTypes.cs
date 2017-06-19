using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCCLogImporter
{
    public class ProgressStatus
    {
        public string ProcessName { get; private set; }
        public string Message { get; private set; }
        public int CurrentValue { get; private set; }
        public int MaxValue { get; private set; }

        public ProgressStatus(string identifier, string message)
        {
            ProcessName = identifier;
            Message = message;
        }

        public ProgressStatus(string identifier, string message, int number, int maxnumber)
        {
            ProcessName = identifier;
            Message = message;
            CurrentValue = number;
            MaxValue = maxnumber;
        }
    }

}
