using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class StatWorker_LeatherAmount : StatWorker
	{
		
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{


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
