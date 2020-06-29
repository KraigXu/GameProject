using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ThoughtDef : Def
	{
		
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

		
		// (get) Token: 0x06003735 RID: 14133 RVA: 0x00129342 File Offset: 0x00127542
		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000f);
			}
		}

		
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

		
		// (get) Token: 0x06003737 RID: 14135 RVA: 0x001293A1 File Offset: 0x001275A1
		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		
		// (get) Token: 0x06003738 RID: 14136 RVA: 0x001293AC File Offset: 0x001275AC
		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		
		// (get) Token: 0x06003739 RID: 14137 RVA: 0x001293C3 File Offset: 0x001275C3
		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != int.MinValue;
			}
		}

		
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

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
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

		
		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}

		
		public Type thoughtClass;

		
		public Type workerClass;

		
		public List<ThoughtStage> stages = new List<ThoughtStage>();

		
		public int stackLimit = 1;

		
		public float stackedEffectMultiplier = 0.75f;

		
		public float durationDays;

		
		public bool invert;

		
		public bool validWhileDespawned;

		
		public ThoughtDef nextThought;

		
		public List<TraitDef> nullifyingTraits;

		
		public List<TaleDef> nullifyingOwnTales;

		
		public List<TraitDef> requiredTraits;

		
		public int requiredTraitsDegree = int.MinValue;

		
		public StatDef effectMultiplyingStat;

		
		public HediffDef hediff;

		
		public GameConditionDef gameCondition;

		
		public bool nullifiedIfNotColonist;

		
		public ThoughtDef thoughtToMake;

		
		[NoTranslate]
		private string icon;

		
		public bool showBubble;

		
		public int stackLimitForSameOtherPawn = -1;

		
		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		
		public float maxCumulatedOpinionOffset = float.MaxValue;

		
		public TaleDef taleDef;

		
		[Unsaved(false)]
		private ThoughtWorker workerInt;

		
		[Unsaved(false)]
		private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

		
		private Texture2D iconInt;
	}
}
