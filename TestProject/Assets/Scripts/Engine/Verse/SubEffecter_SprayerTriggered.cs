using System;

namespace Verse
{
	// Token: 0x02000434 RID: 1076
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06001FF8 RID: 8184 RVA: 0x000C375A File Offset: 0x000C195A
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000C3809 File Offset: 0x000C1A09
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
