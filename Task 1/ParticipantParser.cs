using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace Task_1
{
	public class ParticipantsParser
	{
		public List<Participant> Parse(FileInfo file)
		{
			string extension = file.Extension;
			switch (extension)
			{
				case ".csv":
					return ParseFromCSV(file);
				case ".xml":
					return ParseFromXML(file);
				case ".json":
					return ParseFromJSON(file);
				default:
					throw new FileLoadException("Not supported extension");
			}
		}
		private List<Participant> ParseFromXML(FileInfo fileInfo)
		{
			XmlDocument docXML = new XmlDocument();
			docXML.LoadXml(File.ReadAllText(fileInfo.FullName));

			List<Participant> participants = new List<Participant>();
			foreach (XmlNode node in docXML.DocumentElement!)
			{
				string name = node["Name"].InnerText;
				string surname = node["Surname"].InnerText;
				DateTime dateTime = DateTime.Parse(node["RegisterDate"].InnerText);
				participants.Add(new Participant(name, surname, dateTime, "Сервис №2"));
			}
			return participants;
		}
		private List<Participant> ParseFromCSV(FileInfo fileInfo)
		{
			List<Participant> participants = new List<Participant>();

			using (TextFieldParser parser = new TextFieldParser(fileInfo.FullName))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					string[] patricipantProperties = parser.ReadFields();
					participants.Add(new Participant(patricipantProperties[0], patricipantProperties[1], DateTime.Parse(patricipantProperties[2]), "Сервис №3"));
				}
			}
			return participants;
		}
		private List<Participant> ParseFromJSON(FileInfo fileInfo)
		{
			List<Participant> participants = new List<Participant>();

			var jsonParticipantModel = new { FirstName = "", LastName = "", RegistrationDate = DateTime.Now };
			var jObjects = JsonConvert.DeserializeAnonymousType(File.ReadAllText(fileInfo.FullName), new List<Object>());

			foreach (var jObject in jObjects)
			{
				var jParticipant = JsonConvert.DeserializeAnonymousType(jObject.ToString(), jsonParticipantModel);
				participants.Add(new Participant(jParticipant.FirstName, jParticipant.LastName, jParticipant.RegistrationDate, "Сервис №1"));
			}
			return participants;
		}
	}


}
