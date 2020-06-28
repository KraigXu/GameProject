using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200127D RID: 4733
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x0026AA85 File Offset: 0x00268C85
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06006EFA RID: 28410 RVA: 0x0026AA8C File Offset: 0x00268C8C
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06006EFB RID: 28411 RVA: 0x0026AA9A File Offset: 0x00268C9A
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
