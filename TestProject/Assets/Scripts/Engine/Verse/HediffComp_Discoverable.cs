using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200024E RID: 590
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0005D5FD File Offset: 0x0005B7FD
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0005D60A File Offset: 0x0005B80A
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0005D61E File Offset: 0x0005B81E
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0005D629 File Offset: 0x0005B829
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0005D640 File Offset: 0x0005B840
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0005D648 File Offset: 0x0005B848
		private void CheckDiscovered()
		{
			if (this.discovered)
			{
				return;
			}
			if (!this.parent.CurStage.becomeVisible)
			{
				return;
			}
			this.discovered = true;
			if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					string str;
					if (!this.Props.discoverLetterLabel.NullOrEmpty())
					{
						str = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShortCap).CapitalizeFirst();
					}
					else
					{
						str = "LetterLabelNewDisease".Translate() + ": " + base.Def.LabelCap;
					}
					string str2;
					if (!this.Props.discoverLetterText.NullOrEmpty())
					{
						str2 = this.Props.discoverLetterText.Formatted(base.Pawn.LabelIndefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else if (this.parent.Part == null)
					{
						str2 = "NewDisease".Translate(base.Pawn.Named("PAWN"), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						str2 = "NewPartDisease".Translate(base.Pawn.Named("PAWN"), this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.label).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					Find.LetterStack.ReceiveLetter(str, str2, (this.Props.letterType != null) ? this.Props.letterType : LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null);
					return;
				}
				string text;
				if (!this.Props.discoverLetterText.NullOrEmpty())
				{
					string value = base.Pawn.KindLabelIndefinite();
					if (base.Pawn.Name.IsValid && !base.Pawn.Name.Numerical)
					{
						value = string.Concat(new object[]
						{
							base.Pawn.Name,
							" (",
							base.Pawn.KindLabel,
							")"
						});
					}
					text = this.Props.discoverLetterText.Formatted(value, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else if (this.parent.Part == null)
				{
					text = "NewDiseaseAnimal".Translate(base.Pawn.LabelShort, base.Def.LabelCap, base.Pawn.LabelDefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else
				{
					text = "NewPartDiseaseAnimal".Translate(base.Pawn.LabelShort, this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.LabelCap, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				Messages.Message(text, base.Pawn, (this.Props.messageType != null) ? this.Props.messageType : MessageTypeDefOf.NegativeHealthEvent, true);
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0005D640 File Offset: 0x0005B840
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0005DA93 File Offset: 0x0005BC93
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x04000BEF RID: 3055
		private bool discovered;
	}
}
