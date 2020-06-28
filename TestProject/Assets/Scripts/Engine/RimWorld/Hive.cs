using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA1 RID: 3233
	public class Hive : ThingWithComps, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06004E28 RID: 20008 RVA: 0x001A481E File Offset: 0x001A2A1E
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06004E29 RID: 20009 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06004E2A RID: 20010 RVA: 0x001A4826 File Offset: 0x001A2A26
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06004E2B RID: 20011 RVA: 0x001A482D File Offset: 0x001A2A2D
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06004E2C RID: 20012 RVA: 0x001A4834 File Offset: 0x001A2A34
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x001A483C File Offset: 0x001A2A3C
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x001A4868 File Offset: 0x001A2A68
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x001A48A1 File Offset: 0x001A2AA1
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x001A48BF File Offset: 0x001A2ABF
		public override void Tick()
		{
			base.Tick();
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x001A48FC File Offset: 0x001A2AFC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(Hive.MemoDeSpawned);
			}
			HiveUtility.Notify_HiveDespawned(this, map);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x001A494C File Offset: 0x001A2B4C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.questTags.NullOrEmpty<string>())
			{
				bool flag = false;
				List<Thing> list = base.Map.listerThings.ThingsOfDef(this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Hive hive;
					if ((hive = (list[i] as Hive)) != null && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty<string>() && QuestUtility.AnyMatchingTags(hive.questTags, this.questTags))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "AllHivesDestroyed");
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x001A49F4 File Offset: 0x001A2BF4
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(Hive.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x001A4A88 File Offset: 0x001A2C88
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && (dinfo == null || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
			{
				List<Lord> lords = base.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDestroyedNonRoofCollapse);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x001A4AF8 File Offset: 0x001A2CF8
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.PawnSpawner.spawnedPawns.Count > 0)
			{
				if (this.PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x001A4B5C File Offset: 0x001A2D5C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x001A4B6C File Offset: 0x001A2D6C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				bool flag = false;
				Scribe_Values.Look<bool>(ref flag, "active", false, false);
				if (flag)
				{
					this.CompDormant.WakeUp();
				}
			}
		}

		// Token: 0x04002BE4 RID: 11236
		public const int PawnSpawnRadius = 2;

		// Token: 0x04002BE5 RID: 11237
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x04002BE6 RID: 11238
		public const float InitialPawnsPoints = 200f;

		// Token: 0x04002BE7 RID: 11239
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x04002BE8 RID: 11240
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x04002BE9 RID: 11241
		public static readonly string MemoDeSpawned = "HiveDeSpawned";

		// Token: 0x04002BEA RID: 11242
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x04002BEB RID: 11243
		public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";
	}
}
