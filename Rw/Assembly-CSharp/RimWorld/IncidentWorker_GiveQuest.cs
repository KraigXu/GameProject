using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A02 RID: 2562
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		// Token: 0x06003CED RID: 15597 RVA: 0x0014285C File Offset: 0x00140A5C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			if (this.def.questScriptDef != null)
			{
				if (!this.def.questScriptDef.CanRun(parms.points))
				{
					return false;
				}
			}
			else if (parms.questScriptDef != null && !parms.questScriptDef.CanRun(parms.points))
			{
				return false;
			}
			return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.Any<Pawn>();
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x001428C2 File Offset: 0x00140AC2
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			QuestScriptDef root;
			if ((root = this.def.questScriptDef) == null)
			{
				root = (parms.questScriptDef ?? NaturalRandomQuestChooser.ChooseNaturalRandomQuest(parms.points, parms.target));
			}
			QuestUtility.SendLetterQuestAvailable(QuestUtility.GenerateQuestAndMakeAvailable(root, parms.points));
			return true;
		}
	}
}
