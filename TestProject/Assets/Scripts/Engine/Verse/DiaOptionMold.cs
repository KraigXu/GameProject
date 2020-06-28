using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x020003EA RID: 1002
	public class DiaOptionMold
	{
		// Token: 0x06001DB0 RID: 7600 RVA: 0x000B6728 File Offset: 0x000B4928
		public DiaNodeMold RandomLinkNode()
		{
			List<DiaNodeMold> list = this.ChildNodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.ChildNodeNames)
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
			if (list.Count == 0)
			{
				return null;
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x04001225 RID: 4645
		public string Text = "OK".Translate();

		// Token: 0x04001226 RID: 4646
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x04001227 RID: 4647
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
