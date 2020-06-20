using System;

namespace RimWorld
{
	// Token: 0x02000A17 RID: 2583
	public class StorytellerComp_RandomQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x06003D42 RID: 15682 RVA: 0x00143C66 File Offset: 0x00141E66
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = base.GenerateParms(incCat, target);
			incidentParms.questScriptDef = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(incidentParms.points, target);
			return incidentParms;
		}
	}
}
