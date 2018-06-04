using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingOddsTester.Models;
using System.Windows;

namespace BettingOddsTester
{
    internal class Program
    {
        private static List<IMachine> BettingMachines = new List<IMachine>();

        private static ConsoleColor defaultColor = Console.ForegroundColor;

        private static void Main(string[] args)
        {
            SeedBettingMachines();

            Console.WriteLine(BettingMachines.Count() + " betting machines found");

            foreach (IMachine machine in BettingMachines)
            {
                Console.WriteLine(String.Format("Press {0} to run tests for {1}", machine.ID, machine.Name));
            }

            Console.WriteLine("Press Q to quit application");

            int ID = 0;

            while (ID == 0)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey();

                if (pressedKey.KeyChar.ToString().ToUpper() == "Q") { Environment.Exit(0); }
                Console.WriteLine();
                if (int.TryParse(pressedKey.KeyChar.ToString(), out ID))
                {
                    bool MachineExists = BettingMachines.Where(x => x.ID == ID).Count() > 0;

                    if (!MachineExists)
                    {
                        Console.WriteLine("Wrong key pressed. Please try again");
                        ID = 0;
                        continue;
                    }
                    TestMachine(ID);
                }
                else
                {
                    Console.WriteLine("Wrong key pressed. Please try again");
                }
            }

            Console.WriteLine("All tests finished, press any key to close application");
            Console.ReadKey(true);
        }

        private static void TestMachine(int id)
        {
            Console.WriteLine();
            IMachine machine = BettingMachines.Where(x => x.ID == id).First();

            Console.WriteLine("Testing machine " + machine.Name);

            bool finishTests = false;

            while (!finishTests)
            {
                Console.WriteLine("Please specify number of tests you want to run on this machine");

                int NumberOfTests = 0;

                while (NumberOfTests == 0)
                {
                    string consoleText = Console.ReadLine();

                    if (int.TryParse(consoleText, out NumberOfTests))
                    {
                        NumberOfTests = Math.Abs(Convert.ToInt32(consoleText));
                    }
                    else
                    {
                        Console.WriteLine("Value must be numeric. Try again");
                    }
                }

                machine.ClearTestResults();

                for (int i = 1; i <= NumberOfTests; i++)
                {
                    machine.RunSingleTest();
                    Console.ForegroundColor = i == NumberOfTests ? ConsoleColor.Green : ConsoleColor.Yellow;
                    Console.WriteLine(machine.ResultString);
                }

                Console.ForegroundColor = defaultColor;
                Console.WriteLine("Test Finished. Would you like to run it again ? Y/N");

                ConsoleKeyInfo YN = Console.ReadKey(true);

                if (YN.KeyChar.ToString().ToUpper() != "Y")
                {
                    finishTests = true;
                }
            }
        }

        private static void SeedBettingMachines()
        {
            BettingMachines.Add(new RouletteType("Black or Red Party", 1));
            BettingMachines.Add(new DiceType("Dice Roll 2000", 1));

            int ID = 1;

            foreach (IMachine machine in BettingMachines)
            {
                machine.ID = ID;
                ID++;
            }
        }
    }
}