using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF4 RID: 3572
	public class Alert_UndignifiedThroneroom : Alert
	{
		// Token: 0x06005682 RID: 22146 RVA: 0x001CAEF9 File Offset: 0x001C90F9
		public Alert_UndignifiedThroneroom()
		{
			this.defaultLabel = "UndignifiedThroneroom".Translate();
			this.defaultExplanation = "UndignifiedThroneroomDesc".Translate();
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06005683 RID: 22147 RVA: 0x001CAF38 File Offset: 0x001C9138
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
						if (pawn.royalty != null && pawn.royalty.GetUnmetThroneroomRequirements(true, false).Any<string>())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x001CAFE0 File Offset: 0x001C91E0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x001CAFF0 File Offset: 0x001C91F0
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithThroneRoomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetThroneroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetThroneroomRequirements(true, true).ToArray<string>();
				StringBuilder stringBuilder = new StringBuilder();
				if (array.Length != 0)
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
					if (array.Length != 0)
					{
						stringBuilder.Append("\n\n");
					}
					stringBuilder.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("- "));
				}
				return stringBuilder.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x04002F34 RID: 12084
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
