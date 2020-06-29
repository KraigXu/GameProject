using System;
using Verse;

namespace RimWorld
{
	
	public class HediffComp_RoyalImplant : HediffComp
	{
		
		// (get) Token: 0x06006432 RID: 25650 RVA: 0x0022B5DD File Offset: 0x002297DD
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = implant as Hediff_ImplantWithLevel;
			if (hediff_ImplantWithLevel != null)
			{
				return hediff_ImplantWithLevel.level;
			}
			return 0;
		}

		
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		
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
