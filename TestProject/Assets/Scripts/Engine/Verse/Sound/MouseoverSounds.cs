using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	
	public static class MouseoverSounds
	{
		
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		
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

		
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		
		private static int lastUsedCallInd = -1;

		
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		
		private static int forceSilenceUntilFrame = -1;

		
		private struct MouseoverRegionCall
		{
			
			// (get) Token: 0x06008630 RID: 34352 RVA: 0x002B59B7 File Offset: 0x002B3BB7
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			
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

			
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			
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

			
			public bool mouseIsOver;

			
			public Rect rect;

			
			public SoundDef sound;
		}
	}
}
