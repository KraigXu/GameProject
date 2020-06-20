using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020001C1 RID: 449
	public class RegionLinkDatabase
	{
		// Token: 0x06000C8F RID: 3215 RVA: 0x00047D3C File Offset: 0x00045F3C
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00047D7C File Offset: 0x00045F7C
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00047D90 File Offset: 0x00045F90
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040009E9 RID: 2537
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
