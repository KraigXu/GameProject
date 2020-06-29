using System;
using RimWorld;

namespace Verse
{
	
	public class GenStepDef : Def
	{
		
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		
		public SitePartDef linkWithSite;

		
		public float order;

		
		public GenStep genStep;
	}
}
