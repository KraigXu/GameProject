using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B6 RID: 182
	public class HediffDef : Def
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001B3F2 File Offset: 0x000195F2
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001B40C File Offset: 0x0001960C
		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001B4AA File Offset: 0x000196AA
		public Hediff ConcreteExample
		{
			get
			{
				if (this.concreteExampleInt == null)
				{
					this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this);
				}
				return this.concreteExampleInt;
			}
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001B4C8 File Offset: 0x000196C8
		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001B510 File Offset: 0x00019710
		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001B564 File Offset: 0x00019764
		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001B5BC File Offset: 0x000197BC
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001B5F0 File Offset: 0x000197F0
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return string.Format(this.labelNounPretty, this.label, bodyPart.Label);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001B618 File Offset: 0x00019818
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.hediffClass == null)
			{
				yield return "hediffClass is null";
			}
			if (!this.comps.NullOrEmpty<HediffCompProperties>() && !typeof(HediffWithComps).IsAssignableFrom(this.hediffClass))
			{
				yield return "has comps but hediffClass is not HediffWithComps or subclass thereof";
			}
			if (this.minSeverity > this.initialSeverity)
			{
				yield return "minSeverity is greater than initialSeverity";
			}
			if (this.maxSeverity < this.initialSeverity)
			{
				yield return "maxSeverity is lower than initialSeverity";
			}
			if (!this.tendable && this.HasComp(typeof(HediffComp_TendDuration)))
			{
				yield return "has HediffComp_TendDuration but tendable = false";
			}
			if (string.IsNullOrEmpty(this.description))
			{
				yield return "Hediff with defName " + this.defName + " has no description!";
			}
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (string arg in this.comps[i].ConfigErrors(this))
					{
						yield return this.comps[i] + ": " + arg;
					}
					enumerator = null;
					num = i;
				}
			}
			if (this.stages != null)
			{
				int num;
				if (!typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass))
				{
					for (int i = 0; i < this.stages.Count; i = num + 1)
					{
						if (i >= 1 && this.stages[i].minSeverity <= this.stages[i - 1].minSeverity)
						{
							yield return "stages are not in order of minSeverity";
						}
						num = i;
					}
				}
				for (int i = 0; i < this.stages.Count; i = num + 1)
				{
					if (this.stages[i].makeImmuneTo != null)
					{
						if (!this.stages[i].makeImmuneTo.Any((HediffDef im) => im.HasComp(typeof(HediffComp_Immunizable))))
						{
							yield return "makes immune to hediff which doesn't have comp immunizable";
						}
					}
					if (this.stages[i].hediffGivers != null)
					{
						for (int j = 0; j < this.stages[i].hediffGivers.Count; j = num + 1)
						{
							foreach (string text2 in this.stages[i].hediffGivers[j].ConfigErrors())
							{
								yield return text2;
							}
							enumerator = null;
							num = j;
						}
					}
					num = i;
				}
			}
			if (this.hediffGivers != null)
			{
				int num;
				for (int i = 0; i < this.hediffGivers.Count; i = num + 1)
				{
					foreach (string text3 in this.hediffGivers[i].ConfigErrors())
					{
						yield return text3;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001B628 File Offset: 0x00019828
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.stages != null && this.stages.Count == 1)
			{
				foreach (StatDrawEntry statDrawEntry in this.stages[0].SpecialDisplayStats())
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001B638 File Offset: 0x00019838
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x040003AD RID: 941
		public Type hediffClass = typeof(Hediff);

		// Token: 0x040003AE RID: 942
		public List<HediffCompProperties> comps;

		// Token: 0x040003AF RID: 943
		public float initialSeverity = 0.5f;

		// Token: 0x040003B0 RID: 944
		public float lethalSeverity = -1f;

		// Token: 0x040003B1 RID: 945
		public List<HediffStage> stages;

		// Token: 0x040003B2 RID: 946
		public bool tendable;

		// Token: 0x040003B3 RID: 947
		public bool isBad = true;

		// Token: 0x040003B4 RID: 948
		public ThingDef spawnThingOnRemoved;

		// Token: 0x040003B5 RID: 949
		public float chanceToCauseNoPain;

		// Token: 0x040003B6 RID: 950
		public bool makesSickThought;

		// Token: 0x040003B7 RID: 951
		public bool makesAlert = true;

		// Token: 0x040003B8 RID: 952
		public NeedDef causesNeed;

		// Token: 0x040003B9 RID: 953
		public NeedDef disablesNeed;

		// Token: 0x040003BA RID: 954
		public float minSeverity;

		// Token: 0x040003BB RID: 955
		public float maxSeverity = float.MaxValue;

		// Token: 0x040003BC RID: 956
		public bool scenarioCanAdd;

		// Token: 0x040003BD RID: 957
		public List<HediffGiver> hediffGivers;

		// Token: 0x040003BE RID: 958
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x040003BF RID: 959
		public TaleDef taleOnVisible;

		// Token: 0x040003C0 RID: 960
		public bool everCurableByItem = true;

		// Token: 0x040003C1 RID: 961
		public string battleStateLabel;

		// Token: 0x040003C2 RID: 962
		public string labelNounPretty;

		// Token: 0x040003C3 RID: 963
		public List<string> tags;

		// Token: 0x040003C4 RID: 964
		public bool priceImpact;

		// Token: 0x040003C5 RID: 965
		public float priceOffset;

		// Token: 0x040003C6 RID: 966
		public bool chronic;

		// Token: 0x040003C7 RID: 967
		public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x040003C8 RID: 968
		public bool removeOnQuestLodgers;

		// Token: 0x040003C9 RID: 969
		public bool displayWound;

		// Token: 0x040003CA RID: 970
		public Color defaultLabelColor = Color.white;

		// Token: 0x040003CB RID: 971
		public InjuryProps injuryProps;

		// Token: 0x040003CC RID: 972
		public AddedBodyPartProps addedPartProps;

		// Token: 0x040003CD RID: 973
		[MustTranslate]
		public string labelNoun;

		// Token: 0x040003CE RID: 974
		private bool alwaysAllowMothballCached;

		// Token: 0x040003CF RID: 975
		private bool alwaysAllowMothball;

		// Token: 0x040003D0 RID: 976
		private Hediff concreteExampleInt;
	}
}
