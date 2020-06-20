using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class LordToil_Siege : LordToil
	{
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x0011A23D File Offset: 0x0011843D
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.siegeCenter;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x0011A24A File Offset: 0x0011844A
		private LordToilData_Siege Data
		{
			get
			{
				return (LordToilData_Siege)this.data;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x0011A257 File Offset: 0x00118457
		private IEnumerable<Frame> Frames
		{
			get
			{
				LordToilData_Siege data = this.Data;
				float radSquared = (data.baseRadius + 10f) * (data.baseRadius + 10f);
				List<Thing> framesList = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
				if (framesList.Count == 0)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < framesList.Count; i = num + 1)
				{
					Frame frame = (Frame)framesList[i];
					if (frame.Faction == this.lord.faction && (float)(frame.Position - data.siegeCenter).LengthHorizontalSquared < radSquared)
					{
						yield return frame;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x0011A267 File Offset: 0x00118467
		public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
		{
			this.data = new LordToilData_Siege();
			this.Data.siegeCenter = siegeCenter;
			this.Data.blueprintPoints = blueprintPoints;
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x0011A2A0 File Offset: 0x001184A0
		public override void Init()
		{
			base.Init();
			LordToilData_Siege data = this.Data;
			data.baseRadius = Mathf.InverseLerp(14f, 25f, (float)this.lord.ownedPawns.Count / 50f);
			data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
			List<Thing> list = new List<Thing>();
			foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, base.Map, this.lord.faction, data.blueprintPoints))
			{
				data.blueprints.Add(blueprint_Build);
				using (List<ThingDefCountClass>.Enumerator enumerator2 = blueprint_Build.MaterialsNeeded().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDefCountClass cost = enumerator2.Current;
						Thing thing = list.FirstOrDefault((Thing t) => t.def == cost.thingDef);
						if (thing != null)
						{
							thing.stackCount += cost.count;
						}
						else
						{
							Thing thing2 = ThingMaker.MakeThing(cost.thingDef, null);
							thing2.stackCount = cost.count;
							list.Add(thing2);
						}
					}
				}
				ThingDef thingDef = blueprint_Build.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ThingDef thingDef2 = TurretGunUtility.TryFindRandomShellDef(thingDef, false, true, this.lord.faction.def.techLevel, false, 250f);
					if (thingDef2 != null)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef2, null);
						thing3.stackCount = 5;
						list.Add(thing3);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].stackCount = Mathf.CeilToInt((float)list[i].stackCount * Rand.Range(1f, 1.2f));
			}
			List<List<Thing>> list2 = new List<List<Thing>>();
			for (int j = 0; j < list.Count; j++)
			{
				while (list[j].stackCount > list[j].def.stackLimit)
				{
					int num = Mathf.CeilToInt((float)list[j].def.stackLimit * Rand.Range(0.9f, 0.999f));
					Thing thing4 = ThingMaker.MakeThing(list[j].def, null);
					thing4.stackCount = num;
					list[j].stackCount -= num;
					list.Add(thing4);
				}
			}
			List<Thing> list3 = new List<Thing>();
			for (int k = 0; k < list.Count; k++)
			{
				list3.Add(list[k]);
				if (k % 2 == 1 || k == list.Count - 1)
				{
					list2.Add(list3);
					list3 = new List<Thing>();
				}
			}
			List<Thing> list4 = new List<Thing>();
			int num2 = Mathf.RoundToInt(LordToil_Siege.MealCountRangePerRaider.RandomInRange * (float)this.lord.ownedPawns.Count);
			for (int l = 0; l < num2; l++)
			{
				Thing item = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null);
				list4.Add(item);
			}
			list2.Add(list4);
			DropPodUtility.DropThingGroupsNear(data.siegeCenter, base.Map, list2, 110, false, false, true, true);
			data.desiredBuilderFraction = LordToil_Siege.BuilderCountFraction.RandomInRange;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x0011A658 File Offset: 0x00118858
		public override void UpdateAllDuties()
		{
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil < 450)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					this.SetAsDefender(this.lord.ownedPawns[i]);
				}
				return;
			}
			this.rememberedDuties.Clear();
			int num = Mathf.RoundToInt((float)this.lord.ownedPawns.Count * data.desiredBuilderFraction);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
			where b.def.hasInteractionCell && b.Faction == this.lord.faction && b.Position.InHorDistOf(this.FlagLoc, data.baseRadius)
			select b).Count<Thing>();
			if (num < num2)
			{
				num = num2;
			}
			int num3 = 0;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = this.lord.ownedPawns[j];
				if (pawn.mindState.duty.def == DutyDefOf.Build)
				{
					this.rememberedDuties.Add(pawn, DutyDefOf.Build);
					this.SetAsBuilder(pawn);
					num3++;
				}
			}
			int num4 = num - num3;
			Func<Pawn, bool> <>9__1;
			for (int k = 0; k < num4; k++)
			{
				IEnumerable<Pawn> ownedPawns = this.lord.ownedPawns;
				Func<Pawn, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((Pawn pa) => !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)));
				}
				Pawn pawn2;
				if (ownedPawns.Where(predicate).TryRandomElement(out pawn2))
				{
					this.rememberedDuties.Add(pawn2, DutyDefOf.Build);
					this.SetAsBuilder(pawn2);
					num3++;
				}
			}
			for (int l = 0; l < this.lord.ownedPawns.Count; l++)
			{
				Pawn pawn3 = this.lord.ownedPawns[l];
				if (!this.rememberedDuties.ContainsKey(pawn3))
				{
					this.SetAsDefender(pawn3);
					this.rememberedDuties.Add(pawn3, DutyDefOf.Defend);
				}
			}
			if (num3 == 0)
			{
				this.lord.ReceiveMemo("NoBuilders");
				return;
			}
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x0011A876 File Offset: 0x00118A76
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.UpdateAllDuties();
			base.Notify_PawnLost(victim, cond);
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x0011A886 File Offset: 0x00118A86
		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
			if (frame.Faction == this.lord.faction && newBlueprint != null)
			{
				this.Data.blueprints.Add(newBlueprint);
			}
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x0011A8B8 File Offset: 0x00118AB8
		private bool CanBeBuilder(Pawn p)
		{
			return !p.WorkTypeIsDisabled(WorkTypeDefOf.Construction) && !p.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter);
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x0011A8D8 File Offset: 0x00118AD8
		private void SetAsBuilder(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Build, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
			int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite, ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
			p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
			p.workSettings.EnableAndInitialize();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef == WorkTypeDefOf.Construction)
				{
					p.workSettings.SetPriority(workTypeDef, 1);
				}
				else
				{
					p.workSettings.Disable(workTypeDef);
				}
			}
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x0011A9A8 File Offset: 0x00118BA8
		private void SetAsDefender(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x0011A9F8 File Offset: 0x00118BF8
		public override void LordToilTick()
		{
			base.LordToilTick();
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil == 450)
			{
				this.lord.CurLordToil.UpdateAllDuties();
			}
			if (this.lord.ticksInToil > 450 && this.lord.ticksInToil % 500 == 0)
			{
				this.UpdateAllDuties();
			}
			if (Find.TickManager.TicksGame % 500 == 0)
			{
				if (!(from frame in this.Frames
				where !frame.Destroyed
				select frame).Any<Frame>())
				{
					if (!(from blue in data.blueprints
					where !blue.Destroyed
					select blue).Any<Blueprint>() && !base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Thing b) => b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery")))
					{
						this.lord.ReceiveMemo("NoArtillery");
						return;
					}
				}
				int num = GenRadial.NumCellsInRadius(20f);
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = data.siegeCenter + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.IsShell)
							{
								num2 += thingList[j].stackCount;
							}
							if (thingList[j].def == ThingDefOf.MealSurvivalPack)
							{
								num3 += thingList[j].stackCount;
							}
						}
					}
				}
				if (num2 < 4)
				{
					ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(ThingDefOf.Turret_Mortar, false, true, this.lord.faction.def.techLevel, false, 250f);
					if (thingDef != null)
					{
						this.DropSupplies(thingDef, 6);
					}
				}
				if (num3 < 5)
				{
					this.DropSupplies(ThingDefOf.MealSurvivalPack, 12);
				}
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x0011AC18 File Offset: 0x00118E18
		private void DropSupplies(ThingDef thingDef, int count)
		{
			List<Thing> list = new List<Thing>();
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			list.Add(thing);
			DropPodUtility.DropThingsNear(this.Data.siegeCenter, base.Map, list, 110, false, false, true, true);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x0011AC60 File Offset: 0x00118E60
		public override void Cleanup()
		{
			LordToilData_Siege data = this.Data;
			data.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			for (int i = 0; i < data.blueprints.Count; i++)
			{
				data.blueprints[i].Destroy(DestroyMode.Cancel);
			}
			foreach (Frame frame in this.Frames.ToList<Frame>())
			{
				frame.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x04001B62 RID: 7010
		public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();

		// Token: 0x04001B63 RID: 7011
		private const float BaseRadiusMin = 14f;

		// Token: 0x04001B64 RID: 7012
		private const float BaseRadiusMax = 25f;

		// Token: 0x04001B65 RID: 7013
		private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);

		// Token: 0x04001B66 RID: 7014
		private const int StartBuildingDelay = 450;

		// Token: 0x04001B67 RID: 7015
		private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);

		// Token: 0x04001B68 RID: 7016
		private const float FractionLossesToAssault = 0.4f;

		// Token: 0x04001B69 RID: 7017
		private const int InitalShellsPerCannon = 5;

		// Token: 0x04001B6A RID: 7018
		private const int ReplenishAtShells = 4;

		// Token: 0x04001B6B RID: 7019
		private const int ShellReplenishCount = 6;

		// Token: 0x04001B6C RID: 7020
		private const int ReplenishAtMeals = 5;

		// Token: 0x04001B6D RID: 7021
		private const int MealReplenishCount = 12;
	}
}
