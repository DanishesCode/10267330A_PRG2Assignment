using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Airline
    {
        private string name;
        public string Name
        {
            get=>name;set=>name=value;
        }
        private string code;
        public string Code
        {
            get=>code;set=>code=value;
        }
        private Dictionary<string, Flight> flights;
        public Dictionary<string,Flight> Flights
        {
            get => flights;set => flights=value;
        }

        public bool AddFlight(Flight toAdd)
        {
            if (toAdd == null) {
                return false;
            }
            else
            {   
                flights.Add(toAdd.FlightNumber, toAdd);
                return true;
            }
        }

        public Airline(string n,string c,Dictionary<string,Flight> dict)
        {
            name = n;
            code = c;
            flights = dict;
        }

        public double CalculateFees() {
            double discount = 0;
            double total = 0;
            if (flights.Count >= 3)
            {
                int batch = (int)Math.Floor((double)flights.Count / 3);
                discount = batch * 350;
            }
            foreach (var (key, value) in flights) {
                DateTime expectedTime = value.ExpectedTime;
                if (expectedTime < Convert.ToDateTime("11:00") || expectedTime > Convert.ToDateTime("21:00"))
                {
                    discount += 110;
                }
                if (value.Origin == "DXB" || value.Origin == "BKK" || value.Origin == "NRT")
                {
                    discount += 25;
                }
                if (value.FlightNumber == "SQ 693" || value.FlightNumber == "CX 312" || value.FlightNumber == "QF 981")
                {
                }
                else
                {
                    discount += 50;
                }
                total += Convert.ToDouble(value.CalculateFees());
            }
            if (flights.Count > 5)
            {
                return( total * 0.97)-discount;
            }
            else
            {
                return total-discount;
            }

        }

    }
}
