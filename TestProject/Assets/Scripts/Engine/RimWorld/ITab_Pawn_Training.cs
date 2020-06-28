using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EB2 RID: 3762
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x001FB564 File Offset: 0x001F9764
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x06005BD5 RID: 23509 RVA: 0x001FB587 File Offset: 0x001F9787
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x06005BD6 RID: 23510 RVA: 0x001FB5A8 File Offset: 0x001F97A8
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}

		// Token: 0x06005BD7 RID: 23511 RVA: 0x001FB604 File Offset: 0x001F9804
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(base.SelPawn));
		}
	}
}
