using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC6 RID: 3782
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06005C76 RID: 23670 RVA: 0x001FF1F7 File Offset: 0x001FD3F7
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06005C77 RID: 23671 RVA: 0x0001028D File Offset: 0x0000E48D
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x001FF208 File Offset: 0x001FD408
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x001FF217 File Offset: 0x001FD417
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x001FF239 File Offset: 0x001FD439
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x001FF246 File Offset: 0x001FD446
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04003269 RID: 12905
		private bool anyGameFiles;
	}
}
