using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ScenPartDef : Def
	{
		
		// (get) Token: 0x060036DF RID: 14047 RVA: 0x00128605 File Offset: 0x00126805
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public ScenPartCategory category;

		
		public Type scenPartClass;

		
		public float summaryPriority = -1f;

		
		public float selectionWeight = 1f;

		
		public int maxUses = 999999;

		
		public Type pageClass;

		
		public GameConditionDef gameCondition;

		
		public bool gameConditionTargetsWorld;

		
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		
		public Type designatorType;
	}
}
