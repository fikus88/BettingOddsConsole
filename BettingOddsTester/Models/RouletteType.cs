using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingOddsTester.Models
{
    internal class RouletteType : IMachine

    {
        // Interface implemented
        public int ID { get; set; }

        public string Name { get; set; }
        public List<SelectableField> Fields { get; set; }
        public decimal stake { get; set; }
        public HashSet<TestResult> Results { get; set; }

        public string ResultString
        {
            get
            {
                int NumOfTests = Results.Max(x => x.testNum);
                decimal Profit = Results.Sum(x => x.profit);
                decimal PercentProfit = Profit / NumOfTests;

                return String.Format("Number of tests {0}, Profit: {1:c}, Percentage profit: {2:p}", NumOfTests, Profit, PercentProfit);
            }
        }

        // ----------------------

        public RouletteType(string MachineName, decimal MachineStake)

        {
            ID = 0;
            Name = MachineName;
            stake = MachineStake;
            Fields = new List<SelectableField>();
            Results = new HashSet<TestResult>();
            for (int i = 1; i <= 63; i++)
            {
                SelectableField field = new SelectableField();

                switch (i)
                {
                    case int n when (n <= 30):
                        field.ID = i;
                        field.Color = "Black";
                        field.stakeMultiplier = 2;

                        break;

                    case int n when (n >= 31 && n <= 60):
                        field.ID = i;
                        field.Color = "Red";
                        field.stakeMultiplier = 2;

                        break;

                    case int n when (n > 60):
                        field.ID = i;
                        field.Color = "White";
                        field.stakeMultiplier = 0.1M;

                        break;
                }

                Fields.Add(field);
            }
        }

        internal class SelectableField
        {
            public int ID { get; set; }
            public string Color { get; set; }
            public decimal stakeMultiplier { get; set; }
        }

        public void RunSingleTest()
        {
            Random randomizer = new Random();

            string selectedColour = randomizer.Next(1, 2) == 1 ? "Black" : "Red";

            int randomResult = randomizer.Next(1, Fields.Count());

            decimal runprofit = stake;

            SelectableField resultField = Fields.Where(x => x.ID == randomResult).First();

            if (resultField.Color == selectedColour || resultField.Color == "White")
            {
                runprofit -= (stake * resultField.stakeMultiplier);
            }

            Results.Add(new TestResult
            {
                testNum = Results.Count == 0 ? 1 : Results.Max(x => x.testNum) + 1,
                profit = runprofit
            });
        }

        public void ClearTestResults()
        {
            Results.Clear();
        }
    }

    internal class TestResult
    {
        public int testNum { get; set; }
        public decimal profit { get; set; }
    }
}