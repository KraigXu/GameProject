using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EAF RID: 3759
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x06005BC9 RID: 23497 RVA: 0x001FB389 File Offset: 0x001F9589
		public static IEnumerable<ITab_Pawn_Log_Utility.LogLineDisplayable> GenerateLogLinesFor(Pawn pawn, bool showAll, bool showCombat, bool showSocial)
		{
			LogEntry[] nonCombatLines = showSocial ? (from e in Find.PlayLog.AllEntries
			where e.Concerns(pawn)
			select e).ToArray<LogEntry>() : new LogEntry[0];
			int nonCombatIndex = 0;
			Battle currentBattle = null;
			if (showCombat)
			{
				bool atTop = true;
				foreach (Battle battle in Find.BattleLog.Battles)
				{
					if (battle.Concerns(pawn))
					{
						foreach (LogEntry entry in battle.Entries)
						{
							if (entry.Concerns(pawn))
							{
								if (showAll || entry.ShowInCompactView())
								{
									while (nonCombatIndex < nonCombatLines.Length && nonCombatLines[nonCombatIndex].Age < entry.Age)
									{
										if (currentBattle != null && currentBattle != battle)
										{
											yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
											currentBattle = null;
										}
										LogEntry[] array = nonCombatLines;
										int num = nonCombatIndex;
										nonCombatIndex = num + 1;
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num], pawn);
										atTop = false;
									}
									if (currentBattle != battle)
									{
										if (!atTop)
										{
											yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
										}
										yield return new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle.GetName());
										currentBattle = battle;
										atTop = false;
									}
									yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(entry, pawn);
									entry = null;
								}
							}
						}
						List<LogEntry>.Enumerator enumerator2 = default(List<LogEntry>.Enumerator);
						battle = null;
					}
				}
				List<Battle>.Enumerator enumerator = default(List<Battle>.Enumerator);
			}
			while (nonCombatIndex < nonCombatLines.Length)
			{
				if (currentBattle != null)
				{
					yield return new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding);
					currentBattle = null;
				}
				LogEntry[] array2 = nonCombatLines;
				int num = nonCombatIndex;
				nonCombatIndex = num + 1;
				yield return new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array2[num], pawn);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003229 RID: 12841
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x0400322A RID: 12842
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x0400322B RID: 12843
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x0400322C RID: 12844
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x02001DD2 RID: 7634
		public class LogDrawData
		{
			// Token: 0x0600A6C9 RID: 42697 RVA: 0x003146F0 File Offset: 0x003128F0
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x04007066 RID: 28774
			public bool alternatingBackground;

			// Token: 0x04007067 RID: 28775
			public LogEntry highlightEntry;

			// Token: 0x04007068 RID: 28776
			public float highlightIntensity;
		}

		// Token: 0x02001DD3 RID: 7635
		public abstract class LogLineDisplayable
		{
			// Token: 0x0600A6CB RID: 42699 RVA: 0x003146F9 File Offset: 0x003128F9
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x0600A6CC RID: 42700
			public abstract float GetHeight_Worker(float width);

			// Token: 0x0600A6CD RID: 42701
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x0600A6CE RID: 42702
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x0600A6CF RID: 42703 RVA: 0x00010306 File Offset: 0x0000E506
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x04007069 RID: 28777
			private float cachedHeight = -1f;
		}

		// Token: 0x02001DD4 RID: 7636
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600A6D1 RID: 42705 RVA: 0x0031472E File Offset: 0x0031292E
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			// Token: 0x0600A6D2 RID: 42706 RVA: 0x00314740 File Offset: 0x00312940
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x0600A6D3 RID: 42707 RVA: 0x00314770 File Offset: 0x00312970
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x0600A6D4 RID: 42708 RVA: 0x0031479C File Offset: 0x0031299C
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x0400706A RID: 28778
			private TaggedString text;
		}

		// Token: 0x02001DD5 RID: 7637
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600A6D5 RID: 42709 RVA: 0x003147BA File Offset: 0x003129BA
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x0600A6D6 RID: 42710 RVA: 0x003147D0 File Offset: 0x003129D0
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

			// Token: 0x0600A6D7 RID: 42711 RVA: 0x00314804 File Offset: 0x00312A04
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				float height = base.GetHeight(width);
				float width2 = width - 29f;
				Rect rect = new Rect(0f, position, width, height);
				if (this.log == data.highlightEntry)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.HighlightAlpha * data.highlightIntensity), null);
					data.highlightIntensity = Mathf.Max(0f, data.highlightIntensity - Time.deltaTime / ITab_Pawn_Log_Utility.HighlightDuration);
				}
				else if (data.alternatingBackground)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.AlternateAlpha), null);
				}
				data.alternatingBackground = !data.alternatingBackground;
				TaggedString label = this.log.ToGameStringFromPOV(this.pawn, false);
				Widgets.Label(new Rect(29f, position, width2, height), label);
				Texture2D texture2D = this.log.IconFromPOV(this.pawn);
				if (texture2D != null)
				{
					GUI.DrawTexture(new Rect(0f, position + (height - 26f) / 2f, 26f, 26f), texture2D);
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.log.GetTipString(), 613261 + this.log.LogID * 2063);
					Widgets.DrawHighlight(rect);
				}
				if (Widgets.ButtonInvisible(rect, this.log.CanBeClickedFromPOV(this.pawn)))
				{
					this.log.ClickedFromPOV(this.pawn);
				}
				if (DebugViewSettings.logCombatLogMouseover && Mouse.IsOver(rect))
				{
					this.log.ToGameStringFromPOV(this.pawn, true);
				}
			}

			// Token: 0x0600A6D8 RID: 42712 RVA: 0x003149B5 File Offset: 0x00312BB5
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x0600A6D9 RID: 42713 RVA: 0x003149D0 File Offset: 0x00312BD0
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x0400706B RID: 28779
			private LogEntry log;

			// Token: 0x0400706C RID: 28780
			private Pawn pawn;
		}

		// Token: 0x02001DD6 RID: 7638
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x0600A6DB RID: 42715 RVA: 0x003149E8 File Offset: 0x00312BE8
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x0600A6DC RID: 42716 RVA: 0x003149F7 File Offset: 0x00312BF7
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x0600A6DD RID: 42717 RVA: 0x00002681 File Offset: 0x00000881
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x0600A6DE RID: 42718 RVA: 0x003149FF File Offset: 0x00312BFF
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x0400706D RID: 28781
			private float height;
		}
	}
}
