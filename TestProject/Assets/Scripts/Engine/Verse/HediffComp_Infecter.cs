using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000261 RID: 609
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x0005E729 File Offset: 0x0005C929
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0005E738 File Offset: 0x0005C938
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			float num = this.Props.infectionChance;
			if (base.Pawn.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			if (Rand.Value <= num)
			{
				this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				return;
			}
			this.ticksUntilInfect = -2;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0005E810 File Offset: 0x0005CA10
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0005E83B File Offset: 0x0005CA3B
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect--;
				if (this.ticksUntilInfect == 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0005E864 File Offset: 0x0005CA64
		public override void CompTended(float quality, int batchPosition = 0)
		{
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0005E8A0 File Offset: 0x0005CAA0
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			float num = 1f;
			HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
			{
				num *= this.infectionChanceFactorFromTendRoom;
				num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
			}
			num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(this.parent.Severity);
			if (base.Pawn.Faction == Faction.OfPlayer)
			{
				num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
			}
			if (Rand.Value < num)
			{
				this.ticksUntilInfect = -4;
				base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, this.parent.Part, null, null);
				return;
			}
			this.ticksUntilInfect = -3;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0005E99C File Offset: 0x0005CB9C
		public override string CompDebugString()
		{
			if (this.ticksUntilInfect > 0)
			{
				return string.Concat(new object[]
				{
					"infection may appear in: ",
					this.ticksUntilInfect,
					" ticks\ninfectChnceFactorFromTendRoom: ",
					this.infectionChanceFactorFromTendRoom.ToStringPercent()
				});
			}
			if (this.ticksUntilInfect == -4)
			{
				return "already created infection";
			}
			if (this.ticksUntilInfect == -3)
			{
				return "failed to make infection";
			}
			if (this.ticksUntilInfect == -2)
			{
				return "will not make infection";
			}
			if (this.ticksUntilInfect == -1)
			{
				return "uninitialized data!";
			}
			return "unexpected ticksUntilInfect = " + this.ticksUntilInfect;
		}

		// Token: 0x04000C14 RID: 3092
		private int ticksUntilInfect = -1;

		// Token: 0x04000C15 RID: 3093
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x04000C16 RID: 3094
		private const int UninitializedValue = -1;

		// Token: 0x04000C17 RID: 3095
		private const int WillNotInfectValue = -2;

		// Token: 0x04000C18 RID: 3096
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x04000C19 RID: 3097
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x04000C1A RID: 3098
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x04000C1B RID: 3099
		private static readonly SimpleCurve InfectionChanceFactorFromSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.1f),
				true
			},
			{
				new CurvePoint(12f, 1f),
				true
			}
		};
	}
}
