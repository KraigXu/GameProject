using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000076 RID: 118
	public class BodyPartDef : Def
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001732E File Offset: 0x0001552E
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00017336 File Offset: 0x00015536
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0001733E File Offset: 0x0001553E
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001735A File Offset: 0x0001555A
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return base.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00017394 File Offset: 0x00015594
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000173A4 File Offset: 0x000155A4
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001740A File Offset: 0x0001560A
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001741D File Offset: 0x0001561D
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00017434 File Offset: 0x00015634
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.conceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x04000195 RID: 405
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000196 RID: 406
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x04000197 RID: 407
		public int hitPoints = 10;

		// Token: 0x04000198 RID: 408
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x04000199 RID: 409
		public float bleedRate = 1f;

		// Token: 0x0400019A RID: 410
		public float frostbiteVulnerability;

		// Token: 0x0400019B RID: 411
		private bool skinCovered;

		// Token: 0x0400019C RID: 412
		private bool solid;

		// Token: 0x0400019D RID: 413
		public bool alive = true;

		// Token: 0x0400019E RID: 414
		public bool delicate;

		// Token: 0x0400019F RID: 415
		public bool beautyRelated;

		// Token: 0x040001A0 RID: 416
		public bool conceptual;

		// Token: 0x040001A1 RID: 417
		public bool socketed;

		// Token: 0x040001A2 RID: 418
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040001A3 RID: 419
		public bool pawnGeneratorCanAmputate;

		// Token: 0x040001A4 RID: 420
		public bool canSuggestAmputation = true;

		// Token: 0x040001A5 RID: 421
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x040001A6 RID: 422
		public bool destroyableByDamage = true;

		// Token: 0x040001A7 RID: 423
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
