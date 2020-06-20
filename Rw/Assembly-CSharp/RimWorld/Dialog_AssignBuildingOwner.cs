using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E4F RID: 3663
	public class Dialog_AssignBuildingOwner : Window
	{
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06005883 RID: 22659 RVA: 0x001D60A3 File Offset: 0x001D42A3
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 500f);
			}
		}

		// Token: 0x06005884 RID: 22660 RVA: 0x001D60B4 File Offset: 0x001D42B4
		public Dialog_AssignBuildingOwner(CompAssignableToPawn assignable)
		{
			this.assignable = assignable;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06005885 RID: 22661 RVA: 0x001D60E0 File Offset: 0x001D42E0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			outRect.width -= 16f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)this.assignable.AssigningCandidates.Count<Pawn>() * 35f + 100f);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			try
			{
				float num = 0f;
				bool flag = false;
				foreach (Pawn pawn in this.assignable.AssignedPawns)
				{
					flag = true;
					Rect rect = new Rect(0f, num, viewRect.width * 0.7f, 32f);
					Widgets.Label(rect, pawn.LabelCap);
					rect.x = rect.xMax;
					rect.width = viewRect.width * 0.3f;
					if (Widgets.ButtonText(rect, "BuildingUnassign".Translate(), true, true, true))
					{
						this.assignable.TryUnassignPawn(pawn, true);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					num += 35f;
				}
				if (flag)
				{
					num += 15f;
				}
				foreach (Pawn pawn2 in this.assignable.AssigningCandidates)
				{
					if (!this.assignable.AssignedPawns.Contains(pawn2))
					{
						AcceptanceReport acceptanceReport = this.assignable.CanAssignTo(pawn2);
						bool accepted = acceptanceReport.Accepted;
						string text = pawn2.LabelCap + (accepted ? "" : (" (" + acceptanceReport.Reason.StripTags() + ")"));
						float width = viewRect.width * 0.7f;
						float num2 = Text.CalcHeight(text, width);
						float num3 = (35f > num2) ? 35f : num2;
						Rect rect2 = new Rect(0f, num, width, num3);
						if (!accepted)
						{
							GUI.color = Color.gray;
						}
						Widgets.Label(rect2, text);
						rect2.x = rect2.xMax;
						rect2.width = viewRect.width * 0.3f;
						rect2.height = 35f;
						TaggedString taggedString = this.assignable.AssignedAnything(pawn2) ? "BuildingReassign".Translate() : "BuildingAssign".Translate();
						if (Widgets.ButtonText(rect2, taggedString, true, true, accepted))
						{
							this.assignable.TryAssignPawn(pawn2);
							if (this.assignable.MaxAssignedPawnsCount == 1)
							{
								this.Close(true);
								break;
							}
							SoundDefOf.Click.PlayOneShotOnCamera(null);
							break;
						}
						else
						{
							GUI.color = Color.white;
							num += num3;
						}
					}
				}
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		// Token: 0x04002FBE RID: 12222
		private CompAssignableToPawn assignable;

		// Token: 0x04002FBF RID: 12223
		private Vector2 scrollPosition;

		// Token: 0x04002FC0 RID: 12224
		private const float EntryHeight = 35f;

		// Token: 0x04002FC1 RID: 12225
		private const float LineSpacing = 8f;
	}
}
