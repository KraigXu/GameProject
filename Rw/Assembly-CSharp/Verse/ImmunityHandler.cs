using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028B RID: 651
	public class ImmunityHandler : IExposable
	{
		// Token: 0x0600116B RID: 4459 RVA: 0x000625FD File Offset: 0x000607FD
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00062617 File Offset: 0x00060817
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00062630 File Offset: 0x00060830
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x0006264C File Offset: 0x0006084C
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

		// Token: 0x0600116F RID: 4463 RVA: 0x00062744 File Offset: 0x00060944
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

		// Token: 0x06001170 RID: 4464 RVA: 0x000627A8 File Offset: 0x000609A8
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

		// Token: 0x06001171 RID: 4465 RVA: 0x000628C4 File Offset: 0x00060AC4
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

		// Token: 0x06001172 RID: 4466 RVA: 0x0006294C File Offset: 0x00060B4C
		[Obsolete("Will be removed in a future update, use AnyHediffMakesFullyImmuneTo_NewTemp")]
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def)
		{
			Hediff hediff;
			return this.AnyHediffMakesFullyImmuneTo_NewTemp(def, out hediff);
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00062964 File Offset: 0x00060B64
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

		// Token: 0x06001174 RID: 4468 RVA: 0x000629E4 File Offset: 0x00060BE4
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

		// Token: 0x06001175 RID: 4469 RVA: 0x00062A24 File Offset: 0x00060C24
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

		// Token: 0x06001176 RID: 4470 RVA: 0x00062AAC File Offset: 0x00060CAC
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x04000C6F RID: 3183
		public Pawn pawn;

		// Token: 0x04000C70 RID: 3184
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x04000C71 RID: 3185
		private const float ForcedImmunityLevel = 0.650000036f;

		// Token: 0x04000C72 RID: 3186
		private static List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x02001451 RID: 5201
		public struct ImmunityInfo
		{
			// Token: 0x04004D26 RID: 19750
			public HediffDef immunity;

			// Token: 0x04004D27 RID: 19751
			public HediffDef source;
		}
	}
}
