using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000914 RID: 2324
	public class ThoughtDef : Def
	{
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x001292A4 File Offset: 0x001274A4
		public string Label
		{
			get
			{
				if (!this.label.NullOrEmpty())
				{
					return this.label;
				}
				if (!this.stages.NullOrEmpty<ThoughtStage>())
				{
					if (!this.stages[0].label.NullOrEmpty())
					{
						return this.stages[0].label;
					}
					if (!this.stages[0].labelSocial.NullOrEmpty())
					{
						return this.stages[0].labelSocial;
					}
				}
				Log.Error("Cannot get good label for ThoughtDef " + this.defName, false);
				return this.defName;
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x00129342 File Offset: 0x00127542
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06003736 RID: 14134 RVA: 0x00129354 File Offset: 0x00127554
		public bool IsMemory
		{
			get
			{
				if (this.isMemoryCached == BoolUnknown.Unknown)
				{
					this.isMemoryCached = ((this.durationDays > 0f || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass)) ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.isMemoryCached == BoolUnknown.True;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x001293A1 File Offset: 0x001275A1
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x001293AC File Offset: 0x001275AC
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x001293C3 File Offset: 0x001275C3
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x001293D5 File Offset: 0x001275D5
		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x00129415 File Offset: 0x00127615
		public Type ThoughtClass
		{
			get
			{
				if (this.thoughtClass != null)
				{
					return this.thoughtClass;
				}
				if (this.IsMemory)
				{
					return typeof(Thought_Memory);
				}
				return typeof(Thought_Situational);
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600373C RID: 14140 RVA: 0x00129449 File Offset: 0x00127649
		public Texture2D Icon
		{
			get
			{
				if (this.iconInt == null)
				{
					if (this.icon == null)
					{
						return null;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconInt;
			}
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0012947B File Offset: 0x0012767B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.stages.NullOrEmpty<ThoughtStage>())
			{
				yield return "no stages";
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
			}
			if (!this.IsMemory && this.workerClass == null && this.IsSituational)
			{
				yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			}
			int num;
			for (int i = 0; i < this.stages.Count; i = num + 1)
			{
				if (this.stages[i] != null)
				{
					foreach (string text2 in this.stages[i].ConfigErrors())
					{
						yield return text2;
					}
					enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0012948B File Offset: 0x0012768B
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		// Token: 0x04002057 RID: 8279
		public Type thoughtClass;

		// Token: 0x04002058 RID: 8280
		public Type workerClass;

		// Token: 0x04002059 RID: 8281
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		// Token: 0x0400205A RID: 8282
		public int stackLimit = 1;

		// Token: 0x0400205B RID: 8283
		public float stackedEffectMultiplier = 0.75f;

		// Token: 0x0400205C RID: 8284
		public float durationDays;

		// Token: 0x0400205D RID: 8285
		public bool invert;

		// Token: 0x0400205E RID: 8286
		public bool validWhileDespawned;

		// Token: 0x0400205F RID: 8287
		public ThoughtDef nextThought;

		// Token: 0x04002060 RID: 8288
		public List<TraitDef> nullifyingTraits;

		// Token: 0x04002061 RID: 8289
		public List<TaleDef> nullifyingOwnTales;

		// Token: 0x04002062 RID: 8290
		public List<TraitDef> requiredTraits;

		// Token: 0x04002063 RID: 8291
		public int requiredTraitsDegree = int.MinValue;

		// Token: 0x04002064 RID: 8292
		public StatDef effectMultiplyingStat;

		// Token: 0x04002065 RID: 8293
		public HediffDef hediff;

		// Token: 0x04002066 RID: 8294
		public GameConditionDef gameCondition;

		// Token: 0x04002067 RID: 8295
		public bool nullifiedIfNotColonist;

		// Token: 0x04002068 RID: 8296
		public ThoughtDef thoughtToMake;

		// Token: 0x04002069 RID: 8297
		[NoTranslate]
		private string icon;

		// Token: 0x0400206A RID: 8298
		public bool showBubble;

		// Token: 0x0400206B RID: 8299
		public int stackLimitForSameOtherPawn = -1;

		// Token: 0x0400206C RID: 8300
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		// Token: 0x0400206D RID: 8301
		public float maxCumulatedOpinionOffset = float.MaxValue;

		// Token: 0x0400206E RID: 8302
		public TaleDef taleDef;

		// Token: 0x0400206F RID: 8303
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		// Token: 0x04002070 RID: 8304
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		// Token: 0x04002071 RID: 8305
		private Texture2D iconInt;
	}
}
