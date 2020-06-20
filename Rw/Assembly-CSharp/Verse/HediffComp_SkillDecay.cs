using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000270 RID: 624
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0005F072 File Offset: 0x0005D272
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0005F080 File Offset: 0x0005D280
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn_SkillTracker skills = base.Pawn.skills;
			if (skills == null)
			{
				return;
			}
			for (int i = 0; i < skills.skills.Count; i++)
			{
				SkillRecord skillRecord = skills.skills[i];
				float num = this.parent.Severity * this.Props.decayPerDayPercentageLevelCurve.Evaluate((float)skillRecord.Level);
				float num2 = skillRecord.XpRequiredForLevelUp * num / 60000f;
				skillRecord.Learn(-num2, false);
			}
		}
	}
}
