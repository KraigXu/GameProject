using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public class FiringIncident : IExposable
	{
		// Token: 0x06003BC1 RID: 15297 RVA: 0x0013B58D File Offset: 0x0013978D
		public FiringIncident()
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0013B5A0 File Offset: 0x001397A0
		public FiringIncident(IncidentDef def, StorytellerComp source, IncidentParms parms = null)
		{
			this.def = def;
			if (parms != null)
			{
				this.parms = parms;
			}
			this.source = source;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0013B5CB File Offset: 0x001397CB
		public void ExposeData()
		{
			Scribe_Defs.Look<IncidentDef>(ref this.def, "def");
			Scribe_Deep.Look<IncidentParms>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0013B5F4 File Offset: 0x001397F4
		public override string ToString()
		{
			string text = this.def.defName;
			if (this.parms != null)
			{
				text = text + ", parms=(" + this.parms.ToString() + ")";
			}
			if (this.source != null)
			{
				text = text + ", source=" + this.source;
			}
			if (this.sourceQuestPart != null)
			{
				text = text + ", sourceQuestPart=" + this.sourceQuestPart;
			}
			return text;
		}

		// Token: 0x0400233A RID: 9018
		public IncidentDef def;

		// Token: 0x0400233B RID: 9019
		public IncidentParms parms = new IncidentParms();

		// Token: 0x0400233C RID: 9020
		public StorytellerComp source;

		// Token: 0x0400233D RID: 9021
		public QuestPart sourceQuestPart;
	}
}
