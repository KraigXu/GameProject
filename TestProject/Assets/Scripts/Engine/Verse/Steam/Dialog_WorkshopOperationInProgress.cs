using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x0200048A RID: 1162
	public class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002279 RID: 8825 RVA: 0x000A0E6A File Offset: 0x0009F06A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000D2170 File Offset: 0x000D0370
		public Dialog_WorkshopOperationInProgress()
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000D219C File Offset: 0x000D039C
		public override void DoWindowContents(Rect inRect)
		{
			EItemUpdateStatus eitemUpdateStatus;
			float num;
			Workshop.GetUpdateStatus(out eitemUpdateStatus, out num);
			WorkshopInteractStage curStage = Workshop.CurStage;
			if (curStage == WorkshopInteractStage.None && eitemUpdateStatus == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				this.Close(true);
				return;
			}
			string text = "";
			if (curStage != WorkshopInteractStage.None)
			{
				text += curStage.GetLabel();
				text += "\n\n";
			}
			if (eitemUpdateStatus != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				text += eitemUpdateStatus.GetLabel();
				if (num > 0f)
				{
					text = text + " (" + num.ToStringPercent() + ")";
				}
				text += GenText.MarchingEllipsis(0f);
			}
			Widgets.Label(inRect, text);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000D2230 File Offset: 0x000D0430
		public static void CloseAll()
		{
			Dialog_WorkshopOperationInProgress dialog_WorkshopOperationInProgress = Find.WindowStack.WindowOfType<Dialog_WorkshopOperationInProgress>();
			if (dialog_WorkshopOperationInProgress != null)
			{
				dialog_WorkshopOperationInProgress.Close(true);
			}
		}
	}
}
