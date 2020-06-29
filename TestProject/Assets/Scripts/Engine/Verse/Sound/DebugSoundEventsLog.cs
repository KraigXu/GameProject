using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	
	public static class DebugSoundEventsLog
	{
		
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x000D96A8 File Offset: 0x000D78A8
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (!DebugViewSettings.writeSoundEventsRecord)
			{
				return;
			}
			string str;
			if (def == null)
			{
				str = "null: ";
			}
			else if (def.isUndefined)
			{
				str = "Undefined: ";
			}
			else
			{
				str = (def.sustain ? "SustainerSpawn: " : "OneShot: ");
			}
			string str2 = (def != null) ? def.defName : "null";
			DebugSoundEventsLog.CreateRecord(str + str2 + " - " + info.ToString());
		}

		
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
