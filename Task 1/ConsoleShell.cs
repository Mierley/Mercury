using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
	public class ConsoleShell
	{
		private static readonly FileInfo[] files;

		private static readonly ParticipantConsoleBook book;

		static ConsoleShell()
		{
			files = new FileInfo[]{
				new(@"C:\MINE\Programming\Mercury\Task 1\participants.csv"),
				new(@"C:\MINE\Programming\Mercury\Task 1\participants.json"),
				new(@"C:\MINE\Programming\Mercury\Task 1\participants.xml")
			};

			book = new ParticipantConsoleBook(files);
		}

		public void StartConsoleApplication()
		{
			string text;
			while ((text = Console.ReadLine()!) != "stop")
			{
				string[] words = text.Split(" ");

				switch (words[0])
				{
					case "get-page":
						{
							int page;
							if (int.TryParse(words[1], out page))
								book.PrintPage(page, 5);
							else
								Console.WriteLine("\nInvalid page number\n");
							break;
						}

					case "search":
						{
							book.PrintSearchResult(words[1].Substring(1, words[1].Length - 2));
							break;
						}

					default:
						{
							Console.WriteLine("\nThis command do not exist\n");
							break;
						}
				}
			}

		}
	}
}

