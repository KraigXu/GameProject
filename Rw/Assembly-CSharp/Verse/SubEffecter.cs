using System;

namespace Verse
{
	// Token: 0x0200042D RID: 1069
	public class SubEffecter
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x000C32EC File Offset: 0x000C14EC
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SubCleanup()
		{
		}

		// Token: 0x040013AD RID: 5037
		public Effecter parent;

		// Token: 0x040013AE RID: 5038
		public SubEffecterDef def;
	}
}
