using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE8 RID: 3816
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x06005D81 RID: 23937 RVA: 0x00205239 File Offset: 0x00203439
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
		}

		// Token: 0x06005D82 RID: 23938 RVA: 0x00205251 File Offset: 0x00203451
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06005D83 RID: 23939 RVA: 0x0020526B File Offset: 0x0020346B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06005D84 RID: 23940 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005D85 RID: 23941 RVA: 0x0020527C File Offset: 0x0020347C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005D86 RID: 23942 RVA: 0x0020529F File Offset: 0x0020349F
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return int.MinValue;
			}
			return (int)pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x040032CB RID: 13003
		private const int TopPadding = 3;

		// Token: 0x040032CC RID: 13004
		private const int Width = 24;
	}
}
