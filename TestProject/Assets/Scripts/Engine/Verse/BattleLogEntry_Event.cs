using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000107 RID: 263
	public class BattleLogEntry_Event : LogEntry
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00020C3C File Offset: 0x0001EE3C
		private string SubjectName
		{
			get
			{
				if (this.subjectPawn == null)
				{
					return "null";
				}
				return this.subjectPawn.LabelShort;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00020C57 File Offset: 0x0001EE57
		public BattleLogEntry_Event() : base(null)
		{
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00020C60 File Offset: 0x0001EE60
		public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator) : base(null)
		{
			if (subject is Pawn)
			{
				this.subjectPawn = (subject as Pawn);
			}
			else if (subject != null)
			{
				this.subjectThing = subject.def;
			}
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			this.eventDef = eventDef;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00020CC5 File Offset: 0x0001EEC5
		public override bool Concerns(Thing t)
		{
			return t == this.subjectPawn || t == this.initiatorPawn;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00020CDB File Offset: 0x0001EEDB
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.subjectPawn != null)
			{
				yield return this.subjectPawn;
			}
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			yield break;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00020CEB File Offset: 0x0001EEEB
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.subjectPawn && CameraJumper.CanJump(this.initiatorPawn)) || (pov == this.initiatorPawn && CameraJumper.CanJump(this.subjectPawn));
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00020D25 File Offset: 0x0001EF25
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.subjectPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn);
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.subjectPawn);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00020D60 File Offset: 0x0001EF60
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Includes.Add(this.eventDef);
			if (this.subjectPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", this.subjectPawn, result.Constants, true, true));
			}
			else if (this.subjectThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", this.subjectThing));
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			return result;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00020E30 File Offset: 0x0001F030
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.eventDef, "eventDef");
			Scribe_References.Look<Pawn>(ref this.subjectPawn, "subjectPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.subjectThing, "subjectThing");
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00020E95 File Offset: 0x0001F095
		public override string ToString()
		{
			return this.eventDef.defName + ": " + this.subjectPawn;
		}

		// Token: 0x0400069A RID: 1690
		protected RulePackDef eventDef;

		// Token: 0x0400069B RID: 1691
		protected Pawn subjectPawn;

		// Token: 0x0400069C RID: 1692
		protected ThingDef subjectThing;

		// Token: 0x0400069D RID: 1693
		protected Pawn initiatorPawn;

		// Token: 0x0400069E RID: 1694
		protected ThingDef initiatorThing;
	}
}
