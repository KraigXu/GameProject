using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8E RID: 3214
	public class Blight : Thing
	{
		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004D66 RID: 19814 RVA: 0x0019FD62 File Offset: 0x0019DF62
		// (set) Token: 0x06004D67 RID: 19815 RVA: 0x0019FD6A File Offset: 0x0019DF6A
		public float Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x0019FD78 File Offset: 0x0019DF78
		public Plant Plant
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map);
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x0019FD95 File Offset: 0x0019DF95
		protected float ReproduceMTBHours
		{
			get
			{
				if (this.severity < 0.28f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.28f, 1f, 16.8f, 2.1f, this.severity);
			}
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0019FDC9 File Offset: 0x0019DFC9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0019FDF9 File Offset: 0x0019DFF9
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x0019FE24 File Offset: 0x0019E024
		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (this.DestroyIfNoPlantHere())
			{
				return;
			}
			this.Severity += 0.0333333351f;
			float reproduceMTBHours = this.ReproduceMTBHours;
			if (reproduceMTBHours > 0f && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
			{
				this.TryReproduceNow();
			}
			if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05f)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				this.lastMapMeshUpdateSeverity = this.Severity;
			}
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0019FEB5 File Offset: 0x0019E0B5
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x0019FEBE File Offset: 0x0019E0BE
		private bool DestroyIfNoPlantHere()
		{
			if (base.Destroyed)
			{
				return true;
			}
			if (this.Plant == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0019FEDC File Offset: 0x0019E0DC
		private void CheckHarmPlant()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastPlantHarmTick >= 60000)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null)
					{
						this.HarmPlant(plant);
					}
				}
				this.lastPlantHarmTick = ticksGame;
			}
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0019FF44 File Offset: 0x0019E144
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label, plant).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0019FFF6 File Offset: 0x0019E1F6
		public void TryReproduceNow()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, delegate(List<IntVec3> cells)
			{
				IntVec3 c;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement(out c))
				{
					BlightUtility.GetFirstBlightableNowPlant(c, base.Map).CropBlighted();
					return true;
				}
				return false;
			}, base.Map);
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x001A001C File Offset: 0x0019E21C
		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				PlantUtility.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = 0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Printer_Plane.PrintPlane(layer, this.TrueCenter(), this.def.graphic.drawSize * num, this.Graphic.MatAt(base.Rotation, this), 0f, false, null, Blight.workingColors, 0.1f, 0f);
		}

		// Token: 0x04002B51 RID: 11089
		private float severity = 0.2f;

		// Token: 0x04002B52 RID: 11090
		private int lastPlantHarmTick;

		// Token: 0x04002B53 RID: 11091
		private float lastMapMeshUpdateSeverity;

		// Token: 0x04002B54 RID: 11092
		private const float InitialSeverity = 0.2f;

		// Token: 0x04002B55 RID: 11093
		private const float SeverityPerDay = 1f;

		// Token: 0x04002B56 RID: 11094
		private const int DamagePerDay = 5;

		// Token: 0x04002B57 RID: 11095
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x04002B58 RID: 11096
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x04002B59 RID: 11097
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x04002B5A RID: 11098
		private const float ReproductionRadius = 4f;

		// Token: 0x04002B5B RID: 11099
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x04002B5C RID: 11100
		private static Color32[] workingColors = new Color32[4];
	}
}
