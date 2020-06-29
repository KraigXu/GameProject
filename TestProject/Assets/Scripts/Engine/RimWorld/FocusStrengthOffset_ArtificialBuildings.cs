using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		
		// (get) Token: 0x060051EE RID: 20974 RVA: 0x001B6397 File Offset: 0x001B4597
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		
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

		
		public float radius = 10f;
	}
}
