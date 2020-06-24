using System;
using RimWorld;

namespace Spirit
{
	public class GenStepDef : Def
	{
		public SitePartDef linkWithSite;
		public float order;

		public GenStep genStep;
		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}

		
	}
}