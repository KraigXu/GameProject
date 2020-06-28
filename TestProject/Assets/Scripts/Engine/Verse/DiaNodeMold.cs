using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003E7 RID: 999
	public class DiaNodeMold
	{
		// Token: 0x06001DA2 RID: 7586 RVA: 0x000B6268 File Offset: 0x000B4468
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		// Token: 0x04001210 RID: 4624
		public string name = "Unnamed";

		// Token: 0x04001211 RID: 4625
		public bool unique;

		// Token: 0x04001212 RID: 4626
		public List<string> texts = new List<string>();

		// Token: 0x04001213 RID: 4627
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04001214 RID: 4628
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x04001215 RID: 4629
		[Unsaved(false)]
		public bool used;

		// Token: 0x04001216 RID: 4630
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
