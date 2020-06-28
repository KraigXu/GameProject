using System;

namespace Verse.Noise
{
	// Token: 0x02000499 RID: 1177
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x060022DC RID: 8924 RVA: 0x000D397C File Offset: 0x000D1B7C
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
