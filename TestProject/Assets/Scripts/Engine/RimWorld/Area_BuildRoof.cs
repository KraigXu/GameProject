using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A31 RID: 2609
	public class Area_BuildRoof : Area
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x00145E4D File Offset: 0x0014404D
		public override string Label
		{
			get
			{
				return "BuildRoof".Translate();
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x00145E5E File Offset: 0x0014405E
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.9f, 0.5f);
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00145E74 File Offset: 0x00144074
		public override int ListPriority
		{
			get
			{
				return 9000;
			}
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00145DFA File Offset: 0x00143FFA
		public Area_BuildRoof()
		{
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00145E02 File Offset: 0x00144002
		public Area_BuildRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00145E7B File Offset: 0x0014407B
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_BuildRoof";
		}
	}
}
