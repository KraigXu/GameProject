using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	
	public static class WorkTypeDefsUtility
	{
		
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001FEB0 File Offset: 0x0001E0B0
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		
		public static string LabelTranslated(this WorkTags tags)
		{
			if (tags <= WorkTags.Artistic)
			{
				if (tags <= WorkTags.Social)
				{
					if (tags <= WorkTags.Violent)
					{
						switch (tags)
						{
						case WorkTags.None:
							return "WorkTagNone".Translate();
						case (WorkTags)1:
						case (WorkTags)3:
							break;
						case WorkTags.ManualDumb:
							return "WorkTagManualDumb".Translate();
						case WorkTags.ManualSkilled:
							return "WorkTagManualSkilled".Translate();
						default:
							if (tags == WorkTags.Violent)
							{
								return "WorkTagViolent".Translate();
							}
							break;
						}
					}
					else
					{
						if (tags == WorkTags.Caring)
						{
							return "WorkTagCaring".Translate();
						}
						if (tags == WorkTags.Social)
						{
							return "WorkTagSocial".Translate();
						}
					}
				}
				else if (tags <= WorkTags.Intellectual)
				{
					if (tags == WorkTags.Commoner)
					{
						return "WorkTagCommoner".Translate();
					}
					if (tags == WorkTags.Intellectual)
					{
						return "WorkTagIntellectual".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Animals)
					{
						return "WorkTagAnimals".Translate();
					}
					if (tags == WorkTags.Artistic)
					{
						return "WorkTagArtistic".Translate();
					}
				}
			}
			else if (tags <= WorkTags.Cleaning)
			{
				if (tags <= WorkTags.Cooking)
				{
					if (tags == WorkTags.Crafting)
					{
						return "WorkTagCrafting".Translate();
					}
					if (tags == WorkTags.Cooking)
					{
						return "WorkTagCooking".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Firefighting)
					{
						return "WorkTagFirefighting".Translate();
					}
					if (tags == WorkTags.Cleaning)
					{
						return "WorkTagCleaning".Translate();
					}
				}
			}
			else if (tags <= WorkTags.PlantWork)
			{
				if (tags == WorkTags.Hauling)
				{
					return "WorkTagHauling".Translate();
				}
				if (tags == WorkTags.PlantWork)
				{
					return "WorkTagPlantWork".Translate();
				}
			}
			else
			{
				if (tags == WorkTags.Mining)
				{
					return "WorkTagMining".Translate();
				}
				if (tags == WorkTags.Hunting)
				{
					return "WorkTagHunting".Translate();
				}
				if (tags == WorkTags.AllWork)
				{
					return "WorkTagAllWork".Translate();
				}
			}
			Log.Error("Unknown or mixed worktags for naming: " + (int)tags, false);
			return "Worktag";
		}

		
		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if ((workTypeDef.workTags & a) != WorkTags.None && (workTypeDef.workTags & b) != WorkTags.None)
				{
					return true;
				}
			}
			return false;
		}
	}
}
