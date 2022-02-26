using System;
using System.Text;

namespace Task_4
{
	public class OpenWeatherTools
	{
		private static readonly HttpClient client = new();
		private static readonly string apiKey = "8f528e888e6c0426103a160586c1b136";

		public static async Task<string> GetResponse(Dictionary<string, string> queryParamDictionary)
		{
			var stringRequest = new StringBuilder();
			stringRequest.Append($"https://api.openweathermap.org/data/2.5/weather?appid={apiKey}");

			foreach (var entry in queryParamDictionary)
			{
				stringRequest.Append($"&{entry.Key}={entry.Value}");
			}

			string responseString =
					await client.GetStringAsync(stringRequest.ToString());
			return responseString;
		}
	}
}
