// See https://aka.ms/new-console-template for more information
using ConsoleApp1;
using System.Globalization;
string[] csvlines = File.ReadAllLines("flights.csv");
Dictionary<string,Flight> flightsDictionary = new Dictionary<string, Flight>();
for (int i=1; i<csvlines.Length; i++)
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

DisplayBasicInfo(flightsDictionary);
void DisplayBasicInfo(Dictionary<string, Flight> flights)
{
    foreach (var flight in flights.Values)
    {
        string airlineName = flight.FlightNumber.Split(' ')[0];
        Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline Name: {airlineName}, Origin: {flight.Origin}, Destination: {flight.Destination}, Expected Time: {flight.ExpectedTime}");
    }
}

