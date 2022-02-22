

using System.Xml;

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
	private string Provider { get; }

	public override bool Equals(object? obj)
	{
		if (obj is not Participant other)
			return false;

		return Name.Equals(other.Name) && Surname.Equals(other.Surname);
	}
}

public class ParticipantsParser
{
	public ParticipantsParser()
	{

	}
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
		docXML.Load(fileInfo.FullName);

		List<Participant> participants = new List<Participant>();
		foreach (XmlNode node in docXML.DocumentElement!)
		{
			string name = node["Name"].InnerText;
			string surname = node["Surname"].InnerText;
			DateTime dateTime = DateTime.Parse(node["RegisterDate"].InnerText);
			participants.Add(new Participant(name, surname, dateTime, "Сервис №2"));
		}

		return null;
	}

	private List<Participant> ParseFromCSV(FileInfo fileInfo)
	{
		return null;
	}

	private List<Participant> ParseFromJSON(FileInfo fileInfo)
	{
		return null;
	}
}

/// <summary>
/// объедение
/// </summary>
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
public class ConsoleBook<T>
{

}

public class Programm
{
	static void Main(string[] args)
	{
		ParticipantsParser parser = new ParticipantsParser();
		parser.Parse(new FileInfo(@"C:\MINE\Programming\Mercury\Task 1\participants.xml"));
	}

}
