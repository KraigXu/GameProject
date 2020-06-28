using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9F RID: 3487
	public class CompUseEffect_FinishRandomResearchProject : CompUseEffect
	{
		// Token: 0x060054B9 RID: 21689 RVA: 0x001C3AB4 File Offset: 0x001C1CB4
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			if (currentProj != null)
			{
				this.FinishInstantly(currentProj, usedBy);
			}
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001C3ADE File Offset: 0x001C1CDE
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (Find.ResearchManager.currentProj == null)
			{
				failReason = "NoActiveResearchProjectToFinish".Translate();
				return false;
			}
			failReason = null;
			return true;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001C3B03 File Offset: 0x001C1D03
		private void FinishInstantly(ResearchProjectDef proj, Pawn usedBy)
		{
			Find.ResearchManager.FinishProject(proj, false, null);
			Messages.Message("MessageResearchProjectFinishedByItem".Translate(proj.LabelCap), usedBy, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
