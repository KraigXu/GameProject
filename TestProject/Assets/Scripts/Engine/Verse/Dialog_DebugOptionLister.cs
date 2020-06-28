using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035D RID: 861
	public abstract class Dialog_DebugOptionLister : Dialog_OptionLister
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x000A04B0 File Offset: 0x0009E6B0
		public Dialog_DebugOptionLister()
		{
			this.forcePause = true;
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000A04C0 File Offset: 0x0009E6C0
		protected bool DebugAction(string label, Action action)
		{
			bool result = false;
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label))
			{
				this.Close(true);
				action();
				result = true;
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
			return result;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000A0540 File Offset: 0x0009E740
		protected void DebugToolMap(string label, Action toolAction)
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000A05C8 File Offset: 0x0009E7C8
		protected void DebugToolMapForPawns(string label, Action<Pawn> pawnAction)
		{
			this.DebugToolMap(label, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn obj in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>().ToList<Pawn>())
					{
						pawnAction(obj);
					}
				}
			});
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000A05F8 File Offset: 0x0009E7F8
		protected void DebugToolWorld(string label, Action toolAction)
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return;
			}
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			if (this.listing.ButtonDebug(label))
			{
				this.Close(true);
				DebugTools.curTool = new DebugTool(label, toolAction, null);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000A0680 File Offset: 0x0009E880
		protected void CheckboxLabeledDebug(string label, ref bool checkOn)
		{
			if (!base.FilterAllows(label))
			{
				GUI.color = new Color(1f, 1f, 1f, 0.3f);
			}
			this.listing.LabelCheckboxDebug(label, ref checkOn);
			GUI.color = Color.white;
			if (Event.current.type == EventType.Layout)
			{
				this.totalOptionsHeight += 24f;
			}
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000A06EA File Offset: 0x0009E8EA
		protected void DoLabel(string label)
		{
			Text.Font = GameFont.Small;
			this.listing.Label(label, -1f, null);
			this.totalOptionsHeight += Text.CalcHeight(label, 300f) + 2f;
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000A0723 File Offset: 0x0009E923
		protected void DoGap()
		{
			this.listing.Gap(7f);
			this.totalOptionsHeight += 7f;
		}

		// Token: 0x04000F3B RID: 3899
		private const float DebugOptionsGap = 7f;
	}
}
