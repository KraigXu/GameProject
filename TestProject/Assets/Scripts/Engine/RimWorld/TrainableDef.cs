using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000917 RID: 2327
	public class TrainableDef : Def
	{
		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x001296ED File Offset: 0x001278ED
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00129718 File Offset: 0x00127918
		public bool MatchesTag(string tag)
		{
			if (tag == this.defName)
			{
				return true;
			}
			for (int i = 0; i < this.tags.Count; i++)
			{
				if (this.tags[i] == tag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x00129762 File Offset: 0x00127962
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.difficulty < 0f)
			{
				yield return "difficulty not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002081 RID: 8321
		public float difficulty = -1f;

		// Token: 0x04002082 RID: 8322
		public float minBodySize;

		// Token: 0x04002083 RID: 8323
		public List<TrainableDef> prerequisites;

		// Token: 0x04002084 RID: 8324
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04002085 RID: 8325
		public bool defaultTrainable;

		// Token: 0x04002086 RID: 8326
		public TrainabilityDef requiredTrainability;

		// Token: 0x04002087 RID: 8327
		public int steps = 1;

		// Token: 0x04002088 RID: 8328
		public float listPriority;

		// Token: 0x04002089 RID: 8329
		[NoTranslate]
		public string icon;

		// Token: 0x0400208A RID: 8330
		[Unsaved(false)]
		public int indent;

		// Token: 0x0400208B RID: 8331
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
