using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BC RID: 2492
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x0013A1CF File Offset: 0x001383CF
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0013A1DE File Offset: 0x001383DE
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x0013A1F8 File Offset: 0x001383F8
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				for (int i = 0; i < affectedMaps.Count; i++)
				{
					this.DoPawnsToxicDamage(affectedMaps[i]);
				}
			}
			for (int j = 0; j < this.overlays.Count; j++)
			{
				for (int k = 0; k < affectedMaps.Count; k++)
				{
					this.overlays[j].TickOverlay(affectedMaps[k]);
				}
			}
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0013A27C File Offset: 0x0013847C
		private void DoPawnsToxicDamage(Map map)
		{
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				GameCondition_ToxicFallout.DoPawnToxicDamage(allPawnsSpawned[i]);
			}
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0013A2B4 File Offset: 0x001384B4
		public static void DoPawnToxicDamage(Pawn p)
		{
			if (p.Spawned && p.Position.Roofed(p.Map))
			{
				return;
			}
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			float num = 0.028758334f;
			num *= p.GetStatValue(StatDefOf.ToxicSensitivity, true);
			if (num != 0f)
			{
				float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(p.thingIDNumber ^ 74374237));
				num *= num2;
				HealthUtility.AdjustSeverity(p, HediffDefOf.ToxicBuildup, num);
			}
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0013A33C File Offset: 0x0013853C
		public override void DoCellSteadyEffects(IntVec3 c, Map map)
		{
			if (!c.Roofed(map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is Plant)
					{
						if (thing.def.plant.dieFromToxicFallout && Rand.Value < 0.0065f)
						{
							thing.Kill(null, null);
						}
					}
					else if (thing.def.category == ThingCategory.Item)
					{
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null && compRottable.Stage < RotStage.Dessicated)
						{
							compRottable.RotProgress += 3000f;
						}
					}
				}
			}
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0013A3E8 File Offset: 0x001385E8
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0013A41D File Offset: 0x0013861D
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0013A431 File Offset: 0x00138631
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0005AC15 File Offset: 0x00058E15
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0013A452 File Offset: 0x00138652
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x04002321 RID: 8993
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04002322 RID: 8994
		private const float SkyGlow = 0.85f;

		// Token: 0x04002323 RID: 8995
		private SkyColorSet ToxicFalloutColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);

		// Token: 0x04002324 RID: 8996
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_Fallout()
		};

		// Token: 0x04002325 RID: 8997
		public const int CheckInterval = 3451;

		// Token: 0x04002326 RID: 8998
		private const float ToxicPerDay = 0.5f;

		// Token: 0x04002327 RID: 8999
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04002328 RID: 9000
		private const float CorpseRotProgressAdd = 3000f;
	}
}
