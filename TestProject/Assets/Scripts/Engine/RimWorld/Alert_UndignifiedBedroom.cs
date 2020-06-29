using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class Alert_UndignifiedBedroom : Alert
	{
		
		public Alert_UndignifiedBedroom()
		{
			this.defaultLabel = "UndignifiedBedroom".Translate();
			this.defaultExplanation = "UndignifiedBedroomDesc".Translate();
		}

		
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x001CB084 File Offset: 0x001C9284
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.royalty != null && pawn.royalty.GetUnmetBedroomRequirements(true, false).Any<string>())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithBedroomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetBedroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetBedroomRequirements(true, true).ToArray<string>();
				bool flag = royalTitleDef != null && array.Length != 0;
				StringBuilder stringBuilder = new StringBuilder();
				if (flag)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						t.LabelShort,
						" (",
						royalTitleDef.GetLabelFor(t.gender),
						"):\n",
						array.ToLineList("- ")
					}));
				}
				if (array2.Length != 0)
				{
					if (flag)
					{
						stringBuilder.Append("\n\n");
					}
					stringBuilder.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("- "));
				}
				return stringBuilder.ToString();
			}).ToLineList("\n", false);
		}

		
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
