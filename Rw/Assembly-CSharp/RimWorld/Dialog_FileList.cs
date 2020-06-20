using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E66 RID: 3686
	public abstract class Dialog_FileList : Window
	{
		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06005954 RID: 22868 RVA: 0x001DE1AF File Offset: 0x001DC3AF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 700f);
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06005955 RID: 22869 RVA: 0x00010306 File Offset: 0x0000E506
		protected virtual bool ShouldDoTypeInField
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x001DE1C0 File Offset: 0x001DC3C0
		public Dialog_FileList()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.ReloadFiles();
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x001DE228 File Offset: 0x001DC428
		public override void DoWindowContents(Rect inRect)
		{
			Vector2 vector = new Vector2(inRect.width - 16f, 40f);
			inRect.height -= 45f;
			float y = vector.y;
			float height = (float)this.files.Count * y;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			Rect outRect = new Rect(inRect.AtZero());
			outRect.height -= this.bottomAreaHeight;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			int num2 = 0;
			foreach (SaveFileInfo sfi in this.files)
			{
				if (num + vector.y >= this.scrollPosition.y && num <= this.scrollPosition.y + outRect.height)
				{
					Rect rect = new Rect(0f, num, vector.x, vector.y);
					if (num2 % 2 == 0)
					{
						Widgets.DrawAltRect(rect);
					}
					GUI.BeginGroup(rect);
					Rect rect2 = new Rect(rect.width - 36f, (rect.height - 36f) / 2f, 36f, 36f);
					if (Widgets.ButtonImage(rect2, TexButton.DeleteX, Color.white, GenUI.SubtleMouseoverColor, true))
					{
						FileInfo localFile = sfi.FileInfo;
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(localFile.Name), delegate
						{
							localFile.Delete();
							this.ReloadFiles();
						}, true, null));
					}
					TooltipHandler.TipRegionByKey(rect2, "DeleteThisSavegame");
					Text.Font = GameFont.Small;
					Rect rect3 = new Rect(rect2.x - 100f, (rect.height - 36f) / 2f, 100f, 36f);
					if (Widgets.ButtonText(rect3, this.interactButLabel, true, true, true))
					{
						this.DoFileInteraction(Path.GetFileNameWithoutExtension(sfi.FileInfo.Name));
					}
					Rect rect4 = new Rect(rect3.x - 94f, 0f, 94f, rect.height);
					Dialog_FileList.DrawDateAndVersion(sfi, rect4);
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = this.FileNameColor(sfi);
					Rect rect5 = new Rect(8f, 0f, rect4.x - 8f - 4f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sfi.FileInfo.Name);
					Widgets.Label(rect5, fileNameWithoutExtension.Truncate(rect5.width * 1.8f, null));
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.EndGroup();
				}
				num += vector.y;
				num2++;
			}
			Widgets.EndScrollView();
			if (this.ShouldDoTypeInField)
			{
				this.DoTypeInField(inRect.AtZero());
			}
		}

		// Token: 0x06005958 RID: 22872
		protected abstract void DoFileInteraction(string fileName);

		// Token: 0x06005959 RID: 22873
		protected abstract void ReloadFiles();

		// Token: 0x0600595A RID: 22874 RVA: 0x001DE580 File Offset: 0x001DC780
		protected virtual void DoTypeInField(Rect rect)
		{
			GUI.BeginGroup(rect);
			bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
			float y = rect.height - 52f;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.SetNextControlName("MapNameField");
			string str = Widgets.TextField(new Rect(5f, y, 400f, 35f), this.typingName);
			if (GenText.IsValidFilename(str))
			{
				this.typingName = str;
			}
			if (!this.focusedNameArea)
			{
				UI.FocusControl("MapNameField", this);
				this.focusedNameArea = true;
			}
			if (Widgets.ButtonText(new Rect(420f, y, rect.width - 400f - 20f, 35f), "SaveGameButton".Translate(), true, true, true) || flag)
			{
				if (this.typingName.NullOrEmpty())
				{
					Messages.Message("NeedAName".Translate(), MessageTypeDefOf.RejectInput, false);
				}
				else
				{
					this.DoFileInteraction(this.typingName);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x001DE69E File Offset: 0x001DC89E
		protected virtual Color FileNameColor(SaveFileInfo sfi)
		{
			return Dialog_FileList.DefaultFileTextColor;
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x001DE6A8 File Offset: 0x001DC8A8
		public static void DrawDateAndVersion(SaveFileInfo sfi, Rect rect)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 2f, rect.width, rect.height / 2f);
			GUI.color = SaveFileInfo.UnimportantTextColor;
			Widgets.Label(rect2, sfi.FileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm"));
			Rect rect3 = new Rect(0f, rect2.yMax, rect.width, rect.height / 2f);
			GUI.color = sfi.VersionColor;
			Widgets.Label(rect3, sfi.GameVersion);
			if (Mouse.IsOver(rect3))
			{
				TooltipHandler.TipRegion(rect3, sfi.CompatibilityTip);
			}
			GUI.EndGroup();
		}

		// Token: 0x04003058 RID: 12376
		protected string interactButLabel = "Error";

		// Token: 0x04003059 RID: 12377
		protected float bottomAreaHeight;

		// Token: 0x0400305A RID: 12378
		protected List<SaveFileInfo> files = new List<SaveFileInfo>();

		// Token: 0x0400305B RID: 12379
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400305C RID: 12380
		protected string typingName = "";

		// Token: 0x0400305D RID: 12381
		private bool focusedNameArea;

		// Token: 0x0400305E RID: 12382
		protected const float EntryHeight = 40f;

		// Token: 0x0400305F RID: 12383
		protected const float FileNameLeftMargin = 8f;

		// Token: 0x04003060 RID: 12384
		protected const float FileNameRightMargin = 4f;

		// Token: 0x04003061 RID: 12385
		protected const float FileInfoWidth = 94f;

		// Token: 0x04003062 RID: 12386
		protected const float InteractButWidth = 100f;

		// Token: 0x04003063 RID: 12387
		protected const float InteractButHeight = 36f;

		// Token: 0x04003064 RID: 12388
		protected const float DeleteButSize = 36f;

		// Token: 0x04003065 RID: 12389
		private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

		// Token: 0x04003066 RID: 12390
		protected const float NameTextFieldWidth = 400f;

		// Token: 0x04003067 RID: 12391
		protected const float NameTextFieldHeight = 35f;

		// Token: 0x04003068 RID: 12392
		protected const float NameTextFieldButtonSpace = 20f;
	}
}
