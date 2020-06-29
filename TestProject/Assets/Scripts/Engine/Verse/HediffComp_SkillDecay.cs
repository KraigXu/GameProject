using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_SkillDecay : HediffComp
	{
		
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0005F072 File Offset: 0x0005D272
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		
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
