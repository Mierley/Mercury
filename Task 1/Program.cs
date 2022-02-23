

using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

public class Participant
{
	public Participant(string name, string surname, DateTime dateTime, string provider)
	{
		Name = name;
		Surname = surname;
		DateTime = dateTime;
		Provider = provider;
	}

	public string Name { get; }
	public string Surname { get; }
	public DateTime DateTime { get; }
	public string Provider { get; }

	public override bool Equals(object? obj)
	{
		if (obj is not Participant other)
			return false;

		return Name.Equals(other.Name) && Surname.Equals(other.Surname);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Surname);
	}
}

public class ParticipantsParser
{
	public ParticipantsParser() { }
	public List<Participant> Parse(FileInfo file)
	{
		String extension = file.Extension;
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

		using (Microsoft.VisualBasic.FileIO.TextFieldParser parser = new TextFieldParser(fileInfo.FullName))
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


public class ParticipantsDB : GeneralDB<Participant>
{
	public override List<Participant> LoadAll(FileInfo[] files)
	{
		HashSet<Participant> participants = new HashSet<Participant>();
		ParticipantsParser parser = new ParticipantsParser();

		foreach (FileInfo file in files)
		{
			foreach (Participant participant in parser.Parse(file))
			{
				Participant previousParticipant;

				if (!participants.TryGetValue(participant, out previousParticipant))
					participants.Add(participant);
				else
				{
					if (previousParticipant.DateTime > participant.DateTime)
					{
						participants.Remove(previousParticipant);
						participants.Add(participant);
					}
				}
			}
		}

		return participants.OrderBy(i => i.DateTime).ToList();
	}

}

public abstract class GeneralDB<T>
{
	public abstract List<T> LoadAll(FileInfo[] files);
}

/// <summary>
/// класс обертка
/// лист -> книга
/// </summary>
public abstract class ConsoleBook<T>
{
	protected GeneralDB<T> db;
	public abstract void PrintPage(int page_num, int page_size);

	public abstract void PrintSearchResult(string searchString);
}

public class ParticipantConsoleBook : ConsoleBook<Participant>
{
	private List<Participant> participantsFromDB;
	protected FileInfo[] files;
	public ParticipantConsoleBook(FileInfo[] files)
	{
		this.files = files;
		db = new ParticipantsDB(); 
		participantsFromDB = db.LoadAll(files);
	}
	public override void PrintPage(int page_num, int page_size = 5)
	{
		PrintTable(GetParticipantsOnPage(page_num, page_size));
	}
	public override void PrintSearchResult(string searchString)
	{
		PrintTable(GetSearchResult(searchString));
	}
	private List<Participant> GetParticipantsOnPage(int page_num, int page_size = 5)
	{
		if (participantsFromDB.Count >= page_size * (page_num-1))
		{
			return participantsFromDB.GetRange(page_size * (page_num - 1), Math.Min(participantsFromDB.Count - (page_num - 1) * page_size, page_size));
		}
		return participantsFromDB;
	}

	private List<Participant> GetSearchResult(string searchString)
	{
		return participantsFromDB.Where(x => x.Surname.Contains(searchString) || x.Name.Contains(searchString)).ToList();
	}
	private void PrintTable(List<Participant> participants)
	{
		Console.WriteLine(String.Format("{0,-18} {1,-25} {2,-30} {3,-20}\n\n", "Имя", "Фамилия", "Дата регистрации", "Поставщик"));

		foreach (var participant in participants)
		{
			Console.WriteLine(String.Format("{0,-18} {1,-25} {2,-30} {3,-20}\n", participant.Name, participant.Surname, participant.DateTime.ToString(), participant.Provider));
		}
	}
}
public class Programm
{
	static void Main(string[] args)
	{
		FileInfo[] files =
		{
			new FileInfo(@"C:\MINE\Programming\Mercury\Task 1\participants.csv"),
			new FileInfo(@"C:\MINE\Programming\Mercury\Task 1\participants.json"),
			new FileInfo(@"C:\MINE\Programming\Mercury\Task 1\participants.xml")
		};

		ParticipantConsoleBook book = new ParticipantConsoleBook(files);
		book.PrintPage(2, 5);
		book.PrintSearchResult("а");
	}

}
