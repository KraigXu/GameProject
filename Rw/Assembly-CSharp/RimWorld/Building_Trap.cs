using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C59 RID: 3161
	public abstract class Building_Trap : Building
	{
		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06004B7B RID: 19323 RVA: 0x001972B2 File Offset: 0x001954B2
		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x06004B7C RID: 19324 RVA: 0x001972DB File Offset: 0x001954DB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x0019730B File Offset: 0x0019550B
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x00197340 File Offset: 0x00195540
		public override void Tick()
		{
			if (base.Spawned)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
				for (int j = 0; j < this.touchingPawns.Count; j++)
				{
					Pawn pawn2 = this.touchingPawns[j];
					if (!pawn2.Spawned || pawn2.Position != base.Position)
					{
						this.touchingPawns.Remove(pawn2);
					}
				}
			}
			base.Tick();
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x00197404 File Offset: 0x00195604
		private void CheckSpring(Pawn p)
		{
			if (Rand.Chance(this.SpringChance(p)))
			{
				Map map = base.Map;
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.LabelShort, p).CapitalizeFirst(), "LetterFriendlyTrapSprung".Translate(p.LabelShort, p).CapitalizeFirst(), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, map, false), null, null, null, null);
				}
			}
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x001974B8 File Offset: 0x001956B8
		protected virtual float SpringChance(Pawn p)
		{
			float num = 1f;
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.RaceProps.Animal)
					{
						num = 0.2f;
						num *= this.def.building.trapPeacefulWildAnimalsSpringChanceFactor;
					}
					else
					{
						num = 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num = 0.005f;
				}
				else
				{
					num = 0f;
				}
			}
			num *= this.GetStatValue(StatDefOf.TrapSpringChance, true) * p.GetStatValue(StatDefOf.PawnTrapSpringChance, true);
			return Mathf.Clamp01(num);
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x0019754C File Offset: 0x0019574C
		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction) || (p.RaceProps.Humanlike && p.IsFormingCaravan()) || (p.IsPrisoner && p.guest.ShouldWaitInsteadOfEscaping && base.Faction == p.HostFaction) || (p.Faction == null && p.RaceProps.Humanlike);
		}

		// Token: 0x06004B82 RID: 19330 RVA: 0x00197620 File Offset: 0x00195820
		public override ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		// Token: 0x06004B83 RID: 19331 RVA: 0x00197632 File Offset: 0x00195832
		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		// Token: 0x06004B84 RID: 19332 RVA: 0x00197641 File Offset: 0x00195841
		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		// Token: 0x06004B85 RID: 19333 RVA: 0x0019764C File Offset: 0x0019584C
		public void Spring(Pawn p)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			this.SpringSub(p);
			if (this.def.building.trapDestroyOnSpring)
			{
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				if (spawned)
				{
					this.CheckAutoRebuild(map);
				}
			}
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x0019769C File Offset: 0x0019589C
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Kill(dinfo, exactCulprit);
			if (spawned)
			{
				this.CheckAutoRebuild(map);
			}
		}

		// Token: 0x06004B87 RID: 19335
		protected abstract void SpringSub(Pawn p);

		// Token: 0x06004B88 RID: 19336 RVA: 0x001976C8 File Offset: 0x001958C8
		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, base.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x00197737 File Offset: 0x00195937
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanSetAutoRearm)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoRearm".Translate(),
					defaultDesc = "CommandAutoRearmDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.RearmTrap,
					isActive = (() => this.autoRearm),
					toggleAction = delegate
					{
						this.autoRearm = !this.autoRearm;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04002AAE RID: 10926
		private bool autoRearm;

		// Token: 0x04002AAF RID: 10927
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x04002AB0 RID: 10928
		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		// Token: 0x04002AB1 RID: 10929
		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		// Token: 0x04002AB2 RID: 10930
		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		// Token: 0x04002AB3 RID: 10931
		private const float KnowerSpringChanceFactorOther = 0f;

		// Token: 0x04002AB4 RID: 10932
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x04002AB5 RID: 10933
		private const ushort KnowerPathWalkCost = 40;
	}
}
