using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001212 RID: 4626
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0025563C File Offset: 0x0025383C
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06006AFE RID: 27390 RVA: 0x00255643 File Offset: 0x00253843
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06006AFF RID: 27391 RVA: 0x0025564F File Offset: 0x0025384F
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06006B00 RID: 27392 RVA: 0x00255658 File Offset: 0x00253858
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
