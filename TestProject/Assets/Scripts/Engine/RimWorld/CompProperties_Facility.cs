using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086D RID: 2157
	public class CompProperties_Facility : CompProperties
	{
		// Token: 0x06003522 RID: 13602 RVA: 0x00122D50 File Offset: 0x00120F50
		public CompProperties_Facility()
		{
			this.compClass = typeof(CompFacility);
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x00122D7C File Offset: 0x00120F7C
		public override void ResolveReferences(ThingDef parentDef)
		{
			this.linkableBuildings = new List<ThingDef>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
				if (compProperties != null && compProperties.linkableFacilities != null)
				{
					for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
					{
						if (compProperties.linkableFacilities[j] == parentDef)
						{
							this.linkableBuildings.Add(allDefsListForReading[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x04001C6A RID: 7274
		[Unsaved(false)]
		public List<ThingDef> linkableBuildings;

		// Token: 0x04001C6B RID: 7275
		public List<StatModifier> statOffsets;

		// Token: 0x04001C6C RID: 7276
		public int maxSimultaneous = 1;

		// Token: 0x04001C6D RID: 7277
		public bool mustBePlacedAdjacent;

		// Token: 0x04001C6E RID: 7278
		public bool mustBePlacedAdjacentCardinalToBedHead;

		// Token: 0x04001C6F RID: 7279
		public bool canLinkToMedBedsOnly;

		// Token: 0x04001C70 RID: 7280
		public float maxDistance = 8f;
	}
}
