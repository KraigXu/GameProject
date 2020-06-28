using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001023 RID: 4131
	public class StatWorker_MeatAmount : StatWorker
	{
		// Token: 0x060062E5 RID: 25317 RVA: 0x00225957 File Offset: 0x00223B57
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__0(statRequest))
			{
				yield return hyperlink;
			}
			IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
			if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.meatDef == null)
			{
				yield break;
			}
			yield return new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.meatDef, -1);
			yield break;
			yield break;
		}
	}
}
