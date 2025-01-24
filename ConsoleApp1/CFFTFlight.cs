using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class CFFTFlight :Flight
    {
        public double RequestFee { get; set; }
        public CFFTFlight(string fN, string o, string d, DateTime eT, string s, double rF) : base(fN, o, d, eT, s)
        {
            RequestFee = rF;
        }
        public override double CalculateFees()
        {
            double totalFee = 0.0;

            // Determine if flight is arriving or departing
            if (Destination == "SIN") totalFee += ARRIVING_FLIGHT_FEE;
            if (Origin == "SIN") totalFee += DEPARTING_FLIGHT_FEE;

            // Add boarding gate base fee and special request fee
            totalFee += BOARDING_GATE_BASE_FEE;

            return totalFee;
        }
        public override string ToString()
        {
            return base.ToString() + $", Request Fee: {RequestFee:F2}";
        }
    }
}
