using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A32 RID: 2610
	public class Area_NoRoof : Area
	{
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x00145E97 File Offset: 0x00144097
		public override string Label
		{
			get
			{
				return "NoRoof".Translate();
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x00145EA8 File Offset: 0x001440A8
		public override Color Color
		{
			get
			{
				return new Color(0.9f, 0.5f, 0.1f);
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00145EBE File Offset: 0x001440BE
		public override int ListPriority
		{
			get
			{
				return 8000;
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00145DFA File Offset: 0x00143FFA
		public Area_NoRoof()
		{
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00145E02 File Offset: 0x00144002
		public Area_NoRoof(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x00145EC5 File Offset: 0x001440C5
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_NoRoof";
		}
	}
}
