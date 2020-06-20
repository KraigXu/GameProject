using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E10 RID: 3600
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060056FE RID: 22270 RVA: 0x001CE6A7 File Offset: 0x001CC8A7
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x001CE6B0 File Offset: 0x001CC8B0
		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
		{
			float num = this.ColonistBar.GetEntryRectAlpha(rect);
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref num);
			if (reordering)
			{
				num *= 0.5f;
			}
			Color color = new Color(1f, 1f, 1f, num);
			GUI.color = color;
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num2 = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num2;
				position.height = num2;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			if (highlight)
			{
				int thickness = (rect.width <= 22f) ? 2 : 3;
				GUI.color = Color.white;
				Widgets.DrawBox(rect, thickness);
				GUI.color = color;
			}
			Rect rect2 = rect.ContractedBy(-2f * this.ColonistBar.Scale);
			if ((colonist.Dead ? Find.Selector.SelectedObjects.Contains(colonist.Corpse) : Find.Selector.SelectedObjects.Contains(colonist)) && !WorldRendererUtility.WorldRenderedNow)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldRenderedNow && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.position), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true));
			GUI.color = new Color(1f, 1f, 1f, num * 0.8f);
			this.DrawIcons(rect, colonist);
			GUI.color = color;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num3 = 4f * this.ColonistBar.Scale;
			Vector2 pos = new Vector2(rect.center.x, rect.yMax - num3);
			GenMapUI.DrawPawnLabel(colonist, pos, num, rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2f, this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x001CE8F4 File Offset: 0x001CCAF4
		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].group == group)
				{
					num = Mathf.Min(num, drawLocs[i].x);
					num2 = Mathf.Max(num2, drawLocs[i].x + this.ColonistBar.Size.x);
					num3 = Mathf.Max(num3, drawLocs[i].y + this.ColonistBar.Size.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3 - 0f).ContractedBy(-12f * this.ColonistBar.Scale);
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x001CE9E0 File Offset: 0x001CCBE0
		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			Map map = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group).map;
			float num;
			if (map == null)
			{
				if (WorldRendererUtility.WorldRenderedNow)
				{
					num = 1f;
				}
				else
				{
					num = 0.75f;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				num = 0.75f;
			}
			else
			{
				num = 1f;
			}
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, 0.4f * num), null);
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x001CEA80 File Offset: 0x001CCC80
		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					alpha = Mathf.Min(alpha, 0.4f);
					return;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x001CEAB8 File Offset: 0x001CCCB8
		public void HandleClicks(Rect rect, Pawn colonist, int reorderableGroup, out bool reordering)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump(colonist);
			}
			reordering = ReorderableWidget.Reorderable(reorderableGroup, rect, true);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001CEB40 File Offset: 0x001CCD40
		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				if ((!worldRenderedNow && !Find.Selector.dragBox.IsValidAndActive) || (worldRenderedNow && !Find.WorldSelector.dragBox.IsValidAndActive))
				{
					Find.Selector.dragBox.active = false;
					Find.WorldSelector.dragBox.active = false;
					ColonistBar.Entry entry = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group);
					Map map = entry.map;
					if (map == null)
					{
						if (WorldRendererUtility.WorldRenderedNow)
						{
							CameraJumper.TrySelect(entry.pawn);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(entry.pawn);
						}
					}
					else
					{
						if (!CameraJumper.TryHideWorld() && Find.CurrentMap != map)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
						Current.Game.CurrentMap = map;
					}
				}
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001CECA0 File Offset: 0x001CCEA0
		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001CECB0 File Offset: 0x001CCEB0
		public Rect GetPawnTextureRect(Vector2 pos)
		{
			float x = pos.x;
			float y = pos.y;
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			return new Rect(x + 1f, y - (vector.y - this.ColonistBar.Size.y) - 1f, vector.x, vector.y).ContractedBy(1f);
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x001CED24 File Offset: 0x001CCF24
		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (colonist.Dead)
			{
				return;
			}
			float num = 20f * this.ColonistBar.Scale;
			Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
			bool flag = false;
			if (colonist.CurJob != null)
			{
				JobDef def = colonist.CurJob.def;
				if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
				{
					flag = true;
				}
				else if (def == JobDefOf.Wait_Combat)
				{
					Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
					if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
					{
						flag = true;
					}
				}
			}
			if (colonist.IsFormingCaravan())
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_FormingCaravan, ref vector, "ActivityIconFormingCaravan".Translate());
			}
			if (colonist.InAggroMentalState)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_MentalStateAggro, ref vector, colonist.MentalStateDef.LabelCap);
			}
			else if (colonist.InMentalState)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_MentalStateNonAggro, ref vector, colonist.MentalStateDef.LabelCap);
			}
			else if (colonist.InBed() && colonist.CurrentBed().Medical)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_MedicalRest, ref vector, "ActivityIconMedicalRest".Translate());
			}
			else if (colonist.CurJob != null && colonist.jobs.curDriver.asleep)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Sleeping, ref vector, "ActivityIconSleeping".Translate());
			}
			else if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Fleeing, ref vector, "ActivityIconFleeing".Translate());
			}
			else if (flag)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Attacking, ref vector, "ActivityIconAttacking".Translate());
			}
			else if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Idle, ref vector, "ActivityIconIdle".Translate());
			}
			if (colonist.IsBurning() && vector.x + num <= rect.xMax)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Burning, ref vector, "ActivityIconBurning".Translate());
			}
			if (colonist.Inspired && vector.x + num <= rect.xMax)
			{
				this.DrawIcon(ColonistBarColonistDrawer.Icon_Inspired, ref vector, colonist.InspirationDef.LabelCap);
			}
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x001CEFA8 File Offset: 0x001CD1A8
		private void DrawIcon(Texture2D icon, ref Vector2 pos, string tooltip)
		{
			float num = 20f * this.ColonistBar.Scale;
			Rect rect = new Rect(pos.x, pos.y, num, num);
			GUI.DrawTexture(rect, icon);
			TooltipHandler.TipRegion(rect, tooltip);
			pos.x += num;
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x001CEFF8 File Offset: 0x001CD1F8
		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing obj = colonist;
			if (colonist.Dead)
			{
				obj = colonist.Corpse;
			}
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<object>(ColonistBarColonistDrawer.bracketLocs, obj, rect, SelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x001CF078 File Offset: 0x001CD278
		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(ColonistBarColonistDrawer.bracketLocs, caravan, rect, WorldSelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x001CF0E8 File Offset: 0x001CD2E8
		private void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale);
				num += 90;
			}
		}

		// Token: 0x04002F72 RID: 12146
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		// Token: 0x04002F73 RID: 12147
		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		// Token: 0x04002F74 RID: 12148
		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		// Token: 0x04002F75 RID: 12149
		private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan", true);

		// Token: 0x04002F76 RID: 12150
		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x04002F77 RID: 12151
		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		// Token: 0x04002F78 RID: 12152
		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		// Token: 0x04002F79 RID: 12153
		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x04002F7A RID: 12154
		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		// Token: 0x04002F7B RID: 12155
		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		// Token: 0x04002F7C RID: 12156
		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		// Token: 0x04002F7D RID: 12157
		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		// Token: 0x04002F7E RID: 12158
		private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired", true);

		// Token: 0x04002F7F RID: 12159
		public static readonly Vector2 PawnTextureSize = new Vector2(ColonistBar.BaseSize.x - 2f, 75f);

		// Token: 0x04002F80 RID: 12160
		public static readonly Vector3 PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);

		// Token: 0x04002F81 RID: 12161
		public const float PawnTextureCameraZoom = 1.28205f;

		// Token: 0x04002F82 RID: 12162
		private const float PawnTextureHorizontalPadding = 1f;

		// Token: 0x04002F83 RID: 12163
		private const float BaseIconSize = 20f;

		// Token: 0x04002F84 RID: 12164
		private const float BaseGroupFrameMargin = 12f;

		// Token: 0x04002F85 RID: 12165
		public const float DoubleClickTime = 0.5f;

		// Token: 0x04002F86 RID: 12166
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
