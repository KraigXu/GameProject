using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107E RID: 4222
	public class HediffComp_RoyalImplant : HediffComp
	{
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06006432 RID: 25650 RVA: 0x0022B5DD File Offset: 0x002297DD
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x0022B5EC File Offset: 0x002297EC
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = implant as Hediff_ImplantWithLevel;
			if (hediff_ImplantWithLevel != null)
			{
				return hediff_ImplantWithLevel.level;
			}
			return 0;
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x0022B60B File Offset: 0x0022980B
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x0022B638 File Offset: 0x00229838
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, -1);
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (!Rand.Chance(detectionChance))
			{
				return;
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.IsViolatingRulesOf(faction, violationSourceLevel))
				{
					faction.Notify_RoyalThingUseViolation(this.parent.def, base.Pawn, violationSourceName, detectionChance, violationSourceLevel);
				}
			}
		}
	}
}
