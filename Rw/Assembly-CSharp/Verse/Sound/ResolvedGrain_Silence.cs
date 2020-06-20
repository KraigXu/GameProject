using System;

namespace Verse.Sound
{
	// Token: 0x020004D2 RID: 1234
	public class ResolvedGrain_Silence : ResolvedGrain
	{
		// Token: 0x06002437 RID: 9271 RVA: 0x000D86C4 File Offset: 0x000D68C4
		public ResolvedGrain_Silence(AudioGrain_Silence sourceGrain)
		{
			this.sourceGrain = sourceGrain;
			this.duration = sourceGrain.durationRange.RandomInRange;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000D86E4 File Offset: 0x000D68E4
		public override string ToString()
		{
			return "Silence";
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000D86EC File Offset: 0x000D68EC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			ResolvedGrain_Silence resolvedGrain_Silence = obj as ResolvedGrain_Silence;
			return resolvedGrain_Silence != null && resolvedGrain_Silence.sourceGrain == this.sourceGrain;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000D8718 File Offset: 0x000D6918
		public override int GetHashCode()
		{
			return this.sourceGrain.GetHashCode();
		}

		// Token: 0x040015DA RID: 5594
		public AudioGrain_Silence sourceGrain;
	}
}
