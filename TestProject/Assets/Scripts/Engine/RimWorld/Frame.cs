﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible
	{
		
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x00195BA0 File Offset: 0x00193DA0
		public float WorkToBuild
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x00195BBD File Offset: 0x00193DBD
		public float WorkLeft
		{
			get
			{
				return this.WorkToBuild - this.workDone;
			}
		}

		
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x00195BCC File Offset: 0x00193DCC
		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToBuild;
			}
		}

		
		// (get) Token: 0x06004B3A RID: 19258 RVA: 0x00195BDB File Offset: 0x00193DDB
		public override string Label
		{
			get
			{
				return this.LabelEntityToBuild + "FrameLabelExtra".Translate();
			}
		}

		
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x00195BF8 File Offset: 0x00193DF8
		public string LabelEntityToBuild
		{
			get
			{
				string label = this.def.entityDefToBuild.label;
				if (base.Stuff != null)
				{
					return "ThingMadeOfStuffLabel".Translate(base.Stuff.LabelAsStuff, label);
				}
				return label;
			}
		}

		
		// (get) Token: 0x06004B3C RID: 19260 RVA: 0x00195C48 File Offset: 0x00193E48
		public override Color DrawColor
		{
			get
			{
				if (!this.def.MadeFromStuff)
				{
					List<ThingDefCountClass> costList = this.def.entityDefToBuild.costList;
					if (costList != null)
					{
						for (int i = 0; i < costList.Count; i++)
						{
							ThingDef thingDef = costList[i].thingDef;
							if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
							{
								return this.def.GetColorForStuff(thingDef);
							}
						}
					}
					return new Color(0.6f, 0.6f, 0.6f);
				}
				return base.DrawColor;
			}
		}

		
		// (get) Token: 0x06004B3D RID: 19261 RVA: 0x00195CDC File Offset: 0x00193EDC
		public EffecterDef ConstructionEffect
		{
			get
			{
				if (base.Stuff != null && base.Stuff.stuffProps.constructEffect != null)
				{
					return base.Stuff.stuffProps.constructEffect;
				}
				if (this.def.entityDefToBuild.constructEffect != null)
				{
					return this.def.entityDefToBuild.constructEffect;
				}
				return EffecterDefOf.ConstructMetal;
			}
		}

		
		// (get) Token: 0x06004B3E RID: 19262 RVA: 0x00195D3C File Offset: 0x00193F3C
		private Material CornerMat
		{
			get
			{
				if (this.cachedCornerMat == null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		
		// (get) Token: 0x06004B3F RID: 19263 RVA: 0x00195D6D File Offset: 0x00193F6D
		private Material TileMat
		{
			get
			{
				if (this.cachedTileMat == null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		
		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[]
			{
				this
			});
		}

		
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (spawned)
			{
				ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def.entityDefToBuild);
			}
		}

		
		public ThingDef EntityToBuildStuff()
		{
			return base.Stuff;
		}

		
		public List<ThingDefCountClass> MaterialsNeeded()
		{
			this.cachedMaterialsNeeded.Clear();
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
				int num2 = thingDefCountClass.count - num;
				if (num2 > 0)
				{
					this.cachedMaterialsNeeded.Add(new ThingDefCountClass(thingDefCountClass.thingDef, num2));
				}
			}
			return this.cachedMaterialsNeeded;
		}

		
		public void CompleteConstruction(Pawn worker)
		{
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true) > 150f && this.def.entityDefToBuild is ThingDef && ((ThingDef)this.def.entityDefToBuild).category == ThingCategory.Building)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			ThingDef thingDef = this.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef != null)
			{
				thing = ThingMaker.MakeThing(thingDef, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction);
					compQuality.SetQuality(q, ArtGenerationContext.Colony);
					QualityUtility.SendCraftNotification(thing, worker);
				}
				CompArt compArt = thing.TryGetComp<CompArt>();
				if (compArt != null)
				{
					if (compQuality == null)
					{
						compArt.InitializeArt(ArtGenerationContext.Colony);
					}
					compArt.JustCreatedBy(worker);
				}
				thing.HitPoints = Mathf.CeilToInt((float)this.HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
			}
			else
			{
				map.terrainGrid.SetTerrain(base.Position, (TerrainDef)this.def.entityDefToBuild);
				FilthMaker.RemoveAllFilth(base.Position, map);
			}
			worker.records.Increment(RecordDefOf.ThingsConstructed);
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true) >= 9500f)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, new object[]
				{
					worker,
					thing.def
				});
			}
		}

		
		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (this.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(this.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToBuild > 1400f)
			{
				Messages.Message("MessageConstructionFailed".Translate(this.LabelEntityToBuild, worker.LabelShort, worker.Named("WORKER")), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		
		public override void Draw()
		{
			Vector2 vector = new Vector2((float)this.def.size.x, (float)this.def.size.z);
			vector.x *= 1.15f;
			vector.y *= 1.15f;
			Vector3 s = new Vector3(vector.x, 1f, vector.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.DrawPos, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z) * 0.38f;
				IntVec3 intVec = default(IntVec3);
				if (i == 0)
				{
					intVec = new IntVec3(-1, 0, -1);
				}
				else if (i == 1)
				{
					intVec = new IntVec3(-1, 0, 1);
				}
				else if (i == 2)
				{
					intVec = new IntVec3(1, 0, 1);
				}
				else if (i == 3)
				{
					intVec = new IntVec3(1, 0, -1);
				}
				Vector3 b = default(Vector3);
				b.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num2 / 2f);
				b.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num2 / 2f);
				Vector3 s2 = new Vector3(num2, 1f, num2);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(this.DrawPos + Vector3.up * 0.03f + b, new Rot4(i).AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			int num3 = Mathf.CeilToInt((this.PercentComplete - 0f) / 1f * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num3; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + this.DrawPos;
				a.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
				a.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, base.Stuff);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass need = list[i];
				int num = need.count;
				IEnumerable<ThingDefCountClass> source = this.MaterialsNeeded();
				Func<ThingDefCountClass, bool> predicate;
				
				if ((predicate ) == null)
				{
					predicate = (9__0 = ((ThingDefCountClass needed) => needed.thingDef == need.thingDef));
				}
				foreach (ThingDefCountClass thingDefCountClass in source.Where(predicate))
				{
					num -= thingDefCountClass.count;
				}
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					need.thingDef.LabelCap + ": ",
					num,
					" / ",
					need.count
				}));
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			return stringBuilder.ToString();
		}

		
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
			if (p.Faction == base.Faction || p.HostFaction == base.Faction)
			{
				return Frame.AvoidUnderConstructionPathFindCost;
			}
			return 0;
		}

		
		public ThingOwner resourceContainer;

		
		public float workDone;

		
		private Material cachedCornerMat;

		
		private Material cachedTileMat;

		
		protected const float UnderfieldOverdrawFactor = 1.15f;

		
		protected const float CenterOverdrawFactor = 0.5f;

		
		private const int LongConstructionProjectThreshold = 9500;

		
		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		
		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		
		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);

		
		[TweakValue("Pathfinding", 0f, 1000f)]
		public static ushort AvoidUnderConstructionPathFindCost = 800;

		
		private List<ThingDefCountClass> cachedMaterialsNeeded = new List<ThingDefCountClass>();
	}
}
