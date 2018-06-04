using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingOddsTester.Models
{
    internal class DiceType : IMachine
    {
        // Interface implemented
        public int ID { get; set; }

        public string Name { get; set; }
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

        public List<Dice> Dices { get; }

        public DiceType(string MachineName, decimal MachineStake)
        {
            ID = 0;
            Name = MachineName;
            stake = MachineStake;

            Dice dice = new Dice { Values = new List<int> { 1, 2, 3, 4, 5, 6 } };

            Dices = new List<Dice>();

            Results = new HashSet<TestResult>();

            Dices.AddRange(new List<Dice>() { dice, dice });
        }

        internal class Dice
        {
            public List<int> Values { get; set; }
        }

        public void ClearTestResults()
        {
            Results.Clear();
        }

        public void RunSingleTest()
        {
            decimal runprofit = stake;

            Random randomizer = new Random();

            int[] diceRolls = { randomizer.Next(1, Dices[0].Values.Count()), randomizer.Next(1, Dices[1].Values.Count()) };

            if (diceRolls[0] == diceRolls[1])
            {
                runprofit -= diceRolls[0] * (stake * 1.5M);
            }

            Results.Add(new TestResult
            {
                testNum = Results.Count == 0 ? 1 : Results.Max(x => x.testNum) + 1,
                profit = runprofit
            });
        }
    }
}