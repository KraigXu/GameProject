using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000430 RID: 1072
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06001FF0 RID: 8176 RVA: 0x000132A9 File Offset: 0x000114A9
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000C33B4 File Offset: 0x000C15B4
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
