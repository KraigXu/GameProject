using System;

namespace Verse
{
	
	public abstract class GenStep
	{
		
		// (get) Token: 0x06000574 RID: 1396
		public abstract int SeedPart { get; }

		
		public abstract void Generate(Map map, GenStepParams parms);

		
		public GenStepDef def;
	}
}
