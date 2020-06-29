using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ITab_Pawn_Training : ITab
	{
		
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x001FB564 File Offset: 0x001F9764
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.training != null && base.SelPawn.Faction == Faction.OfPlayer;
			}
		}

		
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, base.SelPawn);
		}

		
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(base.SelPawn));
		}
	}
}
