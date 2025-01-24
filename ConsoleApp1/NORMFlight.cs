using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class NORMFlight : Flight
    {
        public NORMFlight(string fN, string o, string d, DateTime eT, string s) : base(fN, o, d, eT, s) { }
        public override double CalculateFees()
        {
            double totalFee = 0.0;

            // Determine if flight is arriving or departing
            if (Destination == "SIN") totalFee += ARRIVING_FLIGHT_FEE;
            if (Origin == "SIN") totalFee += DEPARTING_FLIGHT_FEE;

            // Add boarding gate base fee
            totalFee += BOARDING_GATE_BASE_FEE;

            return totalFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
