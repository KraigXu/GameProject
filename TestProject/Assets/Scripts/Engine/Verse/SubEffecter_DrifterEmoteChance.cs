using System;

namespace Verse
{
	// Token: 0x0200004E RID: 78
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00013409 File Offset: 0x00011609
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013444 File Offset: 0x00011644
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float chancePerTick = this.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A);
			}
		}
	}
}
