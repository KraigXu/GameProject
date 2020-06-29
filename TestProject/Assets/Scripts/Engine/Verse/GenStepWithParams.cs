using System;

namespace Verse
{
	
	public struct GenStepWithParams
	{
		
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		
		public GenStepDef def;

		
		public GenStepParams parms;
	}
}
