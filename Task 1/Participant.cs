using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
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
}
