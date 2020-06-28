using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047A RID: 1146
	public struct ShotReport
	{
		// Token: 0x17000693 RID: 1683
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

		// Token: 0x17000694 RID: 1684
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

		// Token: 0x17000695 RID: 1685
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

		// Token: 0x17000696 RID: 1686
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

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x000CDF2B File Offset: 0x000CC12B
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000CDF3A File Offset: 0x000CC13A
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000CDF49 File Offset: 0x000CC149
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000CDF57 File Offset: 0x000CC157
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060021D3 RID: 8659 RVA: 0x000CDF6B File Offset: 0x000CC16B
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000CDF74 File Offset: 0x000CC174
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

		// Token: 0x060021D5 RID: 8661 RVA: 0x000CE1E4 File Offset: 0x000CC3E4
		public static float HitFactorFromShooter(Thing caster, float distance)
		{
			return ShotReport.HitFactorFromShooter((caster is Pawn) ? caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true) : caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true), distance);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000CE20E File Offset: 0x000CC40E
		public static float HitFactorFromShooter(float accRating, float distance)
		{
			return Mathf.Max(Mathf.Pow(accRating, distance), 0.0201f);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000CE224 File Offset: 0x000CC424
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

		// Token: 0x060021D8 RID: 8664 RVA: 0x000CE5D4 File Offset: 0x000CC7D4
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x040014C7 RID: 5319
		private TargetInfo target;

		// Token: 0x040014C8 RID: 5320
		private float distance;

		// Token: 0x040014C9 RID: 5321
		private List<CoverInfo> covers;

		// Token: 0x040014CA RID: 5322
		private float coversOverallBlockChance;

		// Token: 0x040014CB RID: 5323
		private ThingDef coveringGas;

		// Token: 0x040014CC RID: 5324
		private float factorFromShooterAndDist;

		// Token: 0x040014CD RID: 5325
		private float factorFromEquipment;

		// Token: 0x040014CE RID: 5326
		private float factorFromTargetSize;

		// Token: 0x040014CF RID: 5327
		private float factorFromWeather;

		// Token: 0x040014D0 RID: 5328
		private float forcedMissRadius;

		// Token: 0x040014D1 RID: 5329
		private ShootLine shootLine;
	}
}
