using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldGenStep_Components : WorldGenStep
	{
		
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0025563C File Offset: 0x0025383C
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
