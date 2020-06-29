using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class ApparelProperties
	{
		
		// (get) Token: 0x060034E9 RID: 13545 RVA: 0x00121EF8 File Offset: 0x001200F8
		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937, false);
				return ApparelLayerDefOf.Belt;
			}
		}

		
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x00121F38 File Offset: 0x00120138
		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || this.gender == wearerGender;
		}

		
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		
		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false).CapitalizeFirst();
		}

		
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false).CapitalizeFirst();
		}

		
		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartGroupDef[] array = (from bpgd in (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>().SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		
		[NoTranslate]
		public string wornGraphicPath = "";

		
		public bool useWornGraphicMask;

		
		[NoTranslate]
		public List<string> tags = new List<string>();

		
		[NoTranslate]
		public List<string> defaultOutfitTags;

		
		public bool canBeGeneratedToSatisfyWarmth = true;

		
		public float wearPerDay = 0.4f;

		
		public bool careIfWornByCorpse = true;

		
		public bool hatRenderedFrontOfFace;

		
		public bool useDeflectMetalEffect;

		
		public Gender gender;

		
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
