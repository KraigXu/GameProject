using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED8 RID: 3800
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x06005D22 RID: 23842 RVA: 0x00204574 File Offset: 0x00202774
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (!allColonistBondsFor.Any<Pawn>())
			{
				return null;
			}
			if (allColonistBondsFor.Any((Pawn bond) => bond == pawn.playerSettings.Master))
			{
				return PawnColumnWorker_Bond.BondIcon;
			}
			return PawnColumnWorker_Bond.BondBrokenIcon;
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x002045C3 File Offset: 0x002027C3
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x002045CC File Offset: 0x002027CC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x002045F0 File Offset: 0x002027F0
		public int GetCompareValueFor(Pawn a)
		{
			Texture2D iconFor = this.GetIconFor(a);
			if (iconFor == null)
			{
				return 0;
			}
			if (iconFor == PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return 1;
			}
			if (iconFor == PawnColumnWorker_Bond.BondIcon)
			{
				return 2;
			}
			Log.ErrorOnce("Unknown bond type when trying to sort", 20536378, false);
			return 0;
		}

		// Token: 0x06005D26 RID: 23846 RVA: 0x00204640 File Offset: 0x00202840
		protected override void PaintedIcon(Pawn pawn)
		{
			if (this.GetIconFor(pawn) != PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return;
			}
			if (!pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				return;
			}
			pawn.playerSettings.Master = (from master in TrainableUtility.GetAllColonistBondsFor(pawn)
			where TrainableUtility.CanBeMaster(master, pawn, true)
			select master).FirstOrDefault<Pawn>();
		}

		// Token: 0x040032BE RID: 12990
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x040032BF RID: 12991
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
