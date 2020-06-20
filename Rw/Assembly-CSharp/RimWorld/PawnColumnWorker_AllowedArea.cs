using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EE3 RID: 3811
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x06005D60 RID: 23904 RVA: 0x00010306 File Offset: 0x0000E506
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x00204AD0 File Offset: 0x00202CD0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x00204AE3 File Offset: 0x00202CE3
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x00204AFD File Offset: 0x00202CFD
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06005D64 RID: 23908 RVA: 0x00204B0D File Offset: 0x00202D0D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
		}

		// Token: 0x06005D65 RID: 23909 RVA: 0x00204B24 File Offset: 0x00202D24
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageAreas".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x00204BA0 File Offset: 0x00202DA0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x00204BC4 File Offset: 0x00202DC4
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return int.MinValue;
			}
			Area areaRestriction = pawn.playerSettings.AreaRestriction;
			if (areaRestriction == null)
			{
				return -2147483647;
			}
			return areaRestriction.ID;
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x00204C00 File Offset: 0x00202E00
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction != Faction.OfPlayer)
					{
						return;
					}
					if (Event.current.button == 0)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = Find.CurrentMap.areaManager.Home;
					}
					else if (Event.current.button == 1)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = null;
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x00204CD7 File Offset: 0x00202ED7
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}

		// Token: 0x040032C4 RID: 12996
		private const int TopAreaHeight = 65;

		// Token: 0x040032C5 RID: 12997
		private const int ManageAreasButtonHeight = 32;
	}
}
