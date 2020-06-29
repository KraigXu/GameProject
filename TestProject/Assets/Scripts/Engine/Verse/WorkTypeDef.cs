using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public class WorkTypeDef : Def
	{
		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
			}
			yield break;
			yield break;
		}

		
		public override void ResolveReferences()
		{
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		
		public WorkTags workTags;

		
		[MustTranslate]
		public string labelShort;

		
		[MustTranslate]
		public string pawnLabel;

		
		[MustTranslate]
		public string gerundLabel;

		
		[MustTranslate]
		public string verb;

		
		public bool visible = true;

		
		public int naturalPriority;

		
		public bool alwaysStartActive;

		
		public bool requireCapableColonist;

		
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		
		[Unsaved(false)]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();
	}
}
