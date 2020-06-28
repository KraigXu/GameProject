using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200042C RID: 1068
	public class Effecter
	{
		// Token: 0x06001FE3 RID: 8163 RVA: 0x000C31F0 File Offset: 0x000C13F0
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000C3248 File Offset: 0x000C1448
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000C3280 File Offset: 0x000C1480
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000C32B8 File Offset: 0x000C14B8
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x040013AB RID: 5035
		public EffecterDef def;

		// Token: 0x040013AC RID: 5036
		public List<SubEffecter> children = new List<SubEffecter>();
	}
}
