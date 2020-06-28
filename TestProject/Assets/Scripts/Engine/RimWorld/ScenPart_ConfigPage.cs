using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C25 RID: 3109
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x06004A27 RID: 18983 RVA: 0x00191416 File Offset: 0x0018F616
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x00002681 File Offset: 0x00000881
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
