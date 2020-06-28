using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC9 RID: 2761
	public class CompAbilityEffect_Flashstorm : CompAbilityEffect
	{
		// Token: 0x06004185 RID: 16773 RVA: 0x0015E6CC File Offset: 0x0015C8CC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			Thing conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, -1);
			gameCondition_Flashstorm.centerLocation = target.Cell.ToIntVec2;
			gameCondition_Flashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.parent.def.EffectRadius), Mathf.RoundToInt(this.parent.def.EffectRadius));
			gameCondition_Flashstorm.Duration = Mathf.RoundToInt((float)this.parent.def.EffectDuration.SecondsToTicks());
			gameCondition_Flashstorm.suppressEndMessage = true;
			gameCondition_Flashstorm.initialStrikeDelay = new IntRange(60, 180);
			gameCondition_Flashstorm.conditionCauser = conditionCauser;
			gameCondition_Flashstorm.ambientSound = true;
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			this.ApplyGoodwillImpact(target, gameCondition_Flashstorm.AreaRadius);
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0015E7D0 File Offset: 0x0015C9D0
		private void ApplyGoodwillImpact(LocalTargetInfo target, int radius)
		{
			this.affectedFactionCache.Clear();
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, this.parent.pawn.Map, (float)radius, true))
			{
				Pawn pawn;
				if ((pawn = (thing as Pawn)) != null && thing.Faction != null && thing.Faction != this.parent.pawn.Faction && !thing.Faction.HostileTo(this.parent.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) && (base.Props.applyGoodwillImpactToLodgers || !pawn.IsQuestLodger()))
				{
					this.affectedFactionCache.Add(thing.Faction);
					thing.Faction.TryAffectGoodwillWith(this.parent.pawn.Faction, base.Props.goodwillImpact, true, true, "GoodwillChangedReason_UsedAbility".Translate(this.parent.def.LabelCap, pawn.LabelShort), new GlobalTargetInfo?(pawn));
				}
			}
			this.affectedFactionCache.Clear();
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0015E94C File Offset: 0x0015CB4C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Roofed(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityRoofed".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x040025FD RID: 9725
		private HashSet<Faction> affectedFactionCache = new HashSet<Faction>();
	}
}
