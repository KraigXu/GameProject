using System;

namespace Verse.Noise
{
	// Token: 0x0200049E RID: 1182
	public class CurveSimple : ModuleBase
	{
		// Token: 0x060022EA RID: 8938 RVA: 0x000D3B7E File Offset: 0x000D1D7E
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000D3B97 File Offset: 0x000D1D97
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04001541 RID: 5441
		private SimpleCurve curve;
	}
}
