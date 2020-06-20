using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000469 RID: 1129
	[StaticConstructorOnStartup]
	public static class CustomCursor
	{
		// Token: 0x0600215D RID: 8541 RVA: 0x000CC7B4 File Offset: 0x000CA9B4
		public static void Activate()
		{
			Cursor.SetCursor(CustomCursor.CursorTex, CustomCursor.CursorHotspot, CursorMode.Auto);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000CC7C6 File Offset: 0x000CA9C6
		public static void Deactivate()
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x04001468 RID: 5224
		private static readonly Texture2D CursorTex = ContentFinder<Texture2D>.Get("UI/Cursors/CursorCustom", true);

		// Token: 0x04001469 RID: 5225
		private static Vector2 CursorHotspot = new Vector2(3f, 3f);
	}
}
