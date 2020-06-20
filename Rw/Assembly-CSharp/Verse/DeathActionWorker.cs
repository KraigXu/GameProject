using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000098 RID: 152
	public abstract class DeathActionWorker
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0001907E File Offset: 0x0001727E
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004EF RID: 1263
		public abstract void PawnDied(Corpse corpse);
	}
}
