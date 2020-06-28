using System;

namespace Verse
{
	// Token: 0x020000B4 RID: 180
	public struct GenStepWithParams
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0001B3E2 File Offset: 0x000195E2
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x040003AA RID: 938
		public GenStepDef def;

		// Token: 0x040003AB RID: 939
		public GenStepParams parms;
	}
}
