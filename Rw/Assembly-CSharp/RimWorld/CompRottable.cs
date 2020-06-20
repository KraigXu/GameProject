using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D48 RID: 3400
	public class CompRottable : ThingComp
	{
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060052A7 RID: 21159 RVA: 0x001B9E3F File Offset: 0x001B803F
		public CompProperties_Rottable PropsRot
		{
			get
			{
				return (CompProperties_Rottable)this.props;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060052A8 RID: 21160 RVA: 0x001B9E4C File Offset: 0x001B804C
		public float RotProgressPct
		{
			get
			{
				return this.RotProgress / (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x001B9E61 File Offset: 0x001B8061
		// (set) Token: 0x060052AA RID: 21162 RVA: 0x001B9E69 File Offset: 0x001B8069
		public float RotProgress
		{
			get
			{
				return this.rotProgressInt;
			}
			set
			{
				RotStage stage = this.Stage;
				this.rotProgressInt = value;
				if (stage != this.Stage)
				{
					this.StageChanged();
				}
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060052AB RID: 21163 RVA: 0x001B9E86 File Offset: 0x001B8086
		public RotStage Stage
		{
			get
			{
				if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
				{
					return RotStage.Fresh;
				}
				if (this.RotProgress < (float)this.PropsRot.TicksToDessicated)
				{
					return RotStage.Rotting;
				}
				return RotStage.Dessicated;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060052AC RID: 21164 RVA: 0x001B9EB8 File Offset: 0x001B80B8
		public int TicksUntilRotAtCurrentTemp
		{
			get
			{
				float num = this.parent.AmbientTemperature;
				num = (float)Mathf.RoundToInt(num);
				return this.TicksUntilRotAtTemp(num);
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060052AD RID: 21165 RVA: 0x001B9EE0 File Offset: 0x001B80E0
		public bool Active
		{
			get
			{
				if (this.PropsRot.disableIfHatcher)
				{
					CompHatcher compHatcher = this.parent.TryGetComp<CompHatcher>();
					if (compHatcher != null && !compHatcher.TemperatureDamaged)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x001B9F14 File Offset: 0x001B8114
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x001B9F1D File Offset: 0x001B811D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.rotProgressInt, "rotProg", 0f, false);
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x001B9F3B File Offset: 0x001B813B
		public override void CompTick()
		{
			this.Tick(1);
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x001B9F44 File Offset: 0x001B8144
		public override void CompTickRare()
		{
			this.Tick(250);
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x001B9F54 File Offset: 0x001B8154
		private void Tick(int interval)
		{
			if (!this.Active)
			{
				return;
			}
			float rotProgress = this.RotProgress;
			float num = GenTemperature.RotRateAtTemperature(this.parent.AmbientTemperature);
			this.RotProgress += num * (float)interval;
			if (this.Stage == RotStage.Rotting && this.PropsRot.rotDestroys)
			{
				if (this.parent.IsInAnyStorage() && this.parent.SpawnedOrAnyParentSpawned)
				{
					Messages.Message("MessageRottedAwayInStorage".Translate(this.parent.Label, this.parent).CapitalizeFirst(), new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false), MessageTypeDefOf.NegativeEvent, true);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.SpoilageAndFreezers, OpportunityType.GoodToKnow);
				}
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Mathf.FloorToInt(rotProgress / 60000f) != Mathf.FloorToInt(this.RotProgress / 60000f) && this.ShouldTakeRotDamage())
			{
				if (this.Stage == RotStage.Rotting && this.PropsRot.rotDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.rotDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					return;
				}
				if (this.Stage == RotStage.Dessicated && this.PropsRot.dessicatedDamagePerDay > 0f)
				{
					this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)GenMath.RoundRandom(this.PropsRot.dessicatedDamagePerDay), 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				}
			}
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x001BA114 File Offset: 0x001B8314
		private bool ShouldTakeRotDamage()
		{
			Thing thing = this.parent.ParentHolder as Thing;
			return thing == null || thing.def.category != ThingCategory.Building || !thing.def.building.preventDeteriorationInside;
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x001BA158 File Offset: 0x001B8358
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			float rotProgress = ((ThingWithComps)otherStack).GetComp<CompRottable>().RotProgress;
			this.RotProgress = Mathf.Lerp(this.RotProgress, rotProgress, t);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x001BA19B File Offset: 0x001B839B
		public override void PostSplitOff(Thing piece)
		{
			((ThingWithComps)piece).GetComp<CompRottable>().RotProgress = this.RotProgress;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x001BA1B3 File Offset: 0x001B83B3
		public override void PostIngested(Pawn ingester)
		{
			if (this.Stage != RotStage.Fresh && FoodUtility.GetFoodPoisonChanceFactor(ingester) > 1.401298E-45f)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, FoodPoisonCause.Rotten);
			}
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x001BA1D8 File Offset: 0x001B83D8
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.Stage)
			{
			case RotStage.Fresh:
				stringBuilder.Append("RotStateFresh".Translate() + ".");
				break;
			case RotStage.Rotting:
				stringBuilder.Append("RotStateRotting".Translate() + ".");
				break;
			case RotStage.Dessicated:
				stringBuilder.Append("RotStateDessicated".Translate() + ".");
				break;
			}
			if ((float)this.PropsRot.TicksToRotStart - this.RotProgress > 0f)
			{
				float num = GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(this.parent.AmbientTemperature));
				int ticksUntilRotAtCurrentTemp = this.TicksUntilRotAtCurrentTemp;
				stringBuilder.AppendLine();
				if (num < 0.001f)
				{
					stringBuilder.Append("CurrentlyFrozen".Translate() + ".");
				}
				else if (num < 0.999f)
				{
					stringBuilder.Append("CurrentlyRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true)) + ".");
				}
				else
				{
					stringBuilder.Append("NotRefrigerated".Translate(ticksUntilRotAtCurrentTemp.ToStringTicksToPeriod(true, false, true, true)) + ".");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x001BA350 File Offset: 0x001B8550
		public int ApproxTicksUntilRotWhenAtTempOfTile(int tile, int ticksAbs)
		{
			float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs, tile);
			return this.TicksUntilRotAtTemp(temperatureFromSeasonAtTile);
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x001BA36C File Offset: 0x001B856C
		public int TicksUntilRotAtTemp(float temp)
		{
			if (!this.Active)
			{
				return 72000000;
			}
			float num = GenTemperature.RotRateAtTemperature(temp);
			if (num <= 0f)
			{
				return 72000000;
			}
			float num2 = (float)this.PropsRot.TicksToRotStart - this.RotProgress;
			if (num2 <= 0f)
			{
				return 0;
			}
			return Mathf.RoundToInt(num2 / num);
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x001BA3C4 File Offset: 0x001B85C4
		private void StageChanged()
		{
			Corpse corpse = this.parent as Corpse;
			if (corpse != null)
			{
				corpse.RotStageChanged();
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x001BA3E6 File Offset: 0x001B85E6
		public void RotImmediately()
		{
			if (this.RotProgress < (float)this.PropsRot.TicksToRotStart)
			{
				this.RotProgress = (float)this.PropsRot.TicksToRotStart;
			}
		}

		// Token: 0x04002DAE RID: 11694
		private float rotProgressInt;
	}
}
