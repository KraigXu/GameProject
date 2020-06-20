using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001024 RID: 4132
	public class StatWorker_LeatherAmount : StatWorker
	{
		// Token: 0x060062E8 RID: 25320 RVA: 0x00225977 File Offset: 0x00223B77
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			foreach (Dialog_InfoCard.Hyperlink hyperlink in this.<>n__0(statRequest))
			{
				yield return hyperlink;
			}
			IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
			if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.leatherDef == null)
			{
				yield break;
			}
			yield return new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.leatherDef, -1);
			yield break;
			yield break;
		}
	}
}
