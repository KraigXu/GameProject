using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001276 RID: 4726
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x06006ED0 RID: 28368 RVA: 0x0026A60A File Offset: 0x0026880A
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x0026A614 File Offset: 0x00268814
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

		// Token: 0x06006ED2 RID: 28370 RVA: 0x0026A687 File Offset: 0x00268887
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
