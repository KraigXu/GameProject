﻿using System;
using Verse;

namespace RimWorld
{
	
	public class Inspiration : IExposable
	{
		
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000F98F8 File Offset: 0x000F7AF8
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		
		// (get) Token: 0x06002ABE RID: 10942 RVA: 0x000F9900 File Offset: 0x000F7B00
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000F9910 File Offset: 0x000F7B10
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
		}

		
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		
		public virtual void InspirationTick()
		{
			this.age++;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		
		public virtual void PostStart()
		{
			this.SendBeginLetter();
		}

		
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		
		protected virtual void SendBeginLetter()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			if (!string.IsNullOrWhiteSpace(this.reason))
			{
				taggedString = this.reason.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true) + "\n\n" + taggedString;
			}
			string str = (this.def.beginLetterLabel ?? this.def.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
			Find.LetterStack.ReceiveLetter(str, taggedString, this.def.beginLetterDef, this.pawn, null, null, null, null);
		}

		
		protected virtual void AddEndMessage()
		{
			if (this.def.endMessage.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			Messages.Message(this.def.endMessage.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		
		public Pawn pawn;

		
		public InspirationDef def;

		
		private int age;

		
		public string reason;
	}
}
