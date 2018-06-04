using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingOddsTester
{
    internal interface IMachine
    {
        int ID { get; set; }
        string Name { get; set; }
        decimal stake { get; set; }

        HashSet<Models.TestResult> Results { get; set; }

        string ResultString { get; }

        void RunSingleTest();

        void ClearTestResults();
    }
}