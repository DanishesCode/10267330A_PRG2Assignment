using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
    {
        internal abstract class Flight
        {
            protected const double ARRIVING_FLIGHT_FEE = 500.0;
            protected const double DEPARTING_FLIGHT_FEE = 800.0;
            protected const double BOARDING_GATE_BASE_FEE = 300.0;

            public string FlightNumber { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public DateTime ExpectedTime { get; set; }
            public string Status { get; set; }
            public Flight(string fN, string o, string d, DateTime eT, string s)
            {
                FlightNumber = fN;
                Origin = o;
                Destination = d;
                ExpectedTime = eT;
                Status = s;
            }

            public abstract double CalculateFees();
            public override string ToString()
            {
                return $"Flight Number: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, " +
                       $"Expected Time: {ExpectedTime}, Status: {Status}";
            }
        }

    }
}
}
