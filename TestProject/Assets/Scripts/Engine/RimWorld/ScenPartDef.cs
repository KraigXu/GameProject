using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000900 RID: 2304
	public class ScenPartDef : Def
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060036DF RID: 14047 RVA: 0x00128605 File Offset: 0x00126805
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x00128613 File Offset: 0x00126813
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.scenPartClass == null)
			{
				yield return "scenPartClass is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001F9B RID: 8091
		public ScenPartCategory category;

		// Token: 0x04001F9C RID: 8092
		public Type scenPartClass;

		// Token: 0x04001F9D RID: 8093
		public float summaryPriority = -1f;

		// Token: 0x04001F9E RID: 8094
		public float selectionWeight = 1f;

		// Token: 0x04001F9F RID: 8095
		public int maxUses = 999999;

		// Token: 0x04001FA0 RID: 8096
		public Type pageClass;

		// Token: 0x04001FA1 RID: 8097
		public GameConditionDef gameCondition;

		// Token: 0x04001FA2 RID: 8098
		public bool gameConditionTargetsWorld;

		// Token: 0x04001FA3 RID: 8099
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x04001FA4 RID: 8100
		public Type designatorType;
	}
}
