using System;

namespace Verse
{
	
	public abstract class GenStep
	{
		
		
		public abstract int SeedPart { get; }

		
		public abstract void Generate(Map map, GenStepParams parms);

		
		public GenStepDef def;
	}
}
