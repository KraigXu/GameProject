using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000919 RID: 2329
	public class TraitDef : Def
	{
		// Token: 0x06003756 RID: 14166 RVA: 0x00129847 File Offset: 0x00127A47
		public static TraitDef Named(string defName)
		{
			return DefDatabase<TraitDef>.GetNamed(defName, true);
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x00129850 File Offset: 0x00127A50
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

		// Token: 0x06003758 RID: 14168 RVA: 0x001298D3 File Offset: 0x00127AD3
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
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

		// Token: 0x06003759 RID: 14169 RVA: 0x001298E3 File Offset: 0x00127AE3
		public bool ConflictsWith(Trait other)
		{
			return this.ConflictsWith(other.def);
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x001298F4 File Offset: 0x00127AF4
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

		// Token: 0x0600375B RID: 14171 RVA: 0x00129971 File Offset: 0x00127B71
		public bool ConflictsWithPassion(SkillDef passion)
		{
			return this.conflictingPassions != null && this.conflictingPassions.Contains(passion);
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x00129989 File Offset: 0x00127B89
		public bool RequiresPassion(SkillDef passion)
		{
			return this.forcedPassions != null && this.forcedPassions.Contains(passion);
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x001299A1 File Offset: 0x00127BA1
		public float GetGenderSpecificCommonality(Gender gender)
		{
			if (gender == Gender.Female && this.commonalityFemale >= 0f)
			{
				return this.commonalityFemale;
			}
			return this.commonality;
		}

		// Token: 0x040020A6 RID: 8358
		public List<TraitDegreeData> degreeDatas = new List<TraitDegreeData>();

		// Token: 0x040020A7 RID: 8359
		public List<TraitDef> conflictingTraits = new List<TraitDef>();

		// Token: 0x040020A8 RID: 8360
		public List<string> exclusionTags = new List<string>();

		// Token: 0x040020A9 RID: 8361
		public List<SkillDef> conflictingPassions = new List<SkillDef>();

		// Token: 0x040020AA RID: 8362
		public List<SkillDef> forcedPassions = new List<SkillDef>();

		// Token: 0x040020AB RID: 8363
		public List<WorkTypeDef> requiredWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040020AC RID: 8364
		public WorkTags requiredWorkTags;

		// Token: 0x040020AD RID: 8365
		public List<WorkTypeDef> disabledWorkTypes = new List<WorkTypeDef>();

		// Token: 0x040020AE RID: 8366
		public WorkTags disabledWorkTags;

		// Token: 0x040020AF RID: 8367
		private float commonality = 1f;

		// Token: 0x040020B0 RID: 8368
		private float commonalityFemale = -1f;

		// Token: 0x040020B1 RID: 8369
		public bool allowOnHostileSpawn = true;
	}
}
