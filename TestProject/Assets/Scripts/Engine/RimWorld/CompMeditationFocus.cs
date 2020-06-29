using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class CompMeditationFocus : CompStatOffsetBase
	{
		
		// (get) Token: 0x060051BC RID: 20924 RVA: 0x001B5C61 File Offset: 0x001B3E61
		public new CompProperties_MeditationFocus Props
		{
			get
			{
				return (CompProperties_MeditationFocus)this.props;
			}
		}

		
		public override float GetStatOffset(Pawn pawn = null)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Meditation foci are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657117, false);
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				num += this.Props.offsets[i].GetOffset(this.parent, pawn);
			}
			return num;
		}

		
		public override IEnumerable<string> GetExplanation()
		{
			int num;
			for (int i = 0; i < this.Props.offsets.Count; i = num + 1)
			{
				string explanation = this.Props.offsets[i].GetExplanation(this.parent);
				if (!explanation.NullOrEmpty())
				{
					yield return explanation;
				}
				num = i;
			}
			yield break;
		}

		
		public bool CanPawnUse(Pawn pawn)
		{
			for (int i = 0; i < this.Props.focusTypes.Count; i++)
			{
				if (this.Props.focusTypes[i].CanPawnUse(pawn))
				{
					return true;
				}
			}
			return false;
		}

		
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			using (List<FocusStrengthOffset>.Enumerator enumerator = this.Props.offsets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings;
					if ((focusStrengthOffset_ArtificialBuildings = (enumerator.Current as FocusStrengthOffset_ArtificialBuildings)) != null && MeditationUtility.CountsAsArtificialBuilding(def, faction) && cellRect.ClosestCellTo(this.parent.Position).DistanceTo(this.parent.Position) <= focusStrengthOffset_ArtificialBuildings.radius)
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public override string CompInspectStringExtra()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (base.LastUser != null)
			{
				stringBuilder.Append("UserMeditationFocusStrength".Translate(base.LastUser.Named("LASTUSER")) + ": " + this.Props.statDef.ValueToString(this.parent.GetStatValueForPawn(this.Props.statDef, base.LastUser, true), ToStringNumberSense.Absolute, true));
			}
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				string text = this.Props.offsets[i].InspectStringExtra(this.parent, null);
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Meditation, "MeditationFocuses".Translate(), (from f in this.Props.focusTypes
				select f.label).ToCommaList(false).CapitalizeFirst(), "MeditationFocusesDesc".Translate(), 99995, null, null, false);
			}
			yield break;
		}

		
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.Props.offsets.Count; i++)
				{
					this.Props.offsets[i].PostDrawExtraSelectionOverlays(this.parent, base.LastUser);
				}
			}
		}
	}
}
