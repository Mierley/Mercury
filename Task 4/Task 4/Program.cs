using System.Runtime.CompilerServices;

namespace Task_4
{
	public class Program
	{
		public static async Task Main()
		{
			string city, tempScale;

			try
			{
				ReadFromConsole(out city, out tempScale);
				Dictionary<string, string> queryParamDictionary = new Dictionary<string, string>()
				{
					{"q", city},
					{"units", tempScale=="Fahrenheit" ? "imperial" : "metric"},
				};

				
				PrintToConsole(city, tempScale, await OpenWeatherTools.GetResponse(queryParamDictionary));
			}
			catch (Exception e)
			{
				Console.WriteLine("\nInvalid input!!!");
			}
		}

		public static void ReadFromConsole(out string city, out string tempScale)
		{
			Console.WriteLine("The name of city:");
			city = Console.ReadLine();

			Console.WriteLine($"The temperature scale ({TempScales.Fahrenheit.ToString()} or {TempScales.Celsius}):");
			tempScale = Console.ReadLine();
			TempScales temp;
			
			if (!TempScales.TryParse(tempScale, true, out temp))
				throw new Exception();

			tempScale = temp.ToString();
		}

		public static void PrintToConsole(string city, string tempScale, string response)
		{
			JObject ?jWeather = JObject.WeatherParseJSON(response); 
			Console.WriteLine("===================");
			Console.WriteLine($"City: {city}");
			Console.WriteLine($"Weather: {jWeather.weather[0].Main}({jWeather.weather[0].Description})");
			Console.WriteLine($"Tempature({tempScale}): {jWeather.main.Temp}");
			Console.WriteLine($"Tempature feels like({tempScale}): {jWeather.main.feels_like}");
		}
	}
}

