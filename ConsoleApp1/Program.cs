//Danish does question 1,4,7,8
//Tze wei does question 2,3,5,6,9
using ConsoleApp1;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Immutable;
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
    string[] flightsLeft = csvlines;
    for (int i = 1; i < dataBoarding.Length; i++)
    { //Created the boardingGates objects from files//
        string[] currentData = dataBoarding[i].Split(",");
        BoardingGate toAdd = new BoardingGate(currentData[0], Convert.ToBoolean(currentData[2]), Convert.ToBoolean(currentData[1]), Convert.ToBoolean(currentData[3]), null);
        boardingGateDictionary.Add(currentData[0], toAdd);
    }
    for (int i = 1; i < flightsLeft.Length; i++)
    { //Sorting the boarding gates for the flights//
        string[] currentData = flightsLeft[i].Split(",");
        Flight flightObj = flightsDictionary[currentData[0]];
        string specialCode;
        if (currentData[4] != null)
        {
            specialCode = currentData[4];
            if (specialCode == "DDJB")
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
            else if (specialCode == "CFFT")
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
                    if (selected.SupportsLWTT == true && selected.Flight == null)
                    {
                        selected.Flight = flightObj;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (var x in boardingGateDictionary)
            {
                BoardingGate selected = x.Value;
                if (selected.SupportsDDJB == false && selected.SupportsCFFT == false && selected.SupportsLWTT == false && selected.Flight == null)
                {
                    selected.Flight = flightObj;
                    break;
                }
            }
        }
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
            Console.WriteLine("Enter Flight Number:");
            string flightNumber = Console.ReadLine().Trim();

            Console.WriteLine("Enter Origin:");
            string origin = Console.ReadLine().Trim();

            Console.WriteLine("Enter Destination:");
            string destination = Console.ReadLine().Trim();

            Console.WriteLine("Enter Expected Departure/Arrival Time (dd/MM/yyyy hh:mm tt):");
            string input = Console.ReadLine().Trim();

            DateTime expectedTime;

            try
            {
                expectedTime = DateTime.Parse(input);
                Console.WriteLine($"Parsed time: {expectedTime}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid date format. Please enter the date and time in the correct format.");
                return;
            }

            Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None):");
            string specialRequestCode = Console.ReadLine().Trim().ToUpper();

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
                using (StreamWriter sw = new StreamWriter("flights.csv", append: true, Encoding.UTF8))
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
        Console.Write(message);
        string response = Console.ReadLine().ToUpper();
        return response == "Y";
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
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Please select your option: ");
        string input = Console.ReadLine();

        if (input != "1" || input != "2" || input != "3" || input != "4" || input != "5" || input != "6" || input != "7" || input != "0")
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




