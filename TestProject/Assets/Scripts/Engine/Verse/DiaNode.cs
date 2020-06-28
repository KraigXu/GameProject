using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003E5 RID: 997
	public class DiaNode
	{
		// Token: 0x06001D9F RID: 7583 RVA: 0x000B6178 File Offset: 0x000B4378
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000B6194 File Offset: 0x000B4394
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				using (List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00002681 File Offset: 0x00000881
		public void PreClose()
		{
		}

		// Token: 0x04001208 RID: 4616
		public TaggedString text;

		// Token: 0x04001209 RID: 4617
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x0400120A RID: 4618
		protected DiaNodeMold def;
	}
}
