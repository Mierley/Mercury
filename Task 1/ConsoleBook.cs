using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
	public abstract class ConsoleBook<T>
	{
		protected GeneralDB<T> db;
		public abstract void PrintPage(int page_num, int page_size);
		public abstract void PrintSearchResult(string searchString);
	}

	public sealed class ParticipantConsoleBook : ConsoleBook<Participant>
	{
		private readonly List<Participant> participantsFromDB;

		public ParticipantConsoleBook(FileInfo[] files)
		{
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
			if (participantsFromDB.Count >= page_size * (page_num - 1))
			{
				return participantsFromDB.GetRange(page_size * (page_num - 1), Math.Min(participantsFromDB.Count - (page_num - 1) * page_size, page_size));
			}

			return participantsFromDB.GetRange(participantsFromDB.Count - page_size, page_size);
		}

		private List<Participant> GetSearchResult(string searchString)
		{
			return participantsFromDB.Where(x => x.Surname.Contains(searchString) || x.Name.Contains(searchString)).ToList();
		}

		private void PrintTable(List<Participant> participants)
		{
			if (participants.Count != 0)
			{
				Console.WriteLine("\n{0,-18} {1,-25} {2,-30} {3,-20}\n", "Имя", "Фамилия", "Дата регистрации", "Поставщик");

				foreach (var participant in participants)
				{
					Console.WriteLine("{0,-18} {1,-25} {2,-30} {3,-20}\n", participant.Name, participant.Surname, participant.DateTime.ToString(), participant.Provider);
				}
			}
		}
	}
}
