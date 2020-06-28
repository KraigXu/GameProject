using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008CE RID: 2254
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x00125DCE File Offset: 0x00123FCE
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00125DF4 File Offset: 0x00123FF4
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x04001E57 RID: 7767
		public Type workerClass;

		// Token: 0x04001E58 RID: 7768
		public int recordTicksFrequency = 60000;

		// Token: 0x04001E59 RID: 7769
		public Color graphColor = Color.green;

		// Token: 0x04001E5A RID: 7770
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x04001E5B RID: 7771
		public string valueFormat;

		// Token: 0x04001E5C RID: 7772
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
