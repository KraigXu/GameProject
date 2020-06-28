using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000362 RID: 866
	public class Dialog_PawnTableTest : Window
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x000A0AE8 File Offset: 0x0009ECE8
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x000A0AFB File Offset: 0x0009ECFB
		private List<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer).ToList<Pawn>();
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000A0B16 File Offset: 0x0009ED16
		public Dialog_PawnTableTest(PawnColumnDef singleColumn)
		{
			this.singleColumn = singleColumn;
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x000A0B28 File Offset: 0x0009ED28
		public override void DoWindowContents(Rect inRect)
		{
			int num = ((int)inRect.height - 90) / 3;
			PawnTableDef pawnTableDef = new PawnTableDef();
			pawnTableDef.columns = new List<PawnColumnDef>
			{
				this.singleColumn
			};
			pawnTableDef.minWidth = 0;
			if (this.pawnTableMin == null)
			{
				this.pawnTableMin = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMin.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableOptimal == null)
			{
				this.pawnTableOptimal = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableOptimal.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableMax == null)
			{
				this.pawnTableMax = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMax.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), 0, num);
			}
			int num2 = 0;
			Text.Font = GameFont.Small;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Min size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMin.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Optimal size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableOptimal.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Max size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMax.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x000A0DE4 File Offset: 0x0009EFE4
		[DebugOutput("UI", false)]
		private static void PawnColumnTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnColumnDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate
				{
					Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x04000F41 RID: 3905
		private PawnColumnDef singleColumn;

		// Token: 0x04000F42 RID: 3906
		private PawnTable pawnTableMin;

		// Token: 0x04000F43 RID: 3907
		private PawnTable pawnTableOptimal;

		// Token: 0x04000F44 RID: 3908
		private PawnTable pawnTableMax;

		// Token: 0x04000F45 RID: 3909
		private const int TableTitleHeight = 30;
	}
}
