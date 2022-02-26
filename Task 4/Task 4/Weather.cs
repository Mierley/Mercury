using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Task_4
{
	public class JObject
	{
		public List<Weather> weather;
		public Main main;

		public static JObject? WeatherParseJSON(string jString)
		{
			return JsonConvert.DeserializeObject<JObject>(jString);
		}
	}

	public class Weather
	{
		public string Main { get; set; }

		public string Description { get; set; }

	}

	public class Main
	{
		public string Temp { get; set; }
		public string feels_like { get; set; }
	}


}
