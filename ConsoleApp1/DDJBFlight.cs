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
    internal class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight(string fN, string o, string d, DateTime eT, string s, double rF) : base(fN, o, d, eT, s)
        {
            RequestFee = rF;
        }
        public override double CalculateFees()
        {
            double totalFee = 0.0;

            // Determine if flight is arriving or departing
            if (Destination == "Singapore (SIN)") totalFee += ARRIVING_FLIGHT_FEE;
            if (Origin == "Singapore (SIN)") totalFee += DEPARTING_FLIGHT_FEE;

            // Add boarding gate base fee and special request fee
            totalFee += BOARDING_GATE_BASE_FEE;
            totalFee += RequestFee;
            return totalFee;
        }
        public override string ToString()
        {
            return base.ToString() + $", Request Fee: {RequestFee:F2}";
        }
    }
}
