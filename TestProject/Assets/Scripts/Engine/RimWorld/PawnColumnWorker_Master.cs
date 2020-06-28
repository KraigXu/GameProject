using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EEA RID: 3818
	public class PawnColumnWorker_Master : PawnColumnWorker
	{
		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x06005D8D RID: 23949 RVA: 0x00010306 File Offset: 0x0000E506
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x002054F6 File Offset: 0x002036F6
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 100);
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x00205506 File Offset: 0x00203706
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(170, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x00203F72 File Offset: 0x00202172
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x00205520 File Offset: 0x00203720
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return;
			}
			TrainableUtility.MasterSelectButton(rect.ContractedBy(2f), pawn, true);
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x00205540 File Offset: 0x00203740
		public override int Compare(Pawn a, Pawn b)
		{
			int valueToCompare = this.GetValueToCompare1(a);
			int valueToCompare2 = this.GetValueToCompare1(b);
			if (valueToCompare != valueToCompare2)
			{
				return valueToCompare.CompareTo(valueToCompare2);
			}
			return this.GetValueToCompare2(a).CompareTo(this.GetValueToCompare2(b));
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x0020557D File Offset: 0x0020377D
		private bool CanAssignMaster(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x002055B0 File Offset: 0x002037B0
		private int GetValueToCompare1(Pawn pawn)
		{
			if (!this.CanAssignMaster(pawn))
			{
				return 0;
			}
			if (pawn.playerSettings.Master == null)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x002055CD File Offset: 0x002037CD
		private string GetValueToCompare2(Pawn pawn)
		{
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				return pawn.playerSettings.Master.Label;
			}
			return "";
		}
	}
}
