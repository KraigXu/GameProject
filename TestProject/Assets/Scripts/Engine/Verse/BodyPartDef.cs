using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class BodyPartDef : Def
	{
		
		
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		
		
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		
		
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

		
		public override IEnumerable<string> ConfigErrors()
		{


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

		
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		
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

		
		[MustTranslate]
		public string labelShort;

		
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		
		public int hitPoints = 10;

		
		public float permanentInjuryChanceFactor = 1f;

		
		public float bleedRate = 1f;

		
		public float frostbiteVulnerability;

		
		private bool skinCovered;

		
		private bool solid;

		
		public bool alive = true;

		
		public bool delicate;

		
		public bool beautyRelated;

		
		public bool conceptual;

		
		public bool socketed;

		
		public ThingDef spawnThingOnRemoved;

		
		public bool pawnGeneratorCanAmputate;

		
		public bool canSuggestAmputation = true;

		
		public Dictionary<DamageDef, float> hitChanceFactors;

		
		public bool destroyableByDamage = true;

		
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
