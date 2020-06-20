using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C3E RID: 3134
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x06004ACA RID: 19146 RVA: 0x00194490 File Offset: 0x00192690
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x06004ACB RID: 19147 RVA: 0x00194590 File Offset: 0x00192790
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
