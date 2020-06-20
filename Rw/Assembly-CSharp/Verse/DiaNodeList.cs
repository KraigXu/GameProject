using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003E8 RID: 1000
	public class DiaNodeList
	{
		// Token: 0x06001DA4 RID: 7588 RVA: 0x000B6384 File Offset: 0x000B4584
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x04001217 RID: 4631
		public string Name = "NeedsName";

		// Token: 0x04001218 RID: 4632
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04001219 RID: 4633
		public List<string> NodeNames = new List<string>();
	}
}
