﻿//Danish does question 1,4,7,8
//Tze wei does question 2,3,5,6,9

//==========================================================
// Student Number	: S10267330A
// Student Name	: Danish
// Partner Name	: TzeWei
//==========================================================

using ConsoleApp1;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;
string[] dataAirline = File.ReadAllLines("airlines.csv");
string[] dataBoarding = File.ReadAllLines("boardinggates.csv");
string[] csvlines = File.ReadAllLines("flights.csv");
string[] airlineMap = File.ReadAllLines("airlines.csv");
Dictionary<string,Airline> airlineDictionary = new Dictionary<string,Airline>();
Dictionary<string,BoardingGate> boardingGateDictionary = new Dictionary<string,BoardingGate>();
Dictionary<string,Flight> flightsDictionary = new Dictionary<string, Flight>();
Dictionary<string, string> airlineMapDictionary = new Dictionary<string, string>();
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
foreach (var line in airlineMap)
{
    string[] values = line.Split(',');

    // Trim the values to remove any extra spaces
    string airlineName = values[0].Trim(); // Full airline name
    string airlineCode = values[1].Trim(); // Airline code

    // Store the airline code and name in the dictionary
    if (!airlineMapDictionary.ContainsKey(airlineCode))
    {
        airlineMapDictionary.Add(airlineCode, airlineName);
    }
}

    for (int i = 1; i < dataAirline.Length; i++)
    {//Created the airlines from the loaded files into a dictionary//
        string[] currentData = dataAirline[i].Split(",");
        Dictionary<string, Flight> currentAirlineFlights = new Dictionary<string, Flight>();
        foreach (var x in flightsDictionary)
        {
            if (x.Key.Substring(0, 2) == currentData[1])
            {
                currentAirlineFlights.Add(x.Value.FlightNumber, x.Value);
            }
        }
        Airline selectedAirline = new Airline(currentData[0], currentData[1], currentAirlineFlights);
        airlineDictionary.Add(currentData[1], selectedAirline);
    }
    
    for (int i = 1; i < dataBoarding.Length; i++)
    { //Created the boardingGates objects from files//
        string[] currentData = dataBoarding[i].Split(",");
        BoardingGate toAdd = new BoardingGate(currentData[0], Convert.ToBoolean(currentData[2]), Convert.ToBoolean(currentData[1]), Convert.ToBoolean(currentData[3]), null);
        boardingGateDictionary.Add(currentData[0], toAdd);
    }
    

    void DisplayBasicInfo(Dictionary<string, Flight> flights)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Flights for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        Console.WriteLine("--------------------------------------------------------------------------------------------");

        foreach (var flight in flights.Values)
        {
            string airlineCode = flight.FlightNumber.Split(' ')[0];
            string airlineName = "";
            if (airlineMapDictionary.ContainsKey(airlineCode))
            {
                airlineName = airlineMapDictionary[airlineCode];
            }
            else
            {
                airlineName = "Unknown Airline"; 
            }
        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-25}",
                      flight.FlightNumber,
                      airlineName,
                      flight.Origin,
                      flight.Destination,
                      flight.ExpectedTime);
    }
}
    

    void DisplayBoardingGatesInfo(Dictionary<string, BoardingGate> boardingGateDictionary)//feature 4//
    {
        Console.WriteLine("=================================================================");
        Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
        Console.WriteLine("=================================================================");
        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-20}", "Gate Name", "DDJB", "CFFT", "LWTT","Flight Number");
        foreach (var x in boardingGateDictionary)
        {
            BoardingGate obj = x.Value;
            if(obj.Flight == null)
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-20}", obj.GateName, Convert.ToString(obj.SupportsDDJB), Convert.ToString(obj.SupportsCFFT), Convert.ToString(obj.SupportsLWTT), "N/A");
        }
        else
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-20}", obj.GateName, Convert.ToString(obj.SupportsDDJB), Convert.ToString(obj.SupportsCFFT), Convert.ToString(obj.SupportsLWTT), Convert.ToString(obj.Flight.FlightNumber));

        }

    }
    }


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
        Console.WriteLine($"List of Flights for {selected.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-25} {4,-30}",
                          "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (var x in selected.Flights)
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
    if (objSelected.GetType() == typeof(LWTTFlight)) {
        Console.WriteLine("Special Request Code: LWTT");
    }else if (objSelected.GetType() == typeof(CFFTFlight))
    {
        Console.WriteLine("Special Request Code: CFFT");

    }
    else if (objSelected.GetType() == typeof(DDJBFlight))
    {
        Console.WriteLine("Special Request Code: DDJB");

    }
    else
    {
        Console.WriteLine("Special Request Code: N/A");

    }
    foreach(var x in boardingGateDictionary)
    {
        if(x.Value.Flight == objSelected)
        {
            BoardingGate gate = x.Value;
            Console.WriteLine($"{objSelected.FlightNumber} is assigned to boarding gate {gate.GateName}");
        }
    }

}

    void AddNewFlights(Dictionary<string, Flight> flights)
    {
        bool addMoreFlights = true;

    while (addMoreFlights)
    {
        string flightNumber;
        while (true)
        {
            Console.WriteLine("Enter Flight Number:");
            flightNumber = Console.ReadLine().Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(flightNumber))
            {
                Console.WriteLine("❌ Flight number cannot be empty.");
                continue;
            }

            if (!Regex.IsMatch(flightNumber, @"^[A-Z]{2}\s?\d{1,4}$"))
            {
                Console.WriteLine("Invalid format. Flight number must be two letters followed by 1–4 digits (e.g., SQ115, MH298).");
                continue;
            }

            if (flights.ContainsKey(flightNumber))
            {
                Console.WriteLine($"Flight number {flightNumber} already exists. Please enter a unique flight number.");
                continue;
            }
            break;
        }
        string origin;
        while (true)
        {
            Console.WriteLine("Enter Origin:");
            origin = Console.ReadLine().Trim();
            if (!Regex.IsMatch(origin, @"^[A-Za-z\s]+ \([A-Z]{3}\)$"))
            {
                Console.WriteLine("Invalid format. Enter in 'City (IATA)' format (e.g., Singapore (SIN)).");
                continue;
            }
            break;
        }
        string destination;
        while (true)
        {
            Console.WriteLine("Enter Destination:");
            destination = Console.ReadLine().Trim();
            if (!Regex.IsMatch(destination, @"^[A-Za-z\s]+ \([A-Z]{3}\)$"))
            {
                Console.WriteLine("Invalid format. Enter in 'City (IATA)' format (e.g., Tokyo (NRT)).");
                continue;
            }
            break;
        }
        DateTime expectedTime;
        while (true)
        {
            Console.WriteLine("Enter Expected Departure/Arrival Time (dd/MM/yyyy hh:mm tt):");
            string input = Console.ReadLine().Trim();

            try
            {
                expectedTime = DateTime.Parse(input);
                Console.WriteLine($"Parsed time: {expectedTime}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid date format. Please enter the date and time in the correct format.");
                continue;
            }
            break;
        }
        string specialRequestCode;
        while (true)
        {
            Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None):");
            specialRequestCode = Console.ReadLine().Trim().ToUpper();
            if (!new[] { "CFFT", "DDJB", "LWTT", "NONE" }.Contains(specialRequestCode))
            {
                Console.WriteLine("Invalid special request code. Enter CFFT, DDJB, LWTT, or None.");
                continue;
            }
            break;
        }


        Flight flight;
            if (specialRequestCode == "CFFT")
            {
                flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, "On Time", 150.0);
            }
            else if (specialRequestCode == "DDJB")
            {
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, "On Time", 300.0);
            }
            else if (specialRequestCode == "LWTT")
            {
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, "On Time", 500.0);
            }
            else
            {
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime, "On Time");
                specialRequestCode = "";
            }

            flights.Add(flight.FlightNumber, flight);

            try
            {
            using (StreamWriter sw = new StreamWriter("flights.csv", append: true))
            {
                sw.WriteLine($"{flight.FlightNumber},{flight.Origin},{flight.Destination},{expectedTime:hh:mm tt},{specialRequestCode}");
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error appending to file: {ex.Message}");
            }

            Console.WriteLine($"Flight {flight.FlightNumber} has been added!");

            Console.WriteLine("Would you like to add another flight? (Y/N)");
            string addAnotherFlightResponse = Console.ReadLine().Trim().ToUpper();
            addMoreFlights = addAnotherFlightResponse == "Y";
        }

        Console.WriteLine("Flight(s) have been successfully added.");
    }
    void AssignBoardingGate()
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Assign a Boarding Gate to a Flight");
        Console.WriteLine("=============================================");


        Flight selectedFlight = GetFlight();
        if (selectedFlight == null)
        {
            Console.WriteLine("Operation cancelled. No flight selected.");
            return;
    }


        BoardingGate selectedGate = GetBoardingGate();
        if (selectedGate == null)
        {
            Console.WriteLine("Operation cancelled. No boarding gate selected.");
            return;
        }


        selectedGate.Flight = selectedFlight;


        Console.WriteLine();
        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
        Console.WriteLine($"Special Request Code: {GetSpecialRequestCode(selectedFlight)}");
        Console.WriteLine($"Boarding Gate Name: {selectedGate.GateName}");
        Console.WriteLine($"Supports DDJB: {selectedGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {selectedGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {selectedGate.SupportsLWTT}");


        if (PromptYesNo("Would you like to update the status of the flight? (Y/N): "))
        {
            UpdateFlightStatus(selectedFlight);
        }

        Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been successfully assigned to Boarding Gate {selectedGate.GateName}!");
    }
    Flight GetFlight()
    {
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine().ToUpper();
            if (flightsDictionary.ContainsKey(flightNumber))
            {
                return flightsDictionary[flightNumber];
            }

            Console.WriteLine("Flight not found. Try again? (Y/N): ");
            if (!PromptYesNo("")) return null; //
        }
    }

    BoardingGate GetBoardingGate()
    {
        while (true)
        {
            Console.Write("Enter Boarding Gate Name: ");
            string gateName = Console.ReadLine().ToUpper();


            if (boardingGateDictionary.ContainsKey(gateName))
            {
                BoardingGate gate = boardingGateDictionary[gateName];
                if (gate.Flight != null)
                {
                    Console.WriteLine($"Gate {gateName} is already assigned to flight {gate.Flight.FlightNumber}. Try another gate.");
                    continue;
                }
            else
            {
                return gate;
            }
            
        }
            else
            {
                Console.WriteLine("Boarding Gate not found. Try again? (Y/N): ");
                if (!PromptYesNo("")) return null;
            }
        }
    }
void UpdateFlightStatus(Flight flight)
    {
        Console.WriteLine("1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        Console.Write("Please select the new status of the flight: ");
        string input = Console.ReadLine();

        if (input == "1") flight.Status = "Delayed";
        else if (input == "2") flight.Status = "Boarding";
        else if (input == "3") flight.Status = "On Time";
        else
        {
            Console.WriteLine("Invalid option. Status set to default 'On Time'.");
            flight.Status = "On Time";
        }
    }

    string GetSpecialRequestCode(Flight flight)
    {
        string flightType = flight.GetType().Name;
        if (flightType.Contains("DDJB") || flightType.Contains("CFFT") || flightType.Contains("LWTT"))
        {
            return flightType.Replace("Flight", "");
        }
        return "None";
    }

    bool PromptYesNo(string message)
    {
    while (true)
    {
        Console.Write(message);
        string response = Console.ReadLine().ToUpper();
        if (response == "Y") return true;
        if (response == "N") return false;
        Console.WriteLine(" Invalid Response. Please enter 'Y' for Yes or 'N' for No.");
    }
    }
    BoardingGate UpdateGetBoardingGate(Flight flight)
{
    // Here you could implement logic to fetch boarding gate for a flight, 
    // if it's assigned to a gate from your boardingGateDictionary
    foreach (var gate in boardingGateDictionary.Values)
    {
        if (gate.Flight == flight)
        {
            return gate;
        }
    }
    return null;
}
void DisplaySortedFlights(Dictionary<string, Flight> flights, Dictionary<string, string> airlineMapDictionary) //Feature 9
{
    List<Flight> sortedFlights = flights.Values.ToList();
    sortedFlights.Sort(); 
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("{0,-10} {1,-20} {2,-20} {3,-20} {4,-15} {5,-20} {6,-10}",
        "Flight", "Airline Name", "Origin", "Destination", "Boarding Gate", "Expected", "Status");
    Console.WriteLine(new string('-', 115));

    foreach (Flight flight in sortedFlights)
    {
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        if (!airlineMapDictionary.ContainsKey(airlineCode))
        {
            continue; 
        }

        string airlineName = airlineMapDictionary[airlineCode];
        BoardingGate gate = UpdateGetBoardingGate(flight);
        string boardingGate = gate != null ? gate.GateName : "Unassigned";
        flight.Status = "Scheduled";
        
        Console.WriteLine("{0,-10} {1,-20} {2,-20} {3,-20} {4,-15} {5,-20} {6,-10}",
            flight.FlightNumber,
            airlineName,
            flight.Origin,
            flight.Destination,
            boardingGate,
            flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"), 
            flight.Status);
    }
}


void modifyFlightDetail()//feature 8
    {
        Airline selectedAirline;
        Flight selectedFlight;
        Console.WriteLine("=============================================");
        Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("{0,-15} {1,-30}", "Airline Code", "Airline Name");
        Airline selected = null;
        foreach (var x in airlineDictionary)
        {
            Console.WriteLine("{0,-15} {1,-30}", x.Key, x.Value.Name);
        }
    while (true)
    {
        Console.Write("Enter Airline Code: ");
        string chosen1 = Console.ReadLine().ToUpper();
        
        if (chosen1.Length != 2)
        {
            Console.WriteLine("Airline Code Must be 2 Words!");
        }
        else
        {
            try
            {
                selectedAirline = airlineDictionary[chosen1];
                break;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Airline does not exist! Try again");
                continue;
            }
        }
        
    }
    Console.WriteLine($"List of Flight for {selectedAirline.Name}");
    Console.WriteLine("{0,-15} {1,-20} {2,-25} {3,-25} {4,-30}",
                             "Flight Number","Airline Name","Origin","Destination","Expected Departure/Arrival Time");

    foreach(var x in selectedAirline.Flights)
    {
        Flight f = x.Value;
        Console.WriteLine("{0,-15} {1,-20} {2,-25} {3,-25} {4,-30}",
                            f.FlightNumber,selectedAirline.Name,f.Origin,f.Destination,Convert.ToString(f.ExpectedTime));
    }
    while (true)
    {
        Console.Write("Choose an existing Flight to modify or delete: ");
        string chosen2 = Console.ReadLine().ToUpper();
        try
        {
            selectedFlight = selectedAirline.Flights[chosen2];
            break;
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Flight does not exist! Try again!");
        }
    }

    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");

    string chosen3;
    while (true)
    {
        Console.Write("Choose an option:");
        chosen3 = Console.ReadLine();
        if(chosen3 == "1" || chosen3 == "2")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid option try again!");
        }
    }
    if (chosen3 == "1") //modify flight
    {
        string chosen4;
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        while (true)
        {
            Console.WriteLine("Choose an option:");
            chosen4 = Console.ReadLine();
            if (chosen4 == "1" || chosen4 == "2" || chosen4 == "3" || chosen4 == "4")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option try again!");
            }
        }
        if (chosen4 == "1") // Change basic information
        {
            DateTime newTiming;
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();
            while (true)
            {
                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string newTime = Console.ReadLine();
                try
                {
                    newTiming = Convert.ToDateTime(newTime);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid DateTime try again!");
                }
            }
            selectedFlight.Origin = newOrigin;
            selectedFlight.Destination = newDestination;
            selectedFlight.ExpectedTime = newTiming;
            Console.WriteLine("Flight Updated!");
            Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
            Console.WriteLine($"Airline Name: {selectedAirline.Name}");
            Console.WriteLine($"Origin: {selectedFlight.Origin}");
            Console.WriteLine($"Desitnation: {selectedFlight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {Convert.ToString(selectedFlight.ExpectedTime)}");
            Console.WriteLine($"Status: {selectedFlight.Status}");



        }
        else if (chosen4 == "2") // Change Status
        {
            Console.Write("Enter the new status: ");
            string newStatus = Console.ReadLine();
            selectedFlight.Status = newStatus;
            Console.WriteLine($"{selectedFlight.FlightNumber} new status is {selectedFlight.Status}");
        }
        else if (chosen4 == "3")  //Change Special Request Code
        {
            Console.Write("Enter the new Special Request Code: ");
            while (true)
            {
                string newSpec = Console.ReadLine().ToUpper();
                if (newSpec == "NORM")
                {
                    Flight newFlight = new NORMFlight(selectedFlight.FlightNumber,selectedFlight.Origin,selectedFlight.Destination,selectedFlight.ExpectedTime,selectedFlight.Status);
                    selectedAirline.Flights.Remove(selectedFlight.FlightNumber);
                    selectedAirline.AddFlight(newFlight);
                    foreach(var x in boardingGateDictionary)
                    {
                        if(x.Value.Flight == selectedFlight)
                        {
                            x.Value.Flight = newFlight;
                        }
                    }

                   
                    break;
                }
                else if (newSpec == "LWTT")
                {
                    Flight newFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status,500.0);
                    selectedAirline.Flights.Remove(selectedFlight.FlightNumber);
                    selectedAirline.AddFlight(newFlight);
                    foreach (var x in boardingGateDictionary)
                    {
                        if (x.Value.Flight == selectedFlight)
                        {
                            x.Value.Flight = newFlight;
                        }
                    }
                    break;
                }
                else if (newSpec == "DDJB")
                {
                    Flight newFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status,300.0);
                    selectedAirline.Flights.Remove(selectedFlight.FlightNumber);
                    selectedAirline.AddFlight(newFlight);
                    foreach (var x in boardingGateDictionary)
                    {
                        if (x.Value.Flight == selectedFlight)
                        {
                            x.Value.Flight = newFlight;
                        }
                    }
                    break;
                }
                else if (newSpec == "CFFT")
                {
                    Flight newFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status,150.0);
                    selectedAirline.Flights.Remove(selectedFlight.FlightNumber);
                    selectedAirline.AddFlight(newFlight);
                    foreach (var x in boardingGateDictionary)
                    {
                        if (x.Value.Flight == selectedFlight)
                        {
                            x.Value.Flight = newFlight;
                        }
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid request code(NORM,LWTT,DDJB,CFFT)!");
                }
            }
            
            

        }
        else //Change Boarding gate
        {
            BoardingGate selectedBoardingGate;
            while (true)
            {
                Console.Write("Enter the new Boarding gate: ");
                string chosen5 = Console.ReadLine().ToUpper();
                if (boardingGateDictionary.ContainsKey(chosen5))
                {
                    selectedBoardingGate = boardingGateDictionary[chosen5];
                    if(selectedBoardingGate.Flight == null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("There is already an assigned Flight at this gate. Try another gate!");
                    }
                }
                else
                {
                    Console.WriteLine("Boarding gate does not exist! Try again!");
                }
            }
            selectedBoardingGate.Flight = selectedFlight;
            Console.WriteLine($"{selectedFlight.FlightNumber} new asssigned gate is {selectedBoardingGate.GateName}");

        }

    }
    else //delete flight
    {
        while (true)
        {
            Console.WriteLine("Are you sure you want to continue? [Y}Yes [N]No");
            string chosen6 = Console.ReadLine().ToUpper();
           if(chosen6 == "Y" || chosen6 =="N")
            {
                if (chosen6 == "Y")
                {
                    foreach (var x in selectedAirline.Flights)
                    {
                        if (x.Value == selectedFlight)
                        {
                            selectedAirline.Flights.Remove(x.Key);
                            break;
                        }
                    }
                    break;
                }
                else
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid choice!");

            }
        }
        
    }

}
//Advanced feature (a)
void processUnassignedFlights()
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    Queue<Flight> assignedFlights = new Queue<Flight>();
    List<BoardingGate> unassignedGates = new List<BoardingGate>();
    List<BoardingGate> assignedGates = new List<BoardingGate>();
    List<BoardingGate> autoAssignedGates = new List<BoardingGate>();

    foreach (var x in flightsDictionary)
    {
        bool haveGate = false;

        foreach (var y in boardingGateDictionary)
        {
            if (y.Value.Flight == x.Value)
            {
                haveGate = true;
                break;
            }
        }

        if (haveGate)
        {
            assignedFlights.Enqueue(x.Value);
        }
        else
        {
            unassignedFlights.Enqueue(x.Value);
        }
    }

    Console.WriteLine($"Total number of unassigned flights: {unassignedFlights.Count()}");

    foreach (var x in boardingGateDictionary)
    {
        if (x.Value.Flight == null)
        {
            unassignedGates.Add(x.Value);
            Console.WriteLine(x.Value.GateName);
        }
        else
        {
            assignedGates.Add(x.Value);
        }
    }

    Console.WriteLine($"Total number of unassigned gates: {unassignedGates.Count()}");

    List<Flight> remainingUnassignedFlights = new List<Flight>();
    while (unassignedFlights.Count > 0)
    {
        Flight flightSelected = unassignedFlights.Dequeue();
        bool assigned = false;

        foreach (BoardingGate y in unassignedGates.ToList())
        {
            if ((flightSelected.GetType() == typeof(CFFTFlight) && y.SupportsCFFT) ||
                (flightSelected.GetType() == typeof(LWTTFlight) && y.SupportsLWTT) ||
                (flightSelected.GetType() == typeof(DDJBFlight) && y.SupportsDDJB) ||
                (!y.SupportsCFFT && !y.SupportsDDJB && y.SupportsLWTT))
            {
                y.Flight = flightSelected;
                autoAssignedGates.Add(y);
                unassignedGates.Remove(y);
                assigned = true;
                break;
            }
        }

        if (!assigned)
        {
            remainingUnassignedFlights.Add(flightSelected);
        }
    }

    Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
        "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Special Request Code", "Boarding Gate");

    foreach (Flight x in assignedFlights)
    {
        string airlineName = null;
        string boardingGateName = null;

        foreach (var y in boardingGateDictionary)
        {
            if (y.Value.Flight == x)
            {
                boardingGateName = y.Key;
                break;
            }
        }

        foreach (var y in airlineDictionary)
        {
            if (y.Value.Flights.ContainsValue(x))
            {
                airlineName = y.Value.Name;
                break;
            }
        }

        if (x.GetType() == typeof(CFFTFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "CFFT", boardingGateName);
        }
        else if (x.GetType() == typeof(LWTTFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "LWTT", boardingGateName);
        }
        else if (x.GetType() == typeof(DDJBFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "DDJB", boardingGateName);
        }
        else
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "N/A", boardingGateName);
        }
    }

    foreach (BoardingGate y in autoAssignedGates)
    {
        Flight x = y.Flight;
        string airlineName = null;
        string boardingGateName = y.GateName;

        foreach (var z in airlineDictionary)
        {
            if (z.Value.Flights.ContainsValue(x))
            {
                airlineName = z.Value.Name;
                break;
            }
        }

        if (x.GetType() == typeof(CFFTFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "CFFT", boardingGateName);
        }
        else if (x.GetType() == typeof(LWTTFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "LWTT", boardingGateName);
        }
        else if (x.GetType() == typeof(DDJBFlight))
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "DDJB", boardingGateName);
        }
        else
        {
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-30} {5,-40} {6,-50}",
                x.FlightNumber, airlineName, x.Origin, x.Destination, Convert.ToString(x.ExpectedTime), "N/A", boardingGateName);
        }
    }

    Console.WriteLine($"Total number of flights and Boarding Gates Processed: {autoAssignedGates.Count()}");
    Console.WriteLine($"Total percentage of Flights and Boarding Gates that were processed automatically: {((double)autoAssignedGates.Count() / (autoAssignedGates.Count() + assignedGates.Count()) * 100):F2}%");
}

// Advanced Feature (b)
BoardingGate CheckBoardingGate(Flight flight) // check if boardinggate is available
{
    foreach (var gate in boardingGateDictionary.Values)
    {
        if (gate.Flight == flight)
        {
            return gate;
        }
    }
    Console.WriteLine($"Error: Flight {flight.FlightNumber} does not have a boarding gate assigned.");
    Console.WriteLine("Please assign a boarding gate before proceeding.\n");
    return null;
}
void DisplayTotalFees() { 
double totalFees = 0;
double totalDiscounts = 0;
foreach (var airlineEntry in airlineDictionary)
{
    var airline = airlineEntry.Value;
    Console.WriteLine($"Airline: {airline.Name} ({airline.Code})");

    double subtotalFees = 0;
    double subtotalDiscounts = 0;

   
    foreach (var flightEntry in flightsDictionary)
    {
        var flight = flightEntry.Value; 

        
        if (flight.FlightNumber.Substring(0, 2) == airline.Code) 
        {
            double flightFees = CalculateFlightFees(flight);
            subtotalFees += flightFees;
        }
    }

    // Apply discounts after calculating subtotal fees
    subtotalDiscounts = CalculateDiscounts(airline, subtotalFees);
    double totalAirlineFees = subtotalFees - subtotalDiscounts;
    
    Console.WriteLine($"Subtotal Fees: ${subtotalFees}");
    Console.WriteLine($"Subtotal Discounts: ${subtotalDiscounts}");
    Console.WriteLine($"Total Fees: ${totalAirlineFees}");

    totalFees += subtotalFees;
    totalDiscounts += subtotalDiscounts;

    Console.WriteLine(); 
}
    // Calculate Total
    double finalTotalFees = totalFees - totalDiscounts;
    double discountPercentage = (totalDiscounts / totalFees) * 100;

    Console.WriteLine($"Total Subtotal Fees: ${totalFees}");
    Console.WriteLine($"Total Subtotal Discounts: ${totalDiscounts}");
    Console.WriteLine($"Final Total Fees: ${finalTotalFees}");
    Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%");
}

double CalculateFlightFees(Flight flight)
{
    return flight.CalculateFees();
}
double CalculateDiscounts(Airline airline, double subtotalFees)
{
    return airline.CalculateFees();
}




//MAIN PROGRAM//
while (true)
    {
        Console.WriteLine("=============================================");
        Console.WriteLine("Welcome to Changi Airport Terminal 5");
        Console.WriteLine("=============================================");
        Console.WriteLine("1. List All Flights");
        Console.WriteLine("2. List Boarding Gates");
        Console.WriteLine("3. Assign a Boarding Gate to a Flight");
        Console.WriteLine("4. Create Flight");
        Console.WriteLine("5. Display Airline Flights");
        Console.WriteLine("6. Modify Flight Details");
        Console.WriteLine("7. Display Flight Schedule");
        Console.WriteLine("8. Automatically process all unassigned Flights and gates");
        Console.WriteLine("9. Display Total Fees Per Airline");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Please select your option: ");
        string input = Console.ReadLine();

        if (input != "1" || input != "2" || input != "3" || input != "4" || input != "5" || input != "6" || input != "7" || input !="8" || input!="9" || input != "0")
        {
            if (input == "1")
            {
                DisplayBasicInfo(flightsDictionary);
            }
            else if (input == "2")
            {
                DisplayBoardingGatesInfo(boardingGateDictionary);
            }
            else if (input == "3")
            {
                AssignBoardingGate();
        }
            else if (input == "4")
            {
                AddNewFlights(flightsDictionary);
            }
            else if (input == "5")
            {
                listAirlineAvail();
            }
            else if (input == "6")
            {
                modifyFlightDetail();
            }
            else if (input == "7")
        {
            DisplaySortedFlights(flightsDictionary,airlineMapDictionary);
        }
            else if(input == "8")
        {
            processUnassignedFlights();
        }
            else if (input == "9")
        {
            DisplayTotalFees();
        }
            else
            {
                Console.WriteLine("Goodbye!");
                break;
            }

        }
        else
        {
            Console.WriteLine("Please input a valid option!");
        }
    }




