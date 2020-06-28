using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003533 RID: 13619 RVA: 0x00122F89 File Offset: 0x00121189
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003534 RID: 13620 RVA: 0x00122F9C File Offset: 0x0012119C
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00122FAF File Offset: 0x001211AF
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00122FE8 File Offset: 0x001211E8
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00123018 File Offset: 0x00121218
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
			{
				yield return string.Concat(new object[]
				{
					"CompRottable needs tickerType ",
					TickerType.Rare,
					" or ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x04001C86 RID: 7302
		public float daysToRotStart = 2f;

		// Token: 0x04001C87 RID: 7303
		public bool rotDestroys;

		// Token: 0x04001C88 RID: 7304
		public float rotDamagePerDay = 40f;

		// Token: 0x04001C89 RID: 7305
		public float daysToDessicated = 999f;

		// Token: 0x04001C8A RID: 7306
		public float dessicatedDamagePerDay;

		// Token: 0x04001C8B RID: 7307
		public bool disableIfHatcher;
	}
}
