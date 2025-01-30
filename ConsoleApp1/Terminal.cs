using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//==========================================================
// Student Number	: S10267330A
// Student Name	: Danish
// Partner Name	: TzeWei
//==========================================================

namespace ConsoleApp1
{
    internal class Terminal
    {
        private string terminalName;
        public string TerminalName
        {
            get => terminalName; set => terminalName = value;
        }
        private Dictionary<string, Airline> airlines;
        public Dictionary<string, Airline> Airlines
        {
            get => airlines; set => airlines = value;
        }
        private Dictionary<string, Flight> flights;
        public Dictionary<string, Flight> Flights
        {
            get => flights; set => flights = value;
        }
        private Dictionary<string, BoardingGate> boardingGates;
        public Dictionary<string, BoardingGate> BoardingGates
        {
            get => boardingGates; set => boardingGates = value;
        }
        private Dictionary<string, double> gateFees;
        public Dictionary<string, double> GateFees
        {
            get => gateFees; set => gateFees = value;
        }

        public Terminal(string name, Dictionary<string, Airline> a, Dictionary<string, Flight> f, Dictionary<string, BoardingGate> bg, Dictionary<string, double> gf)
        {
            terminalName = name;
            airlines = a;
            flights = f;
            boardingGates = bg;
            gateFees = gf;
        }

        public bool AddAirline(Airline add) {
            airlines.Add(add.Name, add);
            return true;
        }
        public bool AddBoardingGate(BoardingGate add) {
            boardingGates.Add(add.GateName, add);
            return true;
        }
        public Airline GetAirlineFromFlight(Flight selected)
        {
            foreach (var (key, value) in airlines)
            {
                var dictionary = value.Flights;
                foreach (var (x, v) in dictionary)
                {
                    if (v == selected)
                    {
                        return value;
                    }
                }
            }
            return null;
        }
        public void PrintAirlineFees()
        {
            foreach(var (key,value) in airlines)
            {
                Console.WriteLine(Convert.ToString($"Flight {value.Name} fees are ${value.CalculateFees()}"));
            }
        }
        public override string ToString()
        {
            return $"This is terminal {terminalName}";
        }
    } 
}
