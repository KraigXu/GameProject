using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABC RID: 2748
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004120 RID: 16672 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x0015D1B1 File Offset: 0x0015B3B1
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004122 RID: 16674 RVA: 0x0015D1B8 File Offset: 0x0015B3B8
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0015D1C0 File Offset: 0x0015B3C0
		public Zone_Growing()
		{
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0015D1DA File Offset: 0x0015B3DA
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0015D204 File Offset: 0x0015B404
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x0015D230 File Offset: 0x0015B430
		public override string GetInspectString()
		{
			string text = "";
			if (!base.Cells.NullOrEmpty<IntVec3>())
			{
				IntVec3 c = base.Cells.First<IntVec3>();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					text += "OutdoorGrowingPeriod".Translate() + ": " + Zone_Growing.GrowingQuadrumsDescription(base.Map.Tile) + "\n";
				}
				if (PlantUtility.GrowthSeasonNow(c, base.Map, true))
				{
					text += "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0015D2E8 File Offset: 0x0015B4E8
		public static string GrowingQuadrumsDescription(int tile)
		{
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 10f, 42f);
			if (list.NullOrEmpty<Twelfth>())
			{
				return "NoGrowingPeriod".Translate();
			}
			if (list.Count == 12)
			{
				return "GrowYearRound".Translate();
			}
			return "PeriodDays".Translate(list.Count * 5 + "/" + 60) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")";
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0015D389 File Offset: 0x0015B589
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowSow".Translate(),
				defaultDesc = "CommandAllowSowDesc".Translate(),
				hotKey = KeyBindingDefOf.Command_ItemForbid,
				icon = TexCommand.ForbidOff,
				isActive = (() => this.allowSow),
				toggleAction = delegate
				{
					this.allowSow = !this.allowSow;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0015D399 File Offset: 0x0015B599
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0015D3A2 File Offset: 0x0015B5A2
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0015D3AA File Offset: 0x0015B5AA
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x040025D7 RID: 9687
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x040025D8 RID: 9688
		public bool allowSow = true;
	}
}
