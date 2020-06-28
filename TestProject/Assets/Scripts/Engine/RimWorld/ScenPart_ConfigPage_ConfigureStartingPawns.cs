using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C26 RID: 3110
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage
	{
		// Token: 0x06004A2A RID: 18986 RVA: 0x00191428 File Offset: 0x0018F628
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			base.DoEditInterface(listing);
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_StartWithPawns_OutOf".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnCount, ref this.pawnCountBuffer, 1f, 10f);
			scenPartRect.y += ScenPart.RowHeight;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnChoiceCount, ref this.pawnCountChoiceBuffer, (float)this.pawnCount, 10f);
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00191502 File Offset: 0x0018F702
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x0019152E File Offset: 0x0018F72E
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithPawns".Translate(this.pawnCount, this.pawnChoiceCount);
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x00191555 File Offset: 0x0018F755
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x0019156C File Offset: 0x0018F76C
		public override void PostWorldGenerate()
		{
			Find.GameInitData.startingPawnCount = this.pawnCount;
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				for (int i = 0; i < this.pawnCount; i++)
				{
					Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
				}
				num++;
				if (num > 20)
				{
					break;
				}
			}
			while (!StartingPawnUtility.WorkTypeRequirementsSatisfied());
			IL_62:
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
			}
			return;
			goto IL_62;
		}

		// Token: 0x04002A1B RID: 10779
		public int pawnCount = 3;

		// Token: 0x04002A1C RID: 10780
		public int pawnChoiceCount = 10;

		// Token: 0x04002A1D RID: 10781
		private string pawnCountBuffer;

		// Token: 0x04002A1E RID: 10782
		private string pawnCountChoiceBuffer;

		// Token: 0x04002A1F RID: 10783
		private const int MaxPawnCount = 10;

		// Token: 0x04002A20 RID: 10784
		private const int MaxPawnChoiceCount = 10;
	}
}
