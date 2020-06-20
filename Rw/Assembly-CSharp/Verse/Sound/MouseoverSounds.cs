using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020004F8 RID: 1272
	public static class MouseoverSounds
	{
		// Token: 0x06002494 RID: 9364 RVA: 0x000D97D0 File Offset: 0x000D79D0
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000D97DE File Offset: 0x000D79DE
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000D97EC File Offset: 0x000D79EC
		public static void DoRegion(Rect rect, SoundDef sound)
		{
			if (sound == null)
			{
				return;
			}
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect2 = new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size);
			MouseoverSounds.MouseoverRegionCall item = default(MouseoverSounds.MouseoverRegionCall);
			item.rect = rect2;
			item.sound = sound;
			item.mouseIsOver = Mouse.IsOver(rect);
			MouseoverSounds.frameCalls.Add(item);
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000D9858 File Offset: 0x000D7A58
		public static void ResolveFrame()
		{
			for (int i = 0; i < MouseoverSounds.frameCalls.Count; i++)
			{
				if (MouseoverSounds.frameCalls[i].mouseIsOver)
				{
					if (MouseoverSounds.lastUsedCallInd != i && !MouseoverSounds.frameCalls[i].Matches(MouseoverSounds.lastUsedCall) && MouseoverSounds.forceSilenceUntilFrame < Time.frameCount)
					{
						MouseoverSounds.frameCalls[i].sound.PlayOneShotOnCamera(null);
					}
					MouseoverSounds.lastUsedCallInd = i;
					MouseoverSounds.lastUsedCall = MouseoverSounds.frameCalls[i];
					MouseoverSounds.frameCalls.Clear();
					return;
				}
			}
			MouseoverSounds.lastUsedCall = MouseoverSounds.MouseoverRegionCall.Invalid;
			MouseoverSounds.lastUsedCallInd = -1;
			MouseoverSounds.frameCalls.Clear();
		}

		// Token: 0x0400163F RID: 5695
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04001640 RID: 5696
		private static int lastUsedCallInd = -1;

		// Token: 0x04001641 RID: 5697
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04001642 RID: 5698
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x020016D9 RID: 5849
		private struct MouseoverRegionCall
		{
			// Token: 0x17001530 RID: 5424
			// (get) Token: 0x06008630 RID: 34352 RVA: 0x002B59B7 File Offset: 0x002B3BB7
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17001531 RID: 5425
			// (get) Token: 0x06008631 RID: 34353 RVA: 0x002B59D0 File Offset: 0x002B3BD0
			public static MouseoverSounds.MouseoverRegionCall Invalid
			{
				get
				{
					return new MouseoverSounds.MouseoverRegionCall
					{
						rect = new Rect(-1000f, -1000f, 0f, 0f)
					};
				}
			}

			// Token: 0x06008632 RID: 34354 RVA: 0x002B5A06 File Offset: 0x002B3C06
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06008633 RID: 34355 RVA: 0x002B5A1C File Offset: 0x002B3C1C
			public override string ToString()
			{
				if (!this.IsValid)
				{
					return "(Invalid)";
				}
				return string.Concat(new object[]
				{
					"(rect=",
					this.rect,
					this.mouseIsOver ? "mouseIsOver" : "",
					")"
				});
			}

			// Token: 0x040057C4 RID: 22468
			public bool mouseIsOver;

			// Token: 0x040057C5 RID: 22469
			public Rect rect;

			// Token: 0x040057C6 RID: 22470
			public SoundDef sound;
		}
	}
}
