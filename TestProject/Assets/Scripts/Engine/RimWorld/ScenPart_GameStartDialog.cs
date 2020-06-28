using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1B RID: 3099
	public class ScenPart_GameStartDialog : ScenPart
	{
		// Token: 0x060049E1 RID: 18913 RVA: 0x001901C4 File Offset: 0x0018E3C4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 5f);
			this.text = Widgets.TextArea(scenPartRect, this.text, false);
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x001901F7 File Offset: 0x0018E3F7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.textKey, "textKey", null, false);
			Scribe_Defs.Look<SoundDef>(ref this.closeSound, "closeSound");
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x00190234 File Offset: 0x0018E434
		public override void PostGameStart()
		{
			if (Find.GameInitData.startedFromEntry)
			{
				Find.MusicManagerPlay.disabled = true;
				Find.WindowStack.Notify_GameStartDialogOpened();
				DiaNode diaNode = new DiaNode(this.text.NullOrEmpty() ? this.textKey.TranslateSimple() : this.text);
				DiaOption diaOption = new DiaOption();
				diaOption.resolveTree = true;
				diaOption.clickSound = null;
				diaNode.options.Add(diaOption);
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.soundClose = ((this.closeSound != null) ? this.closeSound : SoundDefOf.GameStartSting);
				dialog_NodeTree.closeAction = delegate
				{
					Find.MusicManagerPlay.ForceSilenceFor(7f);
					Find.MusicManagerPlay.disabled = false;
					Find.WindowStack.Notify_GameStartDialogClosed();
					Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
					TutorSystem.Notify_Event("GameStartDialogClosed");
				};
				Find.WindowStack.Add(dialog_NodeTree);
				Find.Archive.Add(new ArchivedDialog(diaNode.text, null, null));
			}
		}

		// Token: 0x04002A04 RID: 10756
		private string text;

		// Token: 0x04002A05 RID: 10757
		private string textKey;

		// Token: 0x04002A06 RID: 10758
		private SoundDef closeSound;
	}
}
