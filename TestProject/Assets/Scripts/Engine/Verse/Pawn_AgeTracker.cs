using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class Pawn_AgeTracker : IExposable
	{
		
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x0006BB0F File Offset: 0x00069D0F
		// (set) Token: 0x060012BF RID: 4799 RVA: 0x0006BB17 File Offset: 0x00069D17
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x0006BB20 File Offset: 0x00069D20
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x0006BB30 File Offset: 0x00069D30
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x0006BB3F File Offset: 0x00069D3F
		// (set) Token: 0x060012C3 RID: 4803 RVA: 0x0006BB47 File Offset: 0x00069D47
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.cachedLifeStageIndex = -1;
			}
		}

		
		// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0006BB57 File Offset: 0x00069D57
		// (set) Token: 0x060012C5 RID: 4805 RVA: 0x0006BB66 File Offset: 0x00069D66
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0006BB76 File Offset: 0x00069D76
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0006BB86 File Offset: 0x00069D86
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0006BB95 File Offset: 0x00069D95
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0006BBA7 File Offset: 0x00069DA7
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x0006BBB9 File Offset: 0x00069DB9
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0006BBCB File Offset: 0x00069DCB
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x0006BBE0 File Offset: 0x00069DE0
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					text = string.Concat(new object[]
					{
						text,
						" (",
						this.AgeChronologicalYears,
						")"
					});
				}
				return text;
			}
		}

		
		// (get) Token: 0x060012CD RID: 4813 RVA: 0x0006BC38 File Offset: 0x00069E38
		public string AgeTooltipString
		{
			get
			{
				int value;
				int value2;
				int value3;
				float num;
				this.ageBiologicalTicksInt.TicksToPeriod(out value, out value2, out value3, out num);
				int value4;
				int value5;
				int value6;
				((long)GenTicks.TicksAbs - this.birthAbsTicksInt).TicksToPeriod(out value4, out value5, out value6, out num);
				string value7 = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1, Gender.None), this.BirthQuadrum.Label(), this.BirthYear);
				string text = "Born".Translate(value7) + "\n" + "AgeChronological".Translate(value4, value5, value6) + "\n" + "AgeBiological".Translate(value, value2, value3);
				if (Prefs.DevMode)
				{
					text += "\n\nDev mode info:";
					text = text + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt;
					text = text + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt;
					text = text + "\nnextLifeStageChangeTick: " + this.nextLifeStageChangeTick;
				}
				return text;
			}
		}

		
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x0006BD88 File Offset: 0x00069F88
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0006BD9F File Offset: 0x00069F9F
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0006BDAC File Offset: 0x00069FAC
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0006BDCC File Offset: 0x00069FCC
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811, false);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
		}

		
		public void AgeTick()
		{
			this.ageBiologicalTicksInt += 1L;
			if ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			if (this.ageBiologicalTicksInt % 3600000L == 0L)
			{
				this.BirthdayBiological();
			}
		}

		
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.ageBiologicalTicksInt += (long)interval;
			while ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			int num2 = (int)(num / 3600000L);
			while ((long)num2 < this.ageBiologicalTicksInt / 3600000L)
			{
				this.BirthdayBiological();
				num2 += 3600000;
			}
		}

		
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			for (int i = lifeStageAges.Count - 1; i >= 0; i--)
			{
				if (lifeStageAges[i].minAge <= this.AgeBiologicalYearsFloat + 1E-06f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag && !this.pawn.RaceProps.Humanlike)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
				});
				this.CheckChangePawnKindName();
			}
			if (this.cachedLifeStageIndex < lifeStageAges.Count - 1)
			{
				float num2 = lifeStageAges[this.cachedLifeStageIndex + 1].minAge - this.AgeBiologicalYearsFloat;
				int num3 = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
				this.nextLifeStageChangeTick = (long)num3 + (long)Mathf.Ceil(num2 * 3600000f);
				return;
			}
			this.nextLifeStageChangeTick = long.MaxValue;
		}

		
		private void BirthdayBiological()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, this.AgeBiologicalYears))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("    - " + hediffGiver_Birthday.hediff.LabelCap);
				}
			}
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn) && stringBuilder.Length > 0)
			{
				string str = "BirthdayBiologicalAgeInjuries".Translate(this.pawn, this.AgeBiologicalYears, stringBuilder).AdjustedFor(this.pawn, "PAWN", true);
				Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), str, LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
			}
		}

		
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

		
		private void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle == null || !nameSingle.Numerical)
			{
				return;
			}
			string kindLabel = this.pawn.KindLabel;
			if (nameSingle.NameWithoutNumber == kindLabel)
			{
				return;
			}
			int number = nameSingle.Number;
			string text = this.pawn.KindLabel + " " + number;
			if (!NameUseChecker.NameSingleIsUsed(text))
			{
				this.pawn.Name = new NameSingle(text, true);
				return;
			}
			this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null);
		}

		
		public void DebugMake1YearOlder()
		{
			this.ageBiologicalTicksInt += 3600000L;
			this.birthAbsTicksInt -= 3600000L;
			this.RecalculateLifeStageIndex();
		}

		
		private Pawn pawn;

		
		private long ageBiologicalTicksInt = -1L;

		
		private long birthAbsTicksInt = -1L;

		
		private int cachedLifeStageIndex = -1;

		
		private long nextLifeStageChangeTick = -1L;

		
		private const float BornAtLongitude = 0f;
	}
}
