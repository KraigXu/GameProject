using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B1 RID: 177
	public class GameConditionDef : Def
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x0001B35F File Offset: 0x0001955F
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001B380 File Offset: 0x00019580
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001B389 File Offset: 0x00019589
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.conditionClass == null)
			{
				yield return "conditionClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000399 RID: 921
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x0400039A RID: 922
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x0400039B RID: 923
		[MustTranslate]
		public string endMessage;

		// Token: 0x0400039C RID: 924
		[MustTranslate]
		public string letterText;

		// Token: 0x0400039D RID: 925
		public List<ThingDef> letterHyperlinks;

		// Token: 0x0400039E RID: 926
		public LetterDef letterDef;

		// Token: 0x0400039F RID: 927
		public bool canBePermanent;

		// Token: 0x040003A0 RID: 928
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x040003A1 RID: 929
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x040003A2 RID: 930
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x040003A3 RID: 931
		public bool preventRain;

		// Token: 0x040003A4 RID: 932
		public WeatherDef weatherDef;

		// Token: 0x040003A5 RID: 933
		public float temperatureOffset = -10f;
	}
}
