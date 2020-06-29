using System;

namespace Verse
{
	
	public class WorldGenStepDef : Def
	{
		
		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}

		
		public float order;

		
		public WorldGenStep worldGenStep;
	}
}
