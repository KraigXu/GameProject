using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public sealed class LetterStack : IExposable
	{
		
		
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		
		
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo);
		}

		
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo);
		}

		
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

		
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		
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

		
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		
		private List<Letter> letters = new List<Letter>();

		
		private int mouseoverLetterIndex = -1;

		
		private float lastTopYInt;

		
		private const float LettersBottomY = 350f;

		
		public const float LetterSpacing = 12f;
	}
}
