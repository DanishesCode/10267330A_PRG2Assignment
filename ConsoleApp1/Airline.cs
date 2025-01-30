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
            double discounts = 0;
            double totalFees = 0;
            if (flights.Count >= 3)
            {
                int batch = (int)Math.Floor((double)flights.Count / 3);
                discounts = batch * 350;
            }
            foreach (var flight in flights.Values) {
                DateTime expectedTime = flight.ExpectedTime;
                if (expectedTime < DateTime.Parse("11:00 AM") || expectedTime > DateTime.Parse ("9:00 PM"))
                {
                    discounts += 110;
                }
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    discounts += 25;
                }
                if (flight.GetType().Name.Contains("DDJB") || flight.GetType().Name.Contains("CFFT") || flight.GetType().Name.Contains("LWTT"))
                {
                    // No discount for these specific flights
                }
                else
                {
                    discounts += 50;
                }
                totalFees += Convert.ToDouble(flight.CalculateFees());
            }
            if (flights.Count > 5)
            {
                discounts += totalFees * 0.03;
            }
            return discounts;

        }

    }
}
