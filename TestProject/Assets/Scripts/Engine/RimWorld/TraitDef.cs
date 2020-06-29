using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class TraitDef : Def
	{
		
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		
		public TraitDegreeData DataAtDegree(int degree)
		{
			for (int i = 0; i < this.degreeDatas.Count; i++)
			{
				if (this.degreeDatas[i].degree == degree)
				{
					return this.degreeDatas[i];
				}
			}
			Log.Error(string.Concat(new object[]
			{
				this.defName,
				" found no data at degree ",
				degree,
				", returning first defined."
			}), false);
			return this.degreeDatas[0];
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.commonality < 0.001f && this.commonalityFemale < 0.001f)
			{
				yield return "TraitDef " + this.defName + " has 0 commonality.";
			}
			if (!this.degreeDatas.Any<TraitDegreeData>())
			{
				yield return this.defName + " has no degree datas.";
			}
			int num;
			for (int i = 0; i < this.degreeDatas.Count; i = num + 1)
			{
				TraitDegreeData dd = this.degreeDatas[i];
				if ((from dd2 in this.degreeDatas
				where dd2.degree == dd.degree
				select dd2).Count<TraitDegreeData>() > 1)
				{
					yield return ">1 datas for degree " + dd.degree;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		
		public bool ConflictsWith(TraitDef other)
		{
			if ((other.conflictingTraits != null && other.conflictingTraits.Contains(this)) || (this.conflictingTraits != null && this.conflictingTraits.Contains(other)))
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		
		public List<string> exclusionTags = new List<string>();

		
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		
		public WorkTags requiredWorkTags;

		
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		
		public WorkTags disabledWorkTags;

		
		private float commonality = 1f;

		
		private float commonalityFemale = -1f;

		
		public bool allowOnHostileSpawn = true;
	}
}
