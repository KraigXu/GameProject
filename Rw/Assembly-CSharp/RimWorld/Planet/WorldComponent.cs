using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001207 RID: 4615
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x06006AC8 RID: 27336 RVA: 0x00253A75 File Offset: 0x00251C75
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x06006ACB RID: 27339 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x040042B1 RID: 17073
		public World world;
	}
}
