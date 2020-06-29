using System;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		
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
