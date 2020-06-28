using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000403 RID: 1027
	public class CreditRecord_Space : CreditsEntry
	{
		// Token: 0x06001E5F RID: 7775 RVA: 0x000BDCC2 File Offset: 0x000BBEC2
		public CreditRecord_Space()
		{
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000BDCD5 File Offset: 0x000BBED5
		public CreditRecord_Space(float height)
		{
			this.height = height;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000BDCEF File Offset: 0x000BBEEF
		public override float DrawHeight(float width)
		{
			return this.height;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x00002681 File Offset: 0x00000881
		public override void Draw(Rect rect)
		{
		}

		// Token: 0x040012CA RID: 4810
		private float height = 10f;
	}
}
