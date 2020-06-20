using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000376 RID: 886
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x000864F0 File Offset: 0x000846F0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x000A19A7 File Offset: 0x0009FBA7
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000A19DC File Offset: 0x0009FBDC
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.Layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x04000F55 RID: 3925
		public Def def;

		// Token: 0x04000F56 RID: 3926
		private float viewHeight;

		// Token: 0x04000F57 RID: 3927
		private Vector2 scrollPosition;

		// Token: 0x04000F58 RID: 3928
		private float labelColumnWidth = 140f;

		// Token: 0x04000F59 RID: 3929
		private const float TopAreaHeight = 16f;

		// Token: 0x04000F5A RID: 3930
		private const float ExtraScrollHeight = 200f;
	}
}
