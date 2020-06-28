using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200061B RID: 1563
	public class Inspiration : IExposable
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000F98F8 File Offset: 0x000F7AF8
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002ABE RID: 10942 RVA: 0x000F9900 File Offset: 0x000F7B00
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000F9910 File Offset: 0x000F7B10
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000F9983 File Offset: 0x000F7B83
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000F99B9 File Offset: 0x000F7BB9
		public virtual void InspirationTick()
		{
			this.age++;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000F99E2 File Offset: 0x000F7BE2
		public virtual void PostStart()
		{
			this.SendBeginLetter();
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000F99EA File Offset: 0x000F7BEA
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000F99F2 File Offset: 0x000F7BF2
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000F9A0C File Offset: 0x000F7C0C
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

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000F9B44 File Offset: 0x000F7D44
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

		// Token: 0x0400196F RID: 6511
		public Pawn pawn;

		// Token: 0x04001970 RID: 6512
		public InspirationDef def;

		// Token: 0x04001971 RID: 6513
		private int age;

		// Token: 0x04001972 RID: 6514
		public string reason;
	}
}
