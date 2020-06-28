using System;

namespace Verse.Sound
{
	// Token: 0x0200050E RID: 1294
	public class SustainerScopeFader
	{
		// Token: 0x06002511 RID: 9489 RVA: 0x000DBE54 File Offset: 0x000DA054
		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent + 0.05f;
				this.inScopePercent = num;
				if (this.inScopePercent > 1f)
				{
					this.inScopePercent = 1f;
					return;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.001f)
				{
					this.inScopePercent = 0f;
				}
			}
		}

		// Token: 0x04001682 RID: 5762
		public bool inScope = true;

		// Token: 0x04001683 RID: 5763
		public float inScopePercent = 1f;

		// Token: 0x04001684 RID: 5764
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x04001685 RID: 5765
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
