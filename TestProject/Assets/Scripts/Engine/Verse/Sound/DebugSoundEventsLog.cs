using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x020004F7 RID: 1271
	public static class DebugSoundEventsLog
	{
		// Token: 0x1700073D RID: 1853
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

		// Token: 0x06002490 RID: 9360 RVA: 0x000D9710 File Offset: 0x000D7910
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

		// Token: 0x06002491 RID: 9361 RVA: 0x000D9784 File Offset: 0x000D7984
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000D97B2 File Offset: 0x000D79B2
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x0400163E RID: 5694
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
