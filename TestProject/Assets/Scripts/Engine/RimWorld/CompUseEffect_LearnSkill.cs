using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA4 RID: 3492
	public class CompUseEffect_LearnSkill : CompUseEffect
	{
		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x060054D7 RID: 21719 RVA: 0x001C470F File Offset: 0x001C290F
		private SkillDef Skill
		{
			get
			{
				return this.parent.GetComp<CompNeurotrainer>().skill;
			}
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x001C4724 File Offset: 0x001C2924
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			SkillDef skill = this.Skill;
			int level = user.skills.GetSkill(skill).Level;
			user.skills.Learn(skill, 50000f, true);
			int level2 = user.skills.GetSkill(skill).Level;
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("SkillNeurotrainerUsed".Translate(user.LabelShort, skill.LabelCap, level, level2, user.Named("USER")), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x001C47CC File Offset: 0x001C29CC
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (p.skills == null)
			{
				failReason = null;
				return false;
			}
			if (p.skills.GetSkill(this.Skill).TotallyDisabled)
			{
				failReason = "SkillDisabled".Translate();
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		// Token: 0x04002E87 RID: 11911
		private const float XPGainAmount = 50000f;
	}
}
