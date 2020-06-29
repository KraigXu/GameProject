using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StatWorker_MeatAmount : StatWorker
	{
		
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{

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
