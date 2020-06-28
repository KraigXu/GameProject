using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001033 RID: 4147
	public abstract class Verb_MeleeAttack : Verb
	{
		// Token: 0x06006329 RID: 25385 RVA: 0x00227470 File Offset: 0x00225670
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			if (!casterPawn.Spawned)
			{
				return false;
			}
			if (casterPawn.stances.FullBodyBusy)
			{
				return false;
			}
			Thing thing = this.currentTarget.Thing;
			if (!this.CanHitTarget(thing))
			{
				Log.Warning(string.Concat(new object[]
				{
					casterPawn,
					" meleed ",
					thing,
					" from out of melee position."
				}), false);
			}
			casterPawn.rotationTracker.Face(thing.DrawPos);
			if (!this.IsTargetImmobile(this.currentTarget) && casterPawn.skills != null)
			{
				casterPawn.skills.Learn(SkillDefOf.Melee, 200f * this.verbProps.AdjustedFullCycleTime(this, casterPawn), false);
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && !pawn.Dead && (casterPawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.MentalStateDef != MentalStateDefOf.SocialFighting))
			{
				pawn.mindState.meleeThreat = casterPawn;
				pawn.mindState.lastMeleeThreatHarmTick = Find.TickManager.TicksGame;
			}
			Map map = thing.Map;
			Vector3 drawPos = thing.DrawPos;
			SoundDef soundDef;
			bool result;
			if (Rand.Chance(this.GetNonMissChance(thing)))
			{
				if (!Rand.Chance(this.GetDodgeChance(thing)))
				{
					if (thing.def.category == ThingCategory.Building)
					{
						soundDef = this.SoundHitBuilding();
					}
					else
					{
						soundDef = this.SoundHitPawn();
					}
					if (this.verbProps.impactMote != null)
					{
						MoteMaker.MakeStaticMote(drawPos, map, this.verbProps.impactMote, 1f);
					}
					BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesHit, true);
					result = true;
					DamageWorker.DamageResult damageResult = this.ApplyMeleeDamageToTarget(this.currentTarget);
					if (damageResult.stunned && damageResult.parts.NullOrEmpty<BodyPartRecord>())
					{
						Find.BattleLog.RemoveEntry(battleLogEntry_MeleeCombat);
					}
					else
					{
						damageResult.AssociateWithLog(battleLogEntry_MeleeCombat);
						if (damageResult.deflected)
						{
							battleLogEntry_MeleeCombat.RuleDef = this.maneuver.combatLogRulesDeflect;
							battleLogEntry_MeleeCombat.alwaysShowInCompact = false;
						}
					}
				}
				else
				{
					result = false;
					soundDef = this.SoundDodge(thing);
					MoteMaker.ThrowText(drawPos, map, "TextMote_Dodge".Translate(), 1.9f);
					this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesDodge, false);
				}
			}
			else
			{
				result = false;
				soundDef = this.SoundMiss();
				this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesMiss, false);
			}
			soundDef.PlayOneShot(new TargetInfo(thing.Position, map, false));
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			if (pawn != null && !pawn.Dead && pawn.Spawned)
			{
				pawn.stances.StaggerFor(95);
			}
			if (casterPawn.Spawned)
			{
				casterPawn.rotationTracker.FaceCell(thing.Position);
			}
			if (casterPawn.caller != null)
			{
				casterPawn.caller.Notify_DidMeleeAttack();
			}
			return result;
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x0022778C File Offset: 0x0022598C
		public BattleLogEntry_MeleeCombat CreateCombatLog(Func<ManeuverDef, RulePackDef> rulePackGetter, bool alwaysShow)
		{
			if (this.maneuver == null)
			{
				return null;
			}
			if (this.tool == null)
			{
				return null;
			}
			BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackGetter(this.maneuver), alwaysShow, this.CasterPawn, this.currentTarget.Thing, base.ImplementOwnerType, this.tool.labelUsedInLogging ? this.tool.label : "", (base.EquipmentSource == null) ? null : base.EquipmentSource.def, (base.HediffCompSource == null) ? null : base.HediffCompSource.Def, this.maneuver.logEntryDef);
			Find.BattleLog.Add(battleLogEntry_MeleeCombat);
			return battleLogEntry_MeleeCombat;
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x00227839 File Offset: 0x00225A39
		private float GetNonMissChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 1f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 1f;
			}
			return this.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true);
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x0022786C File Offset: 0x00225A6C
		private float GetDodgeChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 0f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 0f;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.verb != null && !stance_Busy.verb.verbProps.IsMeleeAttack)
			{
				return 0f;
			}
			return pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true);
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x002278EC File Offset: 0x00225AEC
		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x0600632E RID: 25390
		protected abstract DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target);

		// Token: 0x0600632F RID: 25391 RVA: 0x00227928 File Offset: 0x00225B28
		private SoundDef SoundHitPawn()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitPawn;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitPawn;
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x00227A60 File Offset: 0x00225C60
		private SoundDef SoundHitBuilding()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitBuilding;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitBuilding;
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x00227B98 File Offset: 0x00225D98
		private SoundDef SoundMiss()
		{
			if (this.CasterPawn != null)
			{
				if (this.tool != null && !this.tool.soundMeleeMiss.NullOrUndefined())
				{
					return this.tool.soundMeleeMiss;
				}
				if (!this.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined())
				{
					return this.CasterPawn.def.race.soundMeleeMiss;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_Miss;
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x00227C0A File Offset: 0x00225E0A
		private SoundDef SoundDodge(Thing target)
		{
			if (target.def.race != null && target.def.race.soundMeleeDodge != null)
			{
				return target.def.race.soundMeleeDodge;
			}
			return this.SoundMiss();
		}

		// Token: 0x04003C4F RID: 15439
		private const int TargetCooldown = 50;
	}
}
