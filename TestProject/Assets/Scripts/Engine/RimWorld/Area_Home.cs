using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A30 RID: 2608
	public class Area_Home : Area
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x00145DCC File Offset: 0x00143FCC
		public override string Label
		{
			get
			{
				return "Home".Translate();
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x00145DDD File Offset: 0x00143FDD
		public override Color Color
		{
			get
			{
				return new Color(0.3f, 0.3f, 0.9f);
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x00145DF3 File Offset: 0x00143FF3
		public override int ListPriority
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00145DFA File Offset: 0x00143FFA
		public Area_Home()
		{
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00145E02 File Offset: 0x00144002
		public Area_Home(AreaManager areaManager) : base(areaManager)
		{
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00145E0B File Offset: 0x0014400B
		public override string GetUniqueLoadID()
		{
			return "Area_" + this.ID + "_Home";
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00145E27 File Offset: 0x00144027
		protected override void Set(IntVec3 c, bool val)
		{
			if (base[c] == val)
			{
				return;
			}
			base.Set(c, val);
			base.Map.listerFilthInHomeArea.Notify_HomeAreaChanged(c);
		}
	}
}
