using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public struct ShotReport
	{
		
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000CDE28 File Offset: 0x000CC028
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 0.2f;
					}
				}
				return 1f;
			}
		}

		
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x000CDE78 File Offset: 0x000CC078
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 7.5f;
					}
				}
				return 1f;
			}
		}

		
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x000CDEC6 File Offset: 0x000CC0C6
		private float FactorFromCoveringGas
		{
			get
			{
				if (this.coveringGas != null)
				{
					return 1f - this.coveringGas.gas.accuracyPenalty;
				}
				return 1f;
			}
		}

		
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x000CDEEC File Offset: 0x000CC0EC
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.FactorFromCoveringGas * this.FactorFromExecution;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x000CDF2B File Offset: 0x000CC12B
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000CDF3A File Offset: 0x000CC13A
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000CDF49 File Offset: 0x000CC149
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000CDF57 File Offset: 0x000CC157
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000CDF6B File Offset: 0x000CC16B
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			IntVec3 cell = target.Cell;
			ShotReport shotReport;
			shotReport.distance = (cell - caster.Position).LengthHorizontal;
			shotReport.target = target.ToTargetInfo(caster.Map);
			shotReport.factorFromShooterAndDist = ShotReport.HitFactorFromShooter(caster, shotReport.distance);
			shotReport.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.EquipmentSource, shotReport.distance);
			shotReport.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			shotReport.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			shotReport.coveringGas = null;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out shotReport.shootLine))
			{
				using (IEnumerator<IntVec3> enumerator = shotReport.shootLine.Points().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						Thing gas = c.GetGas(caster.Map);
						if (gas != null && (shotReport.coveringGas == null || shotReport.coveringGas.gas.accuracyPenalty < gas.def.gas.accuracyPenalty))
						{
							shotReport.coveringGas = gas.def;
						}
					}
					goto IL_14B;
				}
			}
			shotReport.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			IL_14B:
			if (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map))
			{
				shotReport.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				shotReport.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					shotReport.factorFromTargetSize = pawn.BodySize;
				}
				else
				{
					shotReport.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 2.5f;
				}
				shotReport.factorFromTargetSize = Mathf.Clamp(shotReport.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				shotReport.factorFromTargetSize = 1f;
			}
			shotReport.forcedMissRadius = verb.verbProps.forcedMissRadius;
			return shotReport;
		}

		
		public static float HitFactorFromShooter(Thing caster, float distance)
		{
			return ShotReport.HitFactorFromShooter((caster is Pawn) ? caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true) : caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true), distance);
		}

		
		public static float HitFactorFromShooter(float accRating, float distance)
		{
			return Mathf.Max(Mathf.Pow(accRating, distance), 0.0201f);
		}

		
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + "   " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + "   " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(" " + this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + "  " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + "        " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing && this.factorFromTargetSize != 1f)
				{
					stringBuilder.AppendLine("   " + "TargetSize".Translate() + "       " + this.factorFromTargetSize.ToStringPercent());
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + "         " + this.factorFromWeather.ToStringPercent());
				}
				if (this.FactorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + this.coveringGas.LabelCap + "         " + this.FactorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + "  " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + "   " + this.FactorFromExecution.ToStringPercent());
				}
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + "        " + this.PassCoverChance.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						if (coverInfo.BlockChance > 0f)
						{
							stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(coverInfo.Thing.LabelCap, coverInfo.BlockChance.ToStringPercent(), new NamedArgument(coverInfo.Thing.def, "COVER")).CapitalizeFirst());
						}
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		
		private TargetInfo target;

		
		private float distance;

		
		private List<CoverInfo> covers;

		
		private float coversOverallBlockChance;

		
		private ThingDef coveringGas;

		
		private float factorFromShooterAndDist;

		
		private float factorFromEquipment;

		
		private float factorFromTargetSize;

		
		private float factorFromWeather;

		
		private float forcedMissRadius;

		
		private ShootLine shootLine;
	}
}
