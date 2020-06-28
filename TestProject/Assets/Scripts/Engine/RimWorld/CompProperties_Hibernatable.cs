using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000887 RID: 2183
	public class CompProperties_Hibernatable : CompProperties
	{
		// Token: 0x06003549 RID: 13641 RVA: 0x0012326B File Offset: 0x0012146B
		public CompProperties_Hibernatable()
		{
			this.compClass = typeof(CompHibernatable);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x0012328E File Offset: 0x0012148E
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
					"CompHibernatable needs tickerType ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x04001CB3 RID: 7347
		public float startupDays = 14f;

		// Token: 0x04001CB4 RID: 7348
		public IncidentTargetTagDef incidentTargetWhileStarting;
	}
}
