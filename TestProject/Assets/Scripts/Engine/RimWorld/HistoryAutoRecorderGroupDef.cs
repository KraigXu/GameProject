using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D0 RID: 2256
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x06003633 RID: 13875 RVA: 0x00125E1B File Offset: 0x0012401B
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x04001E5D RID: 7773
		public bool useFixedScale;

		// Token: 0x04001E5E RID: 7774
		public Vector2 fixedScale;

		// Token: 0x04001E5F RID: 7775
		public bool integersOnly;

		// Token: 0x04001E60 RID: 7776
		public bool onlyPositiveValues = true;

		// Token: 0x04001E61 RID: 7777
		public bool devModeOnly;

		// Token: 0x04001E62 RID: 7778
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
