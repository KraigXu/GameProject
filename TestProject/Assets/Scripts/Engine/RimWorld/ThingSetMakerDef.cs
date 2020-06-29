using System;
using Verse;

namespace RimWorld
{
	
	public class ThingSetMakerDef : Def
	{
		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.root.ResolveReferences();
		}

		
		public ThingSetMaker root;

		
		public ThingSetMakerParams debugParams;
	}
}
