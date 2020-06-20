using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A33 RID: 2611
	public class Area_SnowClear : Area
	{
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x00145EE1 File Offset: 0x001440E1
		public override string Label
		{
			get
			{
				return "SnowClear".Translate();
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x00145EF2 File Offset: 0x001440F2
		public override Color Color
		{
			get
			{
				return new Color(0.8f, 0.1f, 0.1f);
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0013A1CF File Offset: 0x001383CF
		public override int ListPriority
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00145DFA File Offset: 0x00143FFA
		public Area_SnowClear()
		{
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x00145E02 File Offset: 0x00144002
		public Area_SnowClear(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00145F08 File Offset: 0x00144108
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_SnowClear";
		}
	}
}
