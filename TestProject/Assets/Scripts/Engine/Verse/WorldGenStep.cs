using System;

namespace Verse
{
	
	public abstract class WorldGenStep
	{
		
		// (get) Token: 0x060006FA RID: 1786
		public abstract int SeedPart { get; }

		
		public abstract void GenerateFresh(string seed);

		
		public virtual void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFresh(seed);
		}

		
		public virtual void GenerateFromScribe(string seed)
		{
		}

		
		public WorldGenStepDef def;
	}
}
