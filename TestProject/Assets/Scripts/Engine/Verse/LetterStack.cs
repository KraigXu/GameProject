using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003A0 RID: 928
	public sealed class LetterStack : IExposable
	{
		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000A6FE5 File Offset: 0x000A51E5
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001B45 RID: 6981 RVA: 0x000A6FED File Offset: 0x000A51ED
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x000A6FF8 File Offset: 0x000A51F8
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000A7020 File Offset: 0x000A5220
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000A704C File Offset: 0x000A524C
		public void ReceiveLetter(Letter let, string debugInfo = null)
		{
			if (!let.CanShowInLetterStack)
			{
				return;
			}
			let.def.arriveSound.PlayOneShotOnCamera(null);
			if (Prefs.AutomaticPauseMode >= let.def.pauseMode)
			{
				Find.TickManager.Pause();
			}
			else if (let.def.forcedSlowdown)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			let.arrivalTime = Time.time;
			let.arrivalTick = Find.TickManager.TicksGame;
			let.debugInfo = debugInfo;
			this.letters.Add(let);
			Find.Archive.Add(let);
			let.Received();
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000A70ED File Offset: 0x000A52ED
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000A7104 File Offset: 0x000A5304
		public void LettersOnGUI(float baseY)
		{
			float num = baseY;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				num -= 30f;
				this.letters[i].DrawButtonAt(num);
				num -= 12f;
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.Repaint)
			{
				num = baseY;
				for (int j = this.letters.Count - 1; j >= 0; j--)
				{
					num -= 30f;
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 12f;
				}
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000A71A0 File Offset: 0x000A53A0
		public void LetterStackTick()
		{
			int num = Find.TickManager.TicksGame + 1;
			for (int i = 0; i < this.letters.Count; i++)
			{
				LetterWithTimeout letterWithTimeout = this.letters[i] as LetterWithTimeout;
				if (letterWithTimeout != null && letterWithTimeout.TimeoutActive && letterWithTimeout.disappearAtTick == num)
				{
					letterWithTimeout.OpenLetter();
					return;
				}
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000A7200 File Offset: 0x000A5400
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000A728B File Offset: 0x000A548B
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000A72A0 File Offset: 0x000A54A0
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x0400102B RID: 4139
		private List<Letter> letters = new List<Letter>();

		// Token: 0x0400102C RID: 4140
		private int mouseoverLetterIndex = -1;

		// Token: 0x0400102D RID: 4141
		private float lastTopYInt;

		// Token: 0x0400102E RID: 4142
		private const float LettersBottomY = 350f;

		// Token: 0x0400102F RID: 4143
		public const float LetterSpacing = 12f;
	}
}
