﻿using System;

namespace Verse
{
	
	public abstract class WorldGenStep
	{
		
		
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
