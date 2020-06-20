using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000875 RID: 2165
	public class CompProperties_ProximityFuse : CompProperties
	{
		// Token: 0x06003530 RID: 13616 RVA: 0x00122F5A File Offset: 0x0012115A
		public CompProperties_ProximityFuse()
		{
			this.compClass = typeof(CompProximityFuse);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x00122F72 File Offset: 0x00121172
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Concat(new object[]
				{
					"CompProximityFuse needs tickerType ",
					TickerType.Rare,
					" or faster, has ",
					parentDef.tickerType
				});
			}
			if (parentDef.CompDefFor<CompExplosive>() == null)
			{
				yield return "CompProximityFuse requires a CompExplosive";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001C84 RID: 7300
		public ThingDef target;

		// Token: 0x04001C85 RID: 7301
		public float radius;
	}
}
