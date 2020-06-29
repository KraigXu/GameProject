using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class MainTabWindow_Menu : MainTabWindow
	{
		
		// (get) Token: 0x06005C76 RID: 23670 RVA: 0x001FF1F7 File Offset: 0x001FD3F7
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		
		// (get) Token: 0x06005C77 RID: 23671 RVA: 0x0001028D File Offset: 0x0000E48D
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
		}

		
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		
		private bool anyGameFiles;
	}
}
