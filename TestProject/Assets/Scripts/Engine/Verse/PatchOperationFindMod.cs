using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000210 RID: 528
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x06000EE3 RID: 3811 RVA: 0x00054928 File Offset: 0x00052B28
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return true;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00054992 File Offset: 0x00052B92
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false));
		}

		// Token: 0x04000B00 RID: 2816
		private List<string> mods;

		// Token: 0x04000B01 RID: 2817
		private PatchOperation match;

		// Token: 0x04000B02 RID: 2818
		private PatchOperation nomatch;
	}
}
