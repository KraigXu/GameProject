using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EEB RID: 3819
	public class PawnColumnWorker_MedicalCare : PawnColumnWorker
	{
		// Token: 0x06005D97 RID: 23959 RVA: 0x002055FA File Offset: 0x002037FA
		public override void DoHeader(Rect rect, PawnTable table)
		{
			MouseoverSounds.DoRegion(rect);
			base.DoHeader(rect, table);
		}

		// Token: 0x06005D98 RID: 23960 RVA: 0x0020404E File Offset: 0x0020224E
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 28);
		}

		// Token: 0x06005D99 RID: 23961 RVA: 0x0020405E File Offset: 0x0020225E
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06005D9A RID: 23962 RVA: 0x0020560A File Offset: 0x0020380A
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			MedicalCareUtility.MedicalCareSelectButton(rect, pawn);
		}

		// Token: 0x06005D9B RID: 23963 RVA: 0x00205613 File Offset: 0x00203813
		public override int Compare(Pawn a, Pawn b)
		{
			return a.playerSettings.medCare.CompareTo(b.playerSettings.medCare);
		}
	}
}
