using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003B1 RID: 945
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x06001BDD RID: 7133 RVA: 0x000AA0BC File Offset: 0x000A82BC
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
			{
				Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000AA136 File Offset: 0x000A8336
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, Quest quest, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets, quest), historical);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000AA157 File Offset: 0x000A8357
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets), historical);
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000AA176 File Offset: 0x000A8376
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def), historical);
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000AA1A0 File Offset: 0x000A83A0
		public static void Message(Message msg, bool historical = true)
		{
			if (!Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				return;
			}
			if (historical && Find.Archive != null)
			{
				Find.Archive.Add(msg);
			}
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (msg.def.sound != null)
			{
				msg.def.sound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000AA21B File Offset: 0x000A841B
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000AA228 File Offset: 0x000A8428
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000AA2A8 File Offset: 0x000A84A8
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000AA300 File Offset: 0x000A8500
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000AA30C File Offset: 0x000A850C
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000AA340 File Offset: 0x000A8540
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				if (Messages.liveMessages[i].text == text && Messages.liveMessages[i].startingFrame == RealTime.frameCount && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000AA3B6 File Offset: 0x000A85B6
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x04001072 RID: 4210
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04001073 RID: 4211
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04001074 RID: 4212
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x04001075 RID: 4213
		private const int MessageYInterval = 26;

		// Token: 0x04001076 RID: 4214
		private const int MaxLiveMessages = 12;
	}
}
