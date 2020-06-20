using System;

namespace Verse
{
	// Token: 0x0200004F RID: 79
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x00013409 File Offset: 0x00011609
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001346C File Offset: 0x0001166C
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
