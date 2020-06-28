using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000848 RID: 2120
	public class ThoughtWorker_PassionateWork : ThoughtWorker
	{
		// Token: 0x060034A1 RID: 13473 RVA: 0x00120788 File Offset: 0x0011E988
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			JobDriver curDriver = p.jobs.curDriver;
			if (curDriver == null)
			{
				return ThoughtState.Inactive;
			}
			if (p.skills == null)
			{
				return ThoughtState.Inactive;
			}
			if (curDriver.ActiveSkill == null)
			{
				return ThoughtState.Inactive;
			}
			SkillRecord skill = p.skills.GetSkill(curDriver.ActiveSkill);
			if (skill == null)
			{
				return ThoughtState.Inactive;
			}
			if (skill.passion == Passion.Minor)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (skill.passion == Passion.Major)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.Inactive;
		}
	}
}
