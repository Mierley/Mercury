using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
	public abstract class GeneralDB<T>
	{
		public abstract List<T> LoadAll(FileInfo[] files);
	}

	public class ParticipantsDB : GeneralDB<Participant>
	{
		/// <summary>
		/// Get Participants from all the files and unite them.
		/// </summary>
		/// <param name="files"></param>
		/// <returns></returns>
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
}
