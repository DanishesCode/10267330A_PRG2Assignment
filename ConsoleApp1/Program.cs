//Danish does question 1,4,7,8
//Tze wei does question 2,3,5,6,9
using ConsoleApp1;
using System.Globalization;
string[] dataAirline = File.ReadAllLines("airlines.csv");
string[] dataBoarding = File.ReadAllLines("boardinggates.csv");
string[] csvlines = File.ReadAllLines("flights.csv");
Dictionary<string,Airline> airlineDictionary = new Dictionary<string,Airline>();
Dictionary<string,BoardingGate> boardingGateDictionary = new Dictionary<string,BoardingGate>();
Dictionary<string,Flight> flightsDictionary = new Dictionary<string, Flight>();
for (int i=1; i<csvlines.Length; i++)//Creating the flight objects from the loaded files//
{
    string[] values = csvlines[i].Split(',');
    DateTime expectedTime = DateTime.Parse(values[3].Trim());

    Flight flight;

    if (values[4].Trim() == "CFFT")
    {
        flight = new CFFTFlight(values[0].Trim(), values[1].Trim(), values[2].Trim(), expectedTime, "On Time",150.0);
    }
    else if (values[4].Trim() == "DDJB")
    {
        flight = new DDJBFlight(values[0].Trim(), values[1].Trim(), values[2].Trim(), expectedTime, "On Time", 300.0);
    }
    else if (values[4].Trim() == "LWTT")
    {
        flight = new LWTTFlight(values[0].Trim(), values[1].Trim(), values[2].Trim(), expectedTime, "On Time", 500.0);
    }
    else
    {
        flight = new NORMFlight(values[0].Trim(), values[1].Trim(), values[2].Trim(), expectedTime, "On Time");
    }

    flightsDictionary.Add(flight.FlightNumber, flight);
}

for (int i = 1; i < dataAirline.Length; i++) {//Created the airlines from the loaded files into a dictionary//
    string[] currentData = dataAirline[i].Split(",");
    Dictionary<string,Flight> currentAirlineFlights = new Dictionary<string,Flight>();
    foreach(var x in flightsDictionary){
        if(x.Key.Substring(0,2) == currentData[1])
        {
            currentAirlineFlights.Add(x.Value.FlightNumber, x.Value);
        }
}
    Airline selectedAirline = new Airline(currentData[0], currentData[1],currentAirlineFlights);
    airlineDictionary.Add(currentData[1], selectedAirline);
}
string[] flightsLeft = csvlines;
for (int i = 1; i < dataBoarding.Length; i++) { //Created the boardingGates objects from files//
    string[] currentData = dataBoarding[i].Split(",");
    BoardingGate toAdd = new BoardingGate(currentData[0], Convert.ToBoolean(currentData[2]), Convert.ToBoolean(currentData[1]), Convert.ToBoolean(currentData[3]),null);
    boardingGateDictionary.Add(currentData[0], toAdd);
}
for (int i = 1; i < flightsLeft.Length; i++) { //Sorting the boarding gates for the flights//
    string[] currentData = flightsLeft[i].Split(",");
    Flight flightObj = flightsDictionary[currentData[0]];
    string specialCode;
    if (currentData[4] != null)
    {
        specialCode = currentData[4];
        if(specialCode == "DDJB")
        {
            foreach (var x in boardingGateDictionary)
            {
                BoardingGate selected = x.Value;
                if (selected.SupportsDDJB == true && selected.Flight == null)
                {
                    selected.Flight = flightObj;
                    break;
                }
            }

        }
        else if(specialCode == "CFFT")
        {
            foreach (var x in boardingGateDictionary)
            {
                BoardingGate selected = x.Value;
                if (selected.SupportsCFFT == true && selected.Flight == null)
                {
                    selected.Flight = flightObj;
                    break;
                }
            }
        }
        else//supports LWTT//
        {
            foreach (var x in boardingGateDictionary)
            {
                BoardingGate selected = x.Value;
                if ( selected.SupportsLWTT == true && selected.Flight == null)
                {
                    selected.Flight = flightObj;
                    break;
                }
            }
        }
    }
    else
    {
        foreach(var x in boardingGateDictionary)
        {
            BoardingGate selected = x.Value;
            if(selected.SupportsDDJB == false && selected.SupportsCFFT == false && selected.SupportsLWTT == false && selected.Flight == null)
            {
                selected.Flight = flightObj;
                break;
            }
        }
    }
}
DisplayBasicInfo(flightsDictionary);
void DisplayBasicInfo(Dictionary<string, Flight> flights)
{
    foreach (var flight in flights.Values)
    {
        string airlineName = flight.FlightNumber.Split(' ')[0];
        Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline Name: {airlineName}, Origin: {flight.Origin}, Destination: {flight.Destination}, Expected Time: {flight.ExpectedTime}");
    }
}

void DisplayBoardingGatesInfo(Dictionary<string,BoardingGate> boardingGateDictionary)//feature 4//
{
    Console.WriteLine("=================================================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=================================================================");
    Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20}", "Gate Name", "DDJB", "CFFT", "LWTT");
    foreach (var x in boardingGateDictionary)
    {
        BoardingGate obj = x.Value;
        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20}", obj.GateName, Convert.ToString(obj.SupportsDDJB), Convert.ToString(obj.SupportsCFFT), Convert.ToString(obj.SupportsLWTT));

    }
}
DisplayBoardingGatesInfo(boardingGateDictionary);

void listAirlineAvail()//feature 7
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15} {1,-30}", "Airline Code", "Airline Name");
    Airline selected = null;
    foreach (var x in airlineDictionary)
    {
        Console.WriteLine("{0,-15} {1,-30}", x.Key, x.Value.Name);
    }
    while (selected == null)
    {
        Console.Write("Enter Airline Code: ");
        string code = Console.ReadLine().ToUpper();
        if (code.Length != 2)
        {
            Console.WriteLine("Please input only 2 letters.Try again!");
        }
        else
        {
            try
            {
                selected = airlineDictionary[code];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("That airlines does not exist. Try again!");
            }
            
        }
    }
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Singapore Airlines");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-25} {4,-30}",
                      "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

    foreach(var x in selected.Flights)
    {
        Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-25} {4,-30}", x.Key, selected.Name, x.Value.Origin, x.Value.Destination, Convert.ToString(x.Value.ExpectedTime));
    }

    Flight objSelected = null;

    while (objSelected == null)
    {
        try
        {
            Console.Write("Choose a flight: ");
            string chosen = Console.ReadLine().ToUpper();
            objSelected = selected.Flights[chosen];
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Flight does not exist!");
        }
    }
    Console.WriteLine($"Flight Number: {objSelected.FlightNumber}, Airline Name: {selected.Name}, Origin: {objSelected.Origin},  Destination: {objSelected.Destination}, Expected Departure/Arrival Time: {Convert.ToString(objSelected.ExpectedTime)}");
}
listAirlineAvail();