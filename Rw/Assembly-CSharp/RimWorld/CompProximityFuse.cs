using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D3C RID: 3388
	public class CompProximityFuse : ThingComp
	{
		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005256 RID: 21078 RVA: 0x001B84DB File Offset: 0x001B66DB
		public CompProperties_ProximityFuse Props
		{
			get
			{
				return (CompProperties_ProximityFuse)this.props;
			}
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001B84E8 File Offset: 0x001B66E8
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x001B8504 File Offset: 0x001B6704
		public override void CompTickRare()
		{
			if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForDef(this.Props.target), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.radius, null, null, 0, -1, false, RegionType.Set_Passable, false) != null)
			{
				this.parent.GetComp<CompExplosive>().StartWick(null);
			}
		}
	}
}
