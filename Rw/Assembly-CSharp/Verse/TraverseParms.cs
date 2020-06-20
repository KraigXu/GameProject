using System;

namespace Verse
{
	// Token: 0x02000059 RID: 89
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x060003DB RID: 987 RVA: 0x00013BAC File Offset: 0x00011DAC
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBash = false)
		{
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.", false);
				return TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBash);
			}
			return new TraverseParms
			{
				pawn = pawn,
				maxDanger = maxDanger,
				mode = mode,
				canBash = canBash
			};
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00013BFC File Offset: 0x00011DFC
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBash = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBash = canBash
			};
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00013C32 File Offset: 0x00011E32
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.", false);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00013C4F File Offset: 0x00011E4F
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00013C67 File Offset: 0x00011E67
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00013CA3 File Offset: 0x00011EA3
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00013CE2 File Offset: 0x00011EE2
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00013CFA File Offset: 0x00011EFA
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00013D38 File Offset: 0x00011F38
		public override int GetHashCode()
		{
			int seed = this.canBash ? 1 : 0;
			if (this.pawn != null)
			{
				seed = Gen.HashCombine<Pawn>(seed, this.pawn);
			}
			else
			{
				seed = Gen.HashCombineStruct<TraverseMode>(seed, this.mode);
			}
			return Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00013D84 File Offset: 0x00011F84
		public override string ToString()
		{
			string text = this.canBash ? " canBash" : "";
			if (this.mode == TraverseMode.ByPawn)
			{
				return string.Concat(new object[]
				{
					"(",
					this.mode,
					" ",
					this.maxDanger,
					" ",
					this.pawn,
					text,
					")"
				});
			}
			return string.Concat(new object[]
			{
				"(",
				this.mode,
				" ",
				this.maxDanger,
				text,
				")"
			});
		}

		// Token: 0x04000121 RID: 289
		public Pawn pawn;

		// Token: 0x04000122 RID: 290
		public TraverseMode mode;

		// Token: 0x04000123 RID: 291
		public Danger maxDanger;

		// Token: 0x04000124 RID: 292
		public bool canBash;
	}
}
