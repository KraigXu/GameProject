using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7B RID: 2939
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x001738BF File Offset: 0x00171ABF
		// (set) Token: 0x060044CF RID: 17615 RVA: 0x001738E4 File Offset: 0x00171AE4
		public DrugPolicy CurrentPolicy
		{
			get
			{
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				if (this.curPolicy == value)
				{
					return;
				}
				this.curPolicy = value;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x060044D0 RID: 17616 RVA: 0x001738F8 File Offset: 0x00171AF8
		private float DayPercentNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				if (this.pawn.timetable == null)
				{
					return GenLocalDate.DayPercent(this.pawn);
				}
				float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
				if (hoursPerDayNotSleeping == 0f)
				{
					return 1f;
				}
				float num = 0f;
				int num2 = GenLocalDate.HourOfDay(this.pawn);
				for (int i = 0; i < num2; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num += 1f;
					}
				}
				if (this.pawn.timetable.CurrentAssignment != TimeAssignmentDefOf.Sleep)
				{
					float num3 = (float)(Find.TickManager.TicksAbs % 2500) / 2500f;
					num += num3;
				}
				return num / hoursPerDayNotSleeping;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x060044D1 RID: 17617 RVA: 0x001739D8 File Offset: 0x00171BD8
		private float HoursPerDayNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return 16f;
				}
				int num = 0;
				for (int i = 0; i < 24; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num++;
					}
				}
				return (float)num;
			}
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x00173A2A File Offset: 0x00171C2A
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x00173A3D File Offset: 0x00171C3D
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x00173A57 File Offset: 0x00171C57
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x00173A80 File Offset: 0x00171C80
		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				Log.Warning(drug + " is not a drug.", false);
				return false;
			}
			return this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x00173AD8 File Offset: 0x00171CD8
		public bool AllowedToTakeToInventory(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				return false;
			}
			if (thingDef.IsNonMedicalDrug && this.pawn.IsTeetotaler())
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			return !drugPolicyEntry.allowScheduled && drugPolicyEntry.takeToInventory > 0 && !this.pawn.inventory.innerContainer.Contains(thingDef);
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x00173B70 File Offset: 0x00171D70
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				return false;
			}
			return this.CurrentPolicy[thingDef].allowScheduled && (!thingDef.IsNonMedicalDrug || !this.pawn.IsTeetotaler());
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x00173BE0 File Offset: 0x00171DE0
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(thingDef + " is not ingestible.", false);
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + thingDef, false);
				return false;
			}
			if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (drugPolicyEntry.onlyIfMoodBelow < 1f && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
			{
				return false;
			}
			if (drugPolicyEntry.onlyIfJoyBelow < 1f && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
			{
				return false;
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == thingDef);
			if (drugTakeRecord != null)
			{
				if (drugPolicyEntry.daysFrequency < 1f)
				{
					int num = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
					if (drugTakeRecord.TimesTakenThisDay >= num)
					{
						return false;
					}
				}
				else
				{
					int num2 = Mathf.Abs(GenDate.DaysPassed - drugTakeRecord.LastTakenDays);
					int num3 = Mathf.RoundToInt(drugPolicyEntry.daysFrequency);
					if (num2 < num3)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00173D4C File Offset: 0x00171F4C
		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			if (!ingestible.IsDrug)
			{
				return false;
			}
			if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				return false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5f && this.CanCauseOverdose(ingestible))
			{
				int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
				if (Find.TickManager.TicksGame - num < 1250)
				{
					return false;
				}
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == ingestible);
			if (drugTakeRecord == null)
			{
				return true;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
			if (drugPolicyEntry.daysFrequency < 1f)
			{
				int num2 = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
				float num3 = 1f / (float)(num2 + 1);
				int num4 = 0;
				float dayPercentNotSleeping = this.DayPercentNotSleeping;
				for (int i = 0; i < num2; i++)
				{
					if (dayPercentNotSleeping > (float)(i + 1) * num3 - num3 * 0.5f)
					{
						num4++;
					}
				}
				return drugTakeRecord.TimesTakenThisDay < num4 && (drugTakeRecord.TimesTakenThisDay == 0 || (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500f) >= 0.6f * num3);
			}
			float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
			bool result = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
			Rand.PopState();
			return result;
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00173EF4 File Offset: 0x001720F4
		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == drug.def);
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			DrugTakeRecord drugTakeRecord2 = drugTakeRecord;
			int timesTakenThisDay = drugTakeRecord2.TimesTakenThisDay;
			drugTakeRecord2.TimesTakenThisDay = timesTakenThisDay + 1;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x00173F6C File Offset: 0x0017216C
		private int LastTicksWhenTakenDrugWhichCanCauseOverdose()
		{
			int num = -999999;
			for (int i = 0; i < this.drugTakeRecords.Count; i++)
			{
				if (this.CanCauseOverdose(this.drugTakeRecords[i].drug))
				{
					num = Mathf.Max(num, this.drugTakeRecords[i].lastTakenTicks);
				}
			}
			return num;
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x00173FC8 File Offset: 0x001721C8
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x04002750 RID: 10064
		public Pawn pawn;

		// Token: 0x04002751 RID: 10065
		private DrugPolicy curPolicy;

		// Token: 0x04002752 RID: 10066
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x04002753 RID: 10067
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
