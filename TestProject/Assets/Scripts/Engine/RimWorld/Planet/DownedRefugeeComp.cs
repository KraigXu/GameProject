using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		
		// (get) Token: 0x06006ED0 RID: 28368 RVA: 0x0026A60A File Offset: 0x0026880A
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			if (this.pawn.Any)
			{
				if (!this.pawn[0].Dead)
				{
					if (this.pawn[0].relations != null)
					{
						this.pawn[0].relations.Notify_FailedRescueQuest();
					}
					HealthUtility.HealNonPermanentInjuriesAndRestoreLegs(this.pawn[0]);
				}
				this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Refugee".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
