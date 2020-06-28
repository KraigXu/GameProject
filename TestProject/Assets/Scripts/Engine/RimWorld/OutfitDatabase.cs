using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000938 RID: 2360
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x0012BC33 File Offset: 0x00129E33
		public List<Outfit> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x0012BC3B File Offset: 0x00129E3B
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x0012BC54 File Offset: 0x00129E54
		public void ExposeData()
		{
			Scribe_Collections.Look<Outfit>(ref this.outfits, "outfits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x0012BC6C File Offset: 0x00129E6C
		public Outfit DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x0012BC90 File Offset: 0x00129E90
		public AcceptanceReport TryDelete(Outfit outfit)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.outfits != null && pawn.outfits.CurrentOutfit == outfit)
				{
					return new AcceptanceReport("OutfitInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.outfits != null && pawn2.outfits.CurrentOutfit == outfit)
				{
					pawn2.outfits.CurrentOutfit = null;
				}
			}
			this.outfits.Remove(outfit);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0012BD80 File Offset: 0x00129F80
		public Outfit MakeNewOutfit()
		{
			int num;
			if (!this.outfits.Any<Outfit>())
			{
				num = 1;
			}
			else
			{
				num = this.outfits.Max((Outfit o) => o.uniqueId) + 1;
			}
			int uniqueId = num;
			Outfit outfit = new Outfit(uniqueId, "Outfit".Translate() + " " + uniqueId.ToString());
			outfit.filter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			this.outfits.Add(outfit);
			return outfit;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x0012BE18 File Offset: 0x0012A018
		private void GenerateStartingOutfits()
		{
			this.MakeNewOutfit().label = "OutfitAnything".Translate();
			Outfit outfit = this.MakeNewOutfit();
			outfit.label = "OutfitWorker".Translate();
			outfit.filter.SetDisallowAll(null, null);
			outfit.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.apparel != null && thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker"))
				{
					outfit.filter.SetAllow(thingDef, true);
				}
			}
			Outfit outfit2 = this.MakeNewOutfit();
			outfit2.label = "OutfitSoldier".Translate();
			outfit2.filter.SetDisallowAll(null, null);
			outfit2.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.apparel != null && thingDef2.apparel.defaultOutfitTags != null && thingDef2.apparel.defaultOutfitTags.Contains("Soldier"))
				{
					outfit2.filter.SetAllow(thingDef2, true);
				}
			}
			Outfit outfit3 = this.MakeNewOutfit();
			outfit3.label = "OutfitNudist".Translate();
			outfit3.filter.SetDisallowAll(null, null);
			outfit3.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.apparel != null && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
				{
					outfit3.filter.SetAllow(thingDef3, true);
				}
			}
		}

		// Token: 0x0400210F RID: 8463
		private List<Outfit> outfits = new List<Outfit>();
	}
}
