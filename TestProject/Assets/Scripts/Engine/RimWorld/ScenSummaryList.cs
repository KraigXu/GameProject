using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C2C RID: 3116
	public static class ScenSummaryList
	{
		// Token: 0x06004A47 RID: 19015 RVA: 0x00191B7C File Offset: 0x0018FD7C
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			if (!text.NullOrEmpty())
			{
				return "\n" + intro + ":\n" + text;
			}
			return null;
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x00191BAC File Offset: 0x0018FDAC
		private static string SummaryList(Scenario scen, string tag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (ScenPart scenPart in scen.AllParts)
			{
				if (!scenPart.summarized)
				{
					foreach (string str in scenPart.GetSummaryListEntries(tag))
					{
						if (!flag)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("   -" + str);
						scenPart.summarized = true;
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
