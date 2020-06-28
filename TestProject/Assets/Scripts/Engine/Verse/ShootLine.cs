using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000478 RID: 1144
	public struct ShootLine
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000CDA00 File Offset: 0x000CBC00
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x000CDA08 File Offset: 0x000CBC08
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000CDA10 File Offset: 0x000CBC10
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000CDA20 File Offset: 0x000CBC20
		public void ChangeDestToMissWild(float aimOnChance)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089, false);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000CDAD1 File Offset: 0x000CBCD1
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000CDAE4 File Offset: 0x000CBCE4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.source,
				"->",
				this.dest,
				")"
			});
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000CDB30 File Offset: 0x000CBD30
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				if (cells == 0)
				{
					return num2.ToStringPercent();
				}
				return ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
			}, "");
		}

		// Token: 0x040014A8 RID: 5288
		private IntVec3 source;

		// Token: 0x040014A9 RID: 5289
		private IntVec3 dest;
	}
}
