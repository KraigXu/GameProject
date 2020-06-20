using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2A RID: 3370
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x060051EE RID: 20974 RVA: 0x001B6397 File Offset: 0x001B4597
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001B639E File Offset: 0x001B459E
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001B63CD File Offset: 0x001B45CD
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001B63F4 File Offset: 0x001B45F4
		public override string InspectStringExtra(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				List<Thing> forCell = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius);
				if (forCell.Count > 0)
				{
					TaggedString taggedString = "MeditationFocusImpacted".Translate() + ": " + (from c in forCell.Take(3)
					select c.LabelShort).ToCommaList(false).CapitalizeFirst();
					if (forCell.Count > 3)
					{
						taggedString += " " + "Etc".Translate();
					}
					return taggedString;
				}
			}
			return "";
		}

		// Token: 0x04002D2A RID: 11562
		public float radius = 10f;
	}
}
