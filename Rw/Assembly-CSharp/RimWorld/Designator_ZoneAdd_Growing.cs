using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E45 RID: 3653
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x0600585B RID: 22619 RVA: 0x001D529D File Offset: 0x001D349D
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x001D52B0 File Offset: 0x001D34B0
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.tutorTag = "ZoneAdd_Growing";
			this.hotKey = KeyBindingDefOf.Misc2;
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x001D5324 File Offset: 0x001D3524
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			if (base.Map.fertilityGrid.FertilityAt(c) < ThingDefOf.Plant_Potato.plant.fertilityMin)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x001D5378 File Offset: 0x001D3578
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}
	}
}
