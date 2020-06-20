using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B9B RID: 2971
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x00178291 File Offset: 0x00176491
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x00178299 File Offset: 0x00176499
		public RoomSizeCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RoomSizeCategory.VeryCramped;
				}
				if (this.CurLevel < 0.3f)
				{
					return RoomSizeCategory.Cramped;
				}
				if (this.CurLevel < 0.7f)
				{
					return RoomSizeCategory.Normal;
				}
				return RoomSizeCategory.Spacious;
			}
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x001782C9 File Offset: 0x001764C9
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00178300 File Offset: 0x00176500
		public float SpacePerceptibleNow()
		{
			if (!this.pawn.Spawned)
			{
				return 1f;
			}
			IntVec3 position = this.pawn.Position;
			Need_RoomSize.tempScanRooms.Clear();
			for (int i = 0; i < 5; i++)
			{
				Room room = (position + GenRadial.RadialPattern[i]).GetRoom(this.pawn.Map, RegionType.Set_Passable);
				if (room != null)
				{
					if (i == 0 && room.PsychologicallyOutdoors)
					{
						return 1f;
					}
					if ((i == 0 || room.RegionType != RegionType.Portal) && !Need_RoomSize.tempScanRooms.Contains(room))
					{
						Need_RoomSize.tempScanRooms.Add(room);
					}
				}
			}
			float num = 0f;
			for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
			{
				IntVec3 loc = position + GenRadial.RadialPattern[j];
				if (Need_RoomSize.tempScanRooms.Contains(loc.GetRoom(this.pawn.Map, RegionType.Set_Passable)))
				{
					num += 1f;
				}
			}
			Need_RoomSize.tempScanRooms.Clear();
			return Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
		}

		// Token: 0x04002807 RID: 10247
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x04002808 RID: 10248
		private const float MinCramped = 0.01f;

		// Token: 0x04002809 RID: 10249
		private const float MinNormal = 0.3f;

		// Token: 0x0400280A RID: 10250
		private const float MinSpacious = 0.7f;

		// Token: 0x0400280B RID: 10251
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x0400280C RID: 10252
		private static readonly SimpleCurve RoomCellCountSpaceCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(9f, 0.25f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			},
			{
				new CurvePoint(42f, 0.71f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
