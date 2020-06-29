using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class ImmunityHandler : IExposable
	{
		
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(diseaseDef, out hediff))
			{
				immunityCause = hediff.def;
				return 0f;
			}
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
				{
					return 0f;
				}
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				if (this.immunityList[j].hediffDef == diseaseDef)
				{
					immunityCause = this.immunityList[j].source;
					return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
				}
			}
			return 1f;
		}

		
		public float GetImmunity(HediffDef def)
		{
			float num = 0f;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					num = immunityRecord.immunity;
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff) && num < 0.650000036f)
			{
				num = 0.650000036f;
			}
			return num;
		}

		
		internal void ImmunityHandlerTick()
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTick(this.pawn, firstHediffOfDef != null, firstHediffOfDef);
			}
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		
		[Obsolete("Will be removed in a future update, use AnyHediffMakesFullyImmuneTo_NewTemp")]
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def)
		{
			Hediff hediff;
			return this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff);
		}

		
		private bool AnyHediffMakesFullyImmuneTo_NewTemp(HediffDef def, out Hediff sourceHediff)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							sourceHediff = hediffs[i];
							return true;
						}
					}
				}
			}
			sourceHediff = null;
			return false;
		}

		
		private void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() == null)
			{
				return;
			}
			if (this.ImmunityRecordExists(def))
			{
				return;
			}
			ImmunityRecord immunityRecord = new ImmunityRecord();
			immunityRecord.hediffDef = def;
			immunityRecord.source = source;
			this.immunityList.Add(immunityRecord);
		}

		
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			ImmunityRecord immunityRecord = null;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					immunityRecord = this.immunityList[i];
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.650000036f))
			{
				immunityRecord = new ImmunityRecord
				{
					immunity = 0.650000036f,
					hediffDef = def,
					source = hediff.def
				};
			}
			return immunityRecord;
		}

		
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		
		public Pawn pawn;

		
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		
		private const float ForcedImmunityLevel = 0.650000036f;

		
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		
		public struct ImmunityInfo
		{
			
			public HediffDef immunity;

			
			public HediffDef source;
		}
	}
}
