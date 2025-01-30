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
    internal class BoardingGate
    {
        private string gateName;
        public string GateName
        {
            get => gateName; set => gateName = value;
        }
        private bool supportsCFFT;
        public bool SupportsCFFT
        {
            get => supportsCFFT; set => supportsCFFT = value;
        }
        private bool supportsDDJB;
        public bool SupportsDDJB
        {
            get=> supportsDDJB; set => supportsDDJB = value;
        }
        private bool supportsLWTT;
        public bool SupportsLWTT
        {
            get=> supportsLWTT; set => supportsLWTT = value;
        }
        private Flight flight;
        public Flight Flight
        {
            get=> flight; set => flight = value;
        }

        public BoardingGate(string gName,bool CFFT,bool DDJB,bool LWTT,Flight f)
        {
            gateName = gName;
            supportsCFFT = CFFT;
            supportsDDJB = DDJB;
            supportsLWTT = LWTT;
            flight = f;
        }

        public double CalculateFees() {
            double baseFee = 300;
            double calculated = flight.CalculateFees();
            if (supportsDDJB) {
                return baseFee + 300 + calculated;
            } else if (supportsCFFT) {
                return baseFee + 150 + calculated;
            }else if (supportsLWTT)
            {
                return baseFee + 500 + calculated;
            }
            else
            {
                return baseFee + calculated;
            }
        }
        public override string ToString() {
            return ($"GateName:{gateName} SupportCFFT:{supportsCFFT} SupportsDDJB:{supportsDDJB} SupportLWTT{SupportsDDJB} flight: {flight}");
        }
    }
}
