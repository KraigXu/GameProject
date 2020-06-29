using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class ITab_Pawn_Log_Utility
	{
		
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
									
								}
							}
						}
						List<LogEntry>.Enumerator enumerator2 = default(List<LogEntry>.Enumerator);
						
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

		
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		
		public class LogDrawData
		{
			
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			
			public bool alternatingBackground;

			
			public LogEntry highlightEntry;

			
			public float highlightIntensity;
		}

		
		public abstract class LogLineDisplayable
		{
			
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			
			public abstract float GetHeight_Worker(float width);

			
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			
			public abstract void AppendTo(StringBuilder sb);

			
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			
			private float cachedHeight = -1f;
		}

		
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			
			private TaggedString text;
		}

		
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2));
			}

			
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

			
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			
			private LogEntry log;

			
			private Pawn pawn;
		}

		
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			
			private float height;
		}
	}
}
