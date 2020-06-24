using System;

namespace Verse.Noise
{
	// Token: 0x0200049A RID: 1178
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x060022DE RID: 8926 RVA: 0x000D397C File Offset: 0x000D1B7C
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000D39D1 File Offset: 0x000D1BD1
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
