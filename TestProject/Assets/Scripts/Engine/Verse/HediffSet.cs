using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class HediffSet : IExposable
	{
		
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0006148A File Offset: 0x0005F68A
		public float PainTotal
		{
			get
			{
				if (this.cachedPain < 0f)
				{
					this.cachedPain = this.CalculatePain();
				}
				return this.cachedPain;
			}
		}

		
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x000614AB File Offset: 0x0005F6AB
		public float BleedRateTotal
		{
			get
			{
				if (this.cachedBleedRate < 0f)
				{
					this.cachedBleedRate = this.CalculateBleedRate();
				}
				return this.cachedBleedRate;
			}
		}

		
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x000614CC File Offset: 0x0005F6CC
		public bool HasHead
		{
			get
			{
				if (this.cachedHasHead == null)
				{
					this.cachedHasHead = new bool?(this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x.def == BodyPartDefOf.Head));
				}
				return this.cachedHasHead.Value;
			}
		}

		
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0006152A File Offset: 0x0005F72A
		public float HungerRateFactor
		{
			get
			{
				return this.GetHungerRateFactor(null);
			}
		}

		
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00061534 File Offset: 0x0005F734
		public float RestFallFactor
		{
			get
			{
				float num = 1f;
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.restFallFactor;
					}
				}
				for (int j = 0; j < this.hediffs.Count; j++)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.restFallFactorOffset;
					}
				}
				return Mathf.Max(num, 0f);
			}
		}

		
		public HediffSet(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Hediff>(ref this.hediffs, "hediffs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some null hediffs.", false);
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (this.hediffs.RemoveAll((Hediff x) => x.def == null) != 0)
				{
					Log.Error(this.pawn.ToStringSafe<Pawn>() + " had some hediffs with null defs.", false);
				}
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					this.hediffs[i].pawn = this.pawn;
				}
				this.DirtyCache();
			}
		}

		
		public void AddDirect(Hediff hediff, DamageInfo? dinfo = null, DamageWorker.DamageResult damageResult = null)
		{
			if (hediff.def == null)
			{
				Log.Error("Tried to add health diff with null def. Canceling.", false);
				return;
			}
			if (hediff.Part != null && !this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff.Part))
			{
				Log.Error("Tried to add health diff to missing part " + hediff.Part, false);
				return;
			}
			hediff.ageTicks = 0;
			hediff.pawn = this.pawn;
			bool flag = false;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].TryMergeWith(hediff))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				this.hediffs.Add(hediff);
				hediff.PostAdd(dinfo);
				if (this.pawn.needs != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			bool flag2 = hediff is Hediff_MissingPart;
			if (!(hediff is Hediff_MissingPart) && hediff.Part != null && hediff.Part != this.pawn.RaceProps.body.corePart && this.GetPartHealth(hediff.Part) == 0f && hediff.Part != this.pawn.RaceProps.body.corePart)
			{
				bool flag3 = this.HasDirectlyAddedPartFor(hediff.Part);
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = !flag3;
				hediff_MissingPart.lastInjury = hediff.def;
				this.pawn.health.AddHediff(hediff_MissingPart, hediff.Part, dinfo, null);
				if (damageResult != null)
				{
					damageResult.AddHediff(hediff_MissingPart);
				}
				if (flag3)
				{
					if (dinfo != null)
					{
						hediff_MissingPart.lastInjury = HealthUtility.GetHediffDefFromDamage(dinfo.Value.Def, this.pawn, hediff.Part);
					}
					else
					{
						hediff_MissingPart.lastInjury = null;
					}
				}
				flag2 = true;
			}
			this.DirtyCache();
			if (flag2 && this.pawn.apparel != null)
			{
				this.pawn.apparel.Notify_LostBodyPart();
			}
			if (hediff.def.causesNeed != null && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		
		public void DirtyCache()
		{
			this.CacheMissingPartsCommonAncestors();
			this.cachedPain = -1f;
			this.cachedBleedRate = -1f;
			this.cachedHasHead = null;
			this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			this.pawn.health.summaryHealth.Notify_HealthChanged();
		}

		
		public float GetHungerRateFactor(HediffDef ignore = null)
		{
			float num = 1f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def != ignore)
				{
					HediffStage curStage = this.hediffs[i].CurStage;
					if (curStage != null)
					{
						num *= curStage.hungerRateFactor;
					}
				}
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j].def != ignore)
				{
					HediffStage curStage2 = this.hediffs[j].CurStage;
					if (curStage2 != null)
					{
						num += curStage2.hungerRateFactorOffset;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		
		public IEnumerable<T> GetHediffs<T>() where T : Hediff
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				T t = this.hediffs[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i + 1;
			}
			yield break;
		}

		
		public Hediff GetFirstHediffOfDef(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return this.hediffs[i];
				}
			}
			return null;
		}

		
		public bool PartIsMissing(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_MissingPart)
				{
					return true;
				}
			}
			return false;
		}

		
		public float GetPartHealth(BodyPartRecord part)
		{
			if (part == null)
			{
				return 0f;
			}
			float num = part.def.GetMaxHealth(this.pawn);
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i] is Hediff_MissingPart && this.hediffs[i].Part == part)
				{
					return 0f;
				}
				if (this.hediffs[i].Part == part)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null)
					{
						num -= hediff_Injury.Severity;
					}
				}
			}
			num = Mathf.Max(num, 0f);
			if (!part.def.destroyableByDamage)
			{
				num = Mathf.Max(num, 1f);
			}
			return (float)Mathf.RoundToInt(num);
		}

		
		public BodyPartRecord GetBrain()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
				{
					return bodyPartRecord;
				}
			}
			return null;
		}

		
		public bool HasHediff(HediffDef def, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasHediff(HediffDef def, BodyPartRecord bodyPart, bool mustBeVisible = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].def == def && this.hediffs[i].Part == bodyPart && (!mustBeVisible || this.hediffs[i].Visible))
				{
					return true;
				}
			}
			return false;
		}

		
		public IEnumerable<Verb> GetHediffsVerbs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				HediffComp_VerbGiver hediffComp_VerbGiver = this.hediffs[i].TryGetComp<HediffComp_VerbGiver>();
				if (hediffComp_VerbGiver != null)
				{
					List<Verb> verbList = hediffComp_VerbGiver.VerbTracker.AllVerbs;
					for (int j = 0; j < verbList.Count; j = num + 1)
					{
						yield return verbList[j];
						num = j;
					}
					verbList = null;
				}
				num = i + 1;
			}
			yield break;
		}

		
		public IEnumerable<Hediff> GetHediffsTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		
		public bool HasTendableHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		
		public IEnumerable<HediffComp> GetAllComps()
		{
			foreach (Hediff hediff in this.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						yield return hediffComp;
					}
					List<HediffComp>.Enumerator enumerator2 = default(List<HediffComp>.Enumerator);
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		
		public IEnumerable<Hediff_Injury> GetInjuriesTendable()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					yield return hediff_Injury;
				}
				num = i + 1;
			}
			yield break;
		}

		
		public bool HasTendableInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasNaturallyHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealNaturally())
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasTendedAndHealingInjury()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && hediff_Injury.CanHealFromTending() && hediff_Injury.Severity > 0f)
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasTemperatureInjury(TemperatureInjuryStage minStage)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((this.hediffs[i].def == HediffDefOf.Hypothermia || this.hediffs[i].def == HediffDefOf.Heatstroke) && this.hediffs[i].CurStageIndex >= (int)minStage)
				{
					return true;
				}
			}
			return false;
		}

		
		public IEnumerable<BodyPartRecord> GetInjuredParts()
		{
			return (from x in this.hediffs
			where x is Hediff_Injury
			select x.Part).Distinct<BodyPartRecord>();
		}

		
		public IEnumerable<BodyPartRecord> GetNaturallyHealingInjuredParts()
		{
			foreach (BodyPartRecord bodyPartRecord in this.GetInjuredParts())
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && this.hediffs[i].Part == bodyPartRecord && hediff_Injury.CanHealNaturally())
					{
						yield return bodyPartRecord;
						break;
					}
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		
		public List<Hediff_MissingPart> GetMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.CacheMissingPartsCommonAncestors();
			}
			return this.cachedMissingPartsCommonAncestors;
		}

		
		public IEnumerable<BodyPartRecord> GetNotMissingParts(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartTagDef tag = null, BodyPartRecord partParent = null)
		{
			List<BodyPartRecord> allPartsList = this.pawn.def.race.body.AllParts;
			int num;
			for (int i = 0; i < allPartsList.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = allPartsList[i];
				if (!this.PartIsMissing(bodyPartRecord) && (height == BodyPartHeight.Undefined || bodyPartRecord.height == height) && (depth == BodyPartDepth.Undefined || bodyPartRecord.depth == depth) && (tag == null || bodyPartRecord.def.tags.Contains(tag)) && (partParent == null || bodyPartRecord.parent == partParent))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		
		public BodyPartRecord GetRandomNotMissingPart(DamageDef damDef, BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined, BodyPartRecord partParent = null)
		{
			IEnumerable<BodyPartRecord> notMissingParts;
			if (this.GetNotMissingParts(height, depth, null, partParent).Any<BodyPartRecord>())
			{
				notMissingParts = this.GetNotMissingParts(height, depth, null, partParent);
			}
			else
			{
				if (!this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent).Any<BodyPartRecord>())
				{
					return null;
				}
				notMissingParts = this.GetNotMissingParts(BodyPartHeight.Undefined, depth, null, partParent);
			}
			BodyPartRecord result;
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs * x.def.GetHitChanceFactorFor(damDef), out result))
			{
				return result;
			}
			if (notMissingParts.TryRandomElementByWeight((BodyPartRecord x) => x.coverageAbs, out result))
			{
				return result;
			}
			throw new InvalidOperationException();
		}

		
		public float GetCoverageOfNotMissingNaturalParts(BodyPartRecord part)
		{
			if (this.PartIsMissing(part))
			{
				return 0f;
			}
			if (this.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return 0f;
			}
			this.coverageRejectedPartsSet.Clear();
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.GetMissingPartsCommonAncestors();
			for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
			{
				this.coverageRejectedPartsSet.Add(missingPartsCommonAncestors[i].Part);
			}
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				if (this.hediffs[j] is Hediff_AddedPart)
				{
					this.coverageRejectedPartsSet.Add(this.hediffs[j].Part);
				}
			}
			float num = 0f;
			this.coveragePartsStack.Clear();
			this.coveragePartsStack.Push(part);
			while (this.coveragePartsStack.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = this.coveragePartsStack.Pop();
				num += bodyPartRecord.coverageAbs;
				for (int k = 0; k < bodyPartRecord.parts.Count; k++)
				{
					if (!this.coverageRejectedPartsSet.Contains(bodyPartRecord.parts[k]))
					{
						this.coveragePartsStack.Push(bodyPartRecord.parts[k]);
					}
				}
			}
			this.coveragePartsStack.Clear();
			this.coverageRejectedPartsSet.Clear();
			return num;
		}

		
		private void CacheMissingPartsCommonAncestors()
		{
			if (this.cachedMissingPartsCommonAncestors == null)
			{
				this.cachedMissingPartsCommonAncestors = new List<Hediff_MissingPart>();
			}
			else
			{
				this.cachedMissingPartsCommonAncestors.Clear();
			}
			this.missingPartsCommonAncestorsQueue.Clear();
			this.missingPartsCommonAncestorsQueue.Enqueue(this.pawn.def.race.body.corePart);
			while (this.missingPartsCommonAncestorsQueue.Count != 0)
			{
				BodyPartRecord node = this.missingPartsCommonAncestorsQueue.Dequeue();
				if (!this.PartOrAnyAncestorHasDirectlyAddedParts(node))
				{
					Hediff_MissingPart hediff_MissingPart = (from x in this.GetHediffs<Hediff_MissingPart>()
					where x.Part == node
					select x).FirstOrDefault<Hediff_MissingPart>();
					if (hediff_MissingPart != null)
					{
						this.cachedMissingPartsCommonAncestors.Add(hediff_MissingPart);
					}
					else
					{
						for (int i = 0; i < node.parts.Count; i++)
						{
							this.missingPartsCommonAncestorsQueue.Enqueue(node.parts[i]);
						}
					}
				}
			}
		}

		
		public bool HasDirectlyAddedPartFor(BodyPartRecord part)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (this.hediffs[i].Part == part && this.hediffs[i] is Hediff_AddedPart)
				{
					return true;
				}
			}
			return false;
		}

		
		public bool PartOrAnyAncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return this.HasDirectlyAddedPartFor(part) || (part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent));
		}

		
		public bool AncestorHasDirectlyAddedParts(BodyPartRecord part)
		{
			return part.parent != null && this.PartOrAnyAncestorHasDirectlyAddedParts(part.parent);
		}

		
		public IEnumerable<Hediff> GetTendableNonInjuryNonMissingPartHediffs()
		{
			int num;
			for (int i = 0; i < this.hediffs.Count; i = num + 1)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					yield return this.hediffs[i];
				}
				num = i;
			}
			yield break;
		}

		
		public bool HasTendableNonInjuryNonMissingPartHediff(bool forAlert = false)
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if ((!forAlert || this.hediffs[i].def.makesAlert) && !(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			return false;
		}

		
		public bool HasImmunizableNotImmuneHediff()
		{
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				if (!(this.hediffs[i] is Hediff_Injury) && !(this.hediffs[i] is Hediff_MissingPart) && this.hediffs[i].Visible && this.hediffs[i].def.PossibleToDevelopImmunityNaturally() && !this.hediffs[i].FullyImmune())
				{
					return true;
				}
			}
			return false;
		}

		
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00062458 File Offset: 0x00060658
		public bool AnyHediffMakesSickThought
		{
			get
			{
				for (int i = 0; i < this.hediffs.Count; i++)
				{
					if (this.hediffs[i].def.makesSickThought && this.hediffs[i].Visible)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		private float CalculateBleedRate()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.health.Dead)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				Hediff hediff = this.hediffs[i];
				HediffStage curStage = hediff.CurStage;
				if (curStage != null)
				{
					num *= curStage.totalBleedFactor;
				}
				num2 += hediff.BleedRate;
			}
			return num2 * num / this.pawn.HealthScale;
		}

		
		private float CalculatePain()
		{
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.Dead)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.hediffs.Count; i++)
			{
				num += this.hediffs[i].PainOffset;
			}
			float num2 = num / this.pawn.HealthScale;
			for (int j = 0; j < this.hediffs.Count; j++)
			{
				num2 *= this.hediffs[j].PainFactor;
			}
			return Mathf.Clamp(num2, 0f, 1f);
		}

		
		public void Clear()
		{
			this.hediffs.Clear();
			this.DirtyCache();
		}

		
		public Pawn pawn;

		
		public List<Hediff> hediffs = new List<Hediff>();

		
		private List<Hediff_MissingPart> cachedMissingPartsCommonAncestors;

		
		private float cachedPain = -1f;

		
		private float cachedBleedRate = -1f;

		
		private bool? cachedHasHead;

		
		private Stack<BodyPartRecord> coveragePartsStack = new Stack<BodyPartRecord>();

		
		private HashSet<BodyPartRecord> coverageRejectedPartsSet = new HashSet<BodyPartRecord>();

		
		private Queue<BodyPartRecord> missingPartsCommonAncestorsQueue = new Queue<BodyPartRecord>();
	}
}
