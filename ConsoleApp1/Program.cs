//Danish does question 1,4,7,8
//Tze wei does question 2,3,5,6,9
using ConsoleApp1;
using System.Globalization;
using System.Security.Cryptography;
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

// Helper methods
Flight GetFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNumber = Console.ReadLine().ToUpper();

        // Check if flight exists in the dictionary
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


void modifyFlightDetail()//feature 8
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
            Console.Write("Choose an existing Flight to modify or delete: ");
            string chosen = Console.ReadLine().ToUpper();
            objSelected = selected.Flights[chosen];
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Flight does not exist!");
        }
    }
    Boolean status = true;//triggers to false if it ends
    string option;
    string option2;
    Console.WriteLine("1.Modify Flight");
    Console.WriteLine("2.Delete Flight");
    while (true)
    {
        Console.Write("Choose an option:");
        option = Console.ReadLine();
        if(option =="1" || option == "2")
        {
            break;
        }
        else
        {
            Console.WriteLine("Please choose a valid option!");
            continue;
        }
    }
    if(option == "1")//If Modify flight
    {
        while (status)
        {
            Console.WriteLine("1.Modify Basic Information");
            Console.WriteLine("2.Modify Status");
            Console.WriteLine("3.Modify Special Request Code");
            Console.WriteLine("4.Modify Boarding Gate");
            Console.Write("Choose an option:");
            option2 = Console.ReadLine();
            if(option2 == "1")
            {
                Console.Write("Enter new Origin: ");
                string originNew = Console.ReadLine();
                Console.Write("Enter new Destination: ");
                string destinationNew = Console.ReadLine();
                Boolean check = true;
                DateTime newExpectedTime;
                while (check)
                {
                    try
                    {
                        Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                        newExpectedTime = Convert.ToDateTime(Console.ReadLine());
                        objSelected.ExpectedTime = newExpectedTime;
                        check = false;
                    }
                    catch(FormatException) {
                        Console.WriteLine("Please follow the format given! Try again!");
                    }
                }
                objSelected.Origin = originNew;
                objSelected.Destination = destinationNew;
                
                Console.WriteLine("Flight Updated!");
                Console.WriteLine($"Flight Number: {objSelected.FlightNumber}");
                Console.WriteLine($"Airline Name: {selected.Name}");
                Console.WriteLine($"Origin: {objSelected.Origin}");
                Console.WriteLine($"Destination: {objSelected.Origin}");
                Console.WriteLine($"Expected Departure/Arrival Time: {Convert.ToString(objSelected.ExpectedTime)}");
                Console.WriteLine($"Status: {objSelected.Status}");
                if(objSelected.GetType() == typeof(NORMFlight))
                {
                    Console.WriteLine("Special Request Code: Null");
                }else if(objSelected.GetType() == typeof(LWTTFlight))
                {
                    Console.WriteLine("Special Request Code: LWTT");
                }
                else if(objSelected.GetType() == typeof(DDJBFlight))
                {
                    Console.WriteLine("Special Request Code: DDJB");
                }
                else
                {
                    Console.WriteLine("Special Request Code: CFFT");
                }
                BoardingGate found = null;
                foreach(var x in boardingGateDictionary)
                {
                    if( x.Value.Flight.FlightNumber == objSelected.FlightNumber)
                    {
                        found = x.Value;
                    }
                }
                if (found != null) {
                    Console.WriteLine($"Boarding Gate: {found.GateName}");
                }
                else
                {
                    Console.WriteLine("Boarding Gate: Unassigned");
                }
                status = false;
            }
            else {
                while (status)
                {
                    try
                    {
                        Console.Write("[Y} To Confirm [N]To go back");
                        string reply = Console.ReadLine().ToUpper();
                        if(reply == "Y" || reply == "N")
                        {
                            if(reply == "Y")
                            {
                                selected.Flights.Remove(objSelected.FlightNumber);
                                status = false;
                                
                            }
                            else
                            {
                                status = false; break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid respond");
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
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

    if (input != "1" || input != "2" || input != "3" || input != "4" || input != "5" || input != "6" || input != "7" || input!= "0")
    {
        if (input == "1")
        {
           
        }
        else if (input == "2")
        {
           DisplayBoardingGatesInfo(boardingGateDictionary);
        }
        else if (input == "3")
        {

        }
        else if (input == "4")
        {

        }
        else if (input == "5")
        {
            listAirlineAvail();
        }
        else if (input == "6")
        {
            modifyFlightDetail();
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