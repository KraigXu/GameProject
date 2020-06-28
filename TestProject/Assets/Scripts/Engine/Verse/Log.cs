using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000030 RID: 48
	public static class Log
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000F2EB File Offset: 0x0000D4EB
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000F2F7 File Offset: 0x0000D4F7
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.messageCount >= 1000;
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000F308 File Offset: 0x0000D508
		public static void ResetMessageCount()
		{
			bool reachedMaxMessagesLimit = Log.ReachedMaxMessagesLimit;
			Log.messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000F322 File Offset: 0x0000D522
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (!ignoreStopLoggingLimit && Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.Log(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000F350 File Offset: 0x0000D550
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (!ignoreStopLoggingLimit && Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.LogWarning(text);
			Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
			Log.PostMessage();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F380 File Offset: 0x0000D580
		public static void Error(string text, bool ignoreStopLoggingLimit = false)
		{
			if (!ignoreStopLoggingLimit && Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			Debug.LogError(text);
			if (!Log.currentlyLoggingError)
			{
				Log.currentlyLoggingError = true;
				try
				{
					if (Prefs.PauseOnError && Current.ProgramState == ProgramState.Playing)
					{
						Find.TickManager.Pause();
					}
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()));
					Log.PostMessage();
					if (!PlayDataLoader.Loaded || Prefs.DevMode)
					{
						Log.TryOpenLogWindow();
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("An error occurred while logging an error: " + arg);
				}
				finally
				{
					Log.currentlyLoggingError = false;
				}
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F42C File Offset: 0x0000D62C
		public static void ErrorOnce(string text, int key, bool ignoreStopLoggingLimit = false)
		{
			if (!ignoreStopLoggingLimit && Log.ReachedMaxMessagesLimit)
			{
				return;
			}
			if (Log.usedKeys.Contains(key))
			{
				return;
			}
			Log.usedKeys.Add(key);
			Log.Error(text, ignoreStopLoggingLimit);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F45A File Offset: 0x0000D65A
		public static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F470 File Offset: 0x0000D670
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000F485 File Offset: 0x0000D685
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
			Log.messageCount++;
			if (Log.messageCount == 1000 && Log.ReachedMaxMessagesLimit)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.", true);
			}
		}

		// Token: 0x04000097 RID: 151
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04000098 RID: 152
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04000099 RID: 153
		public static bool openOnMessage = false;

		// Token: 0x0400009A RID: 154
		private static bool currentlyLoggingError;

		// Token: 0x0400009B RID: 155
		private static int messageCount;

		// Token: 0x0400009C RID: 156
		private const int StopLoggingAtMessageCount = 1000;
	}
}
