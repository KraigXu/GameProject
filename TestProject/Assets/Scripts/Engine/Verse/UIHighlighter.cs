using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003BE RID: 958
	[StaticConstructorOnStartup]
	public static class UIHighlighter
	{
		// Token: 0x06001C3D RID: 7229 RVA: 0x000ABBB0 File Offset: 0x000A9DB0
		public static void HighlightTag(string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (tag.NullOrEmpty())
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				if (UIHighlighter.liveTags[i].First == tag && UIHighlighter.liveTags[i].Second == Time.frameCount)
				{
					return;
				}
			}
			UIHighlighter.liveTags.Add(new Pair<string, int>(tag, Time.frameCount));
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000ABC34 File Offset: 0x000A9E34
		public static void HighlightOpportunity(Rect rect, string tag)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			for (int i = 0; i < UIHighlighter.liveTags.Count; i++)
			{
				Pair<string, int> pair = UIHighlighter.liveTags[i];
				if (tag == pair.First && Time.frameCount == pair.Second + 1)
				{
					Rect rect2 = rect.ContractedBy(-10f);
					GUI.color = new Color(1f, 1f, 1f, Pulser.PulseBrightness(1.2f, 0.7f));
					Widgets.DrawAtlas(rect2, UIHighlighter.TutorHighlightAtlas);
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000ABCD9 File Offset: 0x000A9ED9
		public static void UIHighlighterUpdate()
		{
			UIHighlighter.liveTags.RemoveAll((Pair<string, int> pair) => Time.frameCount > pair.Second + 1);
		}

		// Token: 0x040010A6 RID: 4262
		private static List<Pair<string, int>> liveTags = new List<Pair<string, int>>();

		// Token: 0x040010A7 RID: 4263
		private const float PulseFrequency = 1.2f;

		// Token: 0x040010A8 RID: 4264
		private const float PulseAmplitude = 0.7f;

		// Token: 0x040010A9 RID: 4265
		private static readonly Texture2D TutorHighlightAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/TutorHighlightAtlas", true);
	}
}
