using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C8 RID: 968
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06001C8F RID: 7311 RVA: 0x000ADADD File Offset: 0x000ABCDD
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000ADAFA File Offset: 0x000ABCFA
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000ADB0B File Offset: 0x000ABD0B
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000ADB1C File Offset: 0x000ABD1C
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Widgets.Label(new Rect(rect.xMax - num, pos.y, num, num2), this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}

		// Token: 0x040010D7 RID: 4311
		public Texture2D image;

		// Token: 0x040010D8 RID: 4312
		public string url;

		// Token: 0x040010D9 RID: 4313
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}
