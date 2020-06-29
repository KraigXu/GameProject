using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		
		// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x0026AA85 File Offset: 0x00268C85
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		
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
