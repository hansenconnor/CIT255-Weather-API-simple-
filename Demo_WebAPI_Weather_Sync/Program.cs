using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Demo_WebAPI_Weather
{
    class Program
    {
        //test
        static void Main(string[] args)
        {
            DisplayOpeningScreen();
            DisplayMenu();
            DisplayClosingScreen();
        }

        static void DisplayMenu()
        {
            bool quit = false;
            LocationCoordinates coordinates = new LocationCoordinates(0, 0);

            while (!quit)
            {
                DisplayHeader("Main Menu");

                Console.WriteLine("Enter the number of your menu choice.");
                Console.WriteLine();
                Console.WriteLine("1) Set the Location");
                Console.WriteLine("2) Display the Current Weather");
                Console.WriteLine("3) Display weather for a specific city");
                Console.WriteLine("4) Display weather for a specific zipcode");
                Console.WriteLine("5) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                string userMenuChoice = Console.ReadLine();

                switch (userMenuChoice)
                {
                    case "1":
                        coordinates = DisplayGetLocation();
                        break;

                    case "2":
                        DisplayCurrentWeather(coordinates);
                        break;

                    case "3":
                        DisplayWeatherByCity();
                        break;

                    case "4":
                        DisplayWeatherByZip();
                        break;

                    case "5":
                        quit = true;
                        break;

                    default:
                        Console.WriteLine("You must enter a number from the menu.");
                        break;
                }
            }
        }


        static void DisplayWeatherByCity()
        {
            string url;
            string city = "";

            DisplayHeader("Get weather by city");

            // Prompt user for desired city
            Console.Write("Enter City: ");
            city = (Console.ReadLine());

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("http://api.openweathermap.org/data/2.5/weather?");
            sb.Append("q=" + city);
            sb.Append("&appid=63dd8a59e9d07448ddaf0cabe2745f6c");

            url = sb.ToString();

            WeatherData currentWeather = new WeatherData();

            currentWeather = HttpGetCurrentWeatherByLocation(url, "json");

            // Clear the console
            Console.Clear();

            // Display the temp in Fahrenheit
            Console.WriteLine(ConvertToFahrenheit(currentWeather.Main.Temp));

            DisplayContinuePrompt();
        }


        // Display weather by Zip code -- return XML
        static void DisplayWeatherByZip()
        {
            string url;
            string zip = "";
            string result;

            DisplayHeader("Get weather by zip");

            // Prompt user for desired city
            Console.Write("Enter Zip: ");
            zip = (Console.ReadLine());

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("http://api.openweathermap.org/data/2.5/weather?");
            sb.Append("zip=" + zip);
            sb.Append("&appid=63dd8a59e9d07448ddaf0cabe2745f6c&mode=xml");

            url = sb.ToString();

            using (WebClient syncClient = new WebClient())
            {
                result = syncClient.DownloadString(url);
            }

            XmlDocument xmltest = new XmlDocument();
            xmltest.LoadXml(result);

            XmlNodeList elemlist = xmltest.GetElementsByTagName("current");

            string list = elemlist[0].InnerXml;

            Console.WriteLine(list);


            //XmlTextReader reader = new XmlTextReader(result);
            //while (reader.Read())
            //{
            //    // Do some work here on the data.
            //    //Console.WriteLine(reader.Name);
            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element: // The node is an element.
            //            Console.Write("<" + reader.Name);

            //            while (reader.MoveToNextAttribute()) // Read the attributes.
            //                Console.Write(" " + reader.Name + "='" + reader.Value + "'");
            //            Console.Write(">");
            //            Console.WriteLine(">");
            //            break;
            //        case XmlNodeType.Text: //Display the text in each element.
            //            Console.WriteLine(reader.Value);
            //            break;
            //        case XmlNodeType.EndElement: //Display the end of the element.
            //            Console.Write("</" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //    }
            //}
            //Console.ReadLine();


            //XmlRootAttribute xRoot = new XmlRootAttribute();
            //xRoot.IsNullable = true;
            //xRoot.ElementName = "current";
            //XmlSerializer serializer = new XmlSerializer(typeof(WeatherData), xRoot);
            //currentWeather = serializer.Deserialize(result);

            //currentWeather = (WeatherData)serializer.Deserialize(result);
            //StringReader stringReader = new StringReader(result);

            // currentWeather = (WeatherData)serializer.Deserialize(stringReader);
            //return currentWeather;

            //WeatherData currentWeather = new WeatherData();

            //currentWeather = HttpGetCurrentWeatherByLocation(url, "xml");

            //XElement e = new XElement("City", "abcde");
            //Console.WriteLine(e);
            //Console.WriteLine("Value of e:" + (string)e);

            //IEnumerable<XElement> address =
            // from el in currentWeather("Weather")
            //where (string)el.Attribute("Type") == "Billing"
            //  select el;
            //foreach (XElement el in address)
            //    Console.WriteLine(el);

            // Clear the console
            //Console.Clear();

            // Display the temp in Fahrenheit
            //Console.WriteLine(currentWeather);

            DisplayContinuePrompt();
        }


        static void DisplayOpeningScreen()
        {
            //
            // display an opening screen
            //
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Weather Reporter");
            Console.WriteLine();
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        static void DisplayClosingScreen()
        {
            //
            // display an closing screen
            //
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Thank you for using my application!");
            Console.WriteLine();
            Console.WriteLine();

            //
            // display continue prompt
            //
            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Console.WriteLine();
        }

        static void DisplayContinuePrompt()
        {
            //
            // display continue prompt
            //
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.WriteLine();
        }

        static void DisplayHeader(string headerText)
        {
            //
            // display header
            //
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(headerText);
            Console.WriteLine();
        }

        static LocationCoordinates DisplayGetLocation()
        {
            DisplayHeader("Set Location by Coordinates");

            LocationCoordinates coordinates = new LocationCoordinates();

            Console.Write("Enter Latitude: ");
            coordinates.Latitude = double.Parse(Console.ReadLine());

            Console.Write("Enter longitude: ");
            coordinates.Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"Location Coordinates: ({coordinates.Latitude}, {coordinates.Longitude})");
            Console.WriteLine();

            DisplayContinuePrompt();

            return coordinates;
        }

        static WeatherData GetCurrentWeatherData(LocationCoordinates coordinates)
        {
            string url;

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.Append("http://api.openweathermap.org/data/2.5/weather?");
            sb.Append("&lat=" + coordinates.Latitude.ToString());
            sb.Append("&lon=" + coordinates.Longitude.ToString());
            sb.Append("&appid=63dd8a59e9d07448ddaf0cabe2745f6c");

            url = sb.ToString();

            WeatherData currentWeather = new WeatherData();
 
            currentWeather = HttpGetCurrentWeatherByLocation(url, "json");

            return currentWeather;
        }


        static WeatherData HttpGetCurrentWeatherByLocation(string url, string mode)
        {
            string result = null;
            WeatherData currentWeather;

            using (WebClient syncClient = new WebClient())
            {
                result = syncClient.DownloadString(url);
            }

            //Console.WriteLine(result);

            // Check if response should be xml or json
            if (mode == "json")
            {
                // Return json
                currentWeather = JsonConvert.DeserializeObject<WeatherData>(result);
            } else if (mode == "xml")
            {
                // Return XMl
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.IsNullable = true;
                xRoot.ElementName = "current";
                XmlSerializer serializer = new XmlSerializer(typeof(WeatherData), xRoot);
                //currentWeather = serializer.Deserialize(result);

                //currentWeather = (WeatherData)serializer.Deserialize(result);
                StringReader stringReader = new StringReader(result);

                currentWeather = (WeatherData)serializer.Deserialize(stringReader);
                return currentWeather;

                //StreamReader reader = new StreamReader(path);
                //cars = (CarCollection)serializer.Deserialize(reader);
            } else
            {
                currentWeather = null;
            }

            return currentWeather;
        }

        static void DisplayCurrentWeather(LocationCoordinates coordinates)
        {
            DisplayHeader("Current Weather");

            WeatherData currentWeatherData = GetCurrentWeatherData(coordinates);
            
            Console.WriteLine(String.Format("Temperature (Fahrenheit): {0:0.0}", ConvertToFahrenheit(currentWeatherData.Main.Temp)));

            DisplayContinuePrompt();
        }

        static double ConvertToFahrenheit(double degreesKalvin)
        {
            return (degreesKalvin - 273.15) * 1.8 + 32;
        }
    }
}
