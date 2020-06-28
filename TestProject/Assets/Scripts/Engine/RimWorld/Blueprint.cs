using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C4A RID: 3146
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004B09 RID: 19209 RVA: 0x00195450 File Offset: 0x00193650
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004B0A RID: 19210
		protected abstract float WorkTotal { get; }

		// Token: 0x06004B0B RID: 19211 RVA: 0x00079A3A File Offset: 0x00077C3A
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
				return;
			}
			base.Comps_PostDraw();
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x00195476 File Offset: 0x00193676
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x00195488 File Offset: 0x00193688
		public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			jobEnded = false;
			if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
			{
				workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				jobEnded = true;
				createdThing = null;
				return false;
			}
			createdThing = this.MakeSolidThing();
			Map map = base.Map;
			GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			if (createdThing.def.CanHaveFaction)
			{
				createdThing.SetFactionDirect(workerPawn.Faction);
			}
			GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x00195522 File Offset: 0x00193722
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x00195538 File Offset: 0x00193738
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x06004B10 RID: 19216
		protected abstract Thing MakeSolidThing();

		// Token: 0x06004B11 RID: 19217
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06004B12 RID: 19218
		public abstract ThingDef EntityToBuildStuff();

		// Token: 0x06004B13 RID: 19219 RVA: 0x00195554 File Offset: 0x00193754
		public Thing BlockingHaulableOnTop()
		{
			if (this.def.entityDefToBuild.passability == Traversability.Standable)
			{
				return null;
			}
			foreach (IntVec3 c in this.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.EverHaulable)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x001955F4 File Offset: 0x001937F4
		public override ushort PathFindCostFor(Pawn p)
		{
			if (base.Faction == null)
			{
				return 0;
			}
			if (this.def.entityDefToBuild is TerrainDef)
			{
				return 0;
			}
			if ((p.Faction == base.Faction || p.HostFaction == base.Faction) && (base.Map.reservationManager.IsReservedByAnyoneOf(this, p.Faction) || (p.HostFaction != null && base.Map.reservationManager.IsReservedByAnyoneOf(this, p.HostFaction))))
			{
				return Frame.AvoidUnderConstructionPathFindCost;
			}
			return 0;
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x00195688 File Offset: 0x00193888
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkTotal.ToStringWorkAmount());
			return stringBuilder.ToString();
		}
	}
}
