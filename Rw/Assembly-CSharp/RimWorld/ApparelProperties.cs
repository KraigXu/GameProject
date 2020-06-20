using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200085F RID: 2143
	public class ApparelProperties
	{
		// Token: 0x1700096E RID: 2414
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

		// Token: 0x1700096F RID: 2415
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

		// Token: 0x060034EB RID: 13547 RVA: 0x00121FA7 File Offset: 0x001201A7
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || this.gender == wearerGender;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00121FBC File Offset: 0x001201BC
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00122025 File Offset: 0x00120225
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			yield break;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0012203C File Offset: 0x0012023C
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

		// Token: 0x060034EF RID: 13551 RVA: 0x0012207C File Offset: 0x0012027C
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x001220D4 File Offset: 0x001202D4
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x0012210C File Offset: 0x0012030C
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

		// Token: 0x04001BC9 RID: 7113
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04001BCA RID: 7114
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x04001BCB RID: 7115
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x04001BCC RID: 7116
		public bool useWornGraphicMask;

		// Token: 0x04001BCD RID: 7117
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04001BCE RID: 7118
		[NoTranslate]
		public List<string> defaultOutfitTags;

		// Token: 0x04001BCF RID: 7119
		public bool canBeGeneratedToSatisfyWarmth = true;

		// Token: 0x04001BD0 RID: 7120
		public float wearPerDay = 0.4f;

		// Token: 0x04001BD1 RID: 7121
		public bool careIfWornByCorpse = true;

		// Token: 0x04001BD2 RID: 7122
		public bool hatRenderedFrontOfFace;

		// Token: 0x04001BD3 RID: 7123
		public bool useDeflectMetalEffect;

		// Token: 0x04001BD4 RID: 7124
		public Gender gender;

		// Token: 0x04001BD5 RID: 7125
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x04001BD6 RID: 7126
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		// Token: 0x04001BD7 RID: 7127
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
