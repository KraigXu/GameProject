using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C3 RID: 963
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06001C53 RID: 7251 RVA: 0x000AC20C File Offset: 0x000AA40C
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000AC214 File Offset: 0x000AA414
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000AC21C File Offset: 0x000AA41C
		public static float BackgroundDarkAlphaForText()
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
			float num2 = (Find.CurrentMap.Biome == BiomeDefOf.IceSheet) ? 1f : Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f);
			return num * num2 * 0.41f;
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000AC27C File Offset: 0x000AA47C
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000AC2C8 File Offset: 0x000AA4C8
		public static void DrawTextureWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords = default(Rect))
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, 0.5f), material);
					return;
				}
			}
			else
			{
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, 0.5f), material);
				}
			}
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000AC3D0 File Offset: 0x000AA5D0
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				IntVec2 intVec = (!tDef.defaultPlacingRot.IsHorizontal) ? tDef.Size : tDef.Size.Rotated();
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)intVec.x, tDef.graphicData.drawSize.y / (float)intVec.z);
			}
			return num;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000AC458 File Offset: 0x000AA658
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000AC48C File Offset: 0x000AA68C
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000AC4D8 File Offset: 0x000AA6D8
		public static float GetWidthCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			s = s.StripTags();
			float x;
			if (GenUI.labelWidthCache.TryGetValue(s, out x))
			{
				return x;
			}
			x = Text.CalcSize(s).x;
			GenUI.labelWidthCache.Add(s, x);
			return x;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000AC54D File Offset: 0x000AA74D
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000AC559 File Offset: 0x000AA759
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000AC584 File Offset: 0x000AA784
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000AC59C File Offset: 0x000AA79C
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000AC620 File Offset: 0x000AA820
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null, bool drawTextBackground = false, Color textBgColor = default(Color))
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (drawTextBackground && text != "")
			{
				Rect value;
				if (customRect != null)
				{
					value = customRect.Value;
				}
				else
				{
					Vector2 vector = Text.CalcSize(text);
					float num2 = (iconTex != null) ? 42f : 0f;
					value = new Rect(mousePosition.x + 12f - 4f, num + num2, Text.CalcSize(text).x + 8f, vector.y);
				}
				Widgets.DrawBoxSolid(value, textBgColor);
			}
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					Widgets.DrawTextureRotated(rect, iconTex, angle);
				}, false, false, 0f);
				num += mouseRect.height + 10f;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					Widgets.Label(textRect.AtZero(), text);
					Text.Font = font;
				}, false, false, 0f);
			}
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000AC810 File Offset: 0x000AAA10
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000AC88C File Offset: 0x000AAA8C
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000AC8C0 File Offset: 0x000AAAC0
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			GUI.BeginGroup(rect);
			Widgets.Label(new Rect(0f, 2f, rect.width, 25f), status.LabelCap);
			Rect rect2 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect2, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect2, status.GUIChangeArrow);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, status.GetTipString());
			}
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000AC965 File Offset: 0x000AAB65
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAtMouse_NewTemp(clickParams, thingsOnly, null);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000AC96F File Offset: 0x000AAB6F
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse_NewTemp(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt_NewTemp(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000AC97E File Offset: 0x000AAB7E
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAt_NewTemp(clickPos, clickParams, thingsOnly, null);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000AC989 File Offset: 0x000AAB89
		public static IEnumerable<LocalTargetInfo> TargetsAt_NewTemp(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams);
			Thing caster = (source != null) ? source.Caster : null;
			int num;
			for (int i = 0; i < clickableList.Count; i = num + 1)
			{
				Pawn pawn = clickableList[i] as Pawn;
				if (pawn == null || !pawn.IsInvisible() || (caster != null && caster.Faction == pawn.Faction))
				{
					yield return clickableList[i];
				}
				num = i;
			}
			if (!thingsOnly)
			{
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(intVec, Find.CurrentMap, false)))
				{
					yield return intVec;
				}
			}
			yield break;
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000AC9B0 File Offset: 0x000AABB0
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn))
				{
					GenUI.clickedPawns.Add(pawn);
				}
			}
			GenUI.clickedPawns.Sort(new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int j = 0; j < GenUI.clickedPawns.Count; j++)
			{
				list.Add(GenUI.clickedPawns[j]);
			}
			List<Thing> list2 = new List<Thing>();
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				if (!list.Contains(thing) && clickParams.CanTarget(thing))
				{
					list2.Add(thing);
				}
			}
			List<Thing> list3 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list3.Count; k++)
			{
				Thing thing2 = list3[k];
				if (thing2.CustomRectForSelector != null && thing2.CustomRectForSelector.Value.Contains(c) && !list.Contains(thing2) && clickParams.CanTarget(thing2))
				{
					list2.Add(thing2);
				}
			}
			list2.Sort(new Comparison<Thing>(GenUI.CompareThingsByDrawAltitude));
			list.AddRange(list2);
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int l = 0; l < allPawnsSpawned2.Count; l++)
			{
				Pawn pawn2 = allPawnsSpawned2[l];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2))
				{
					GenUI.clickedPawns.Add(pawn2);
				}
			}
			GenUI.clickedPawns.Sort(new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int m = 0; m < GenUI.clickedPawns.Count; m++)
			{
				if (!list.Contains(GenUI.clickedPawns[m]))
				{
					list.Add(GenUI.clickedPawns[m]);
				}
			}
			list.RemoveAll((Thing t) => !t.Spawned);
			GenUI.clickedPawns.Clear();
			return list;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000ACC78 File Offset: 0x000AAE78
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPos - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPos - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000ACCBC File Offset: 0x000AAEBC
		private static int CompareThingsByDrawAltitude(Thing A, Thing B)
		{
			if (A.def.Altitude < B.def.Altitude)
			{
				return 1;
			}
			if (A.def.Altitude == B.def.Altitude)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000ACCF3 File Offset: 0x000AAEF3
		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000ACD32 File Offset: 0x000AAF32
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000ACD3F File Offset: 0x000AAF3F
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000ACD76 File Offset: 0x000AAF76
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000ACDB0 File Offset: 0x000AAFB0
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000ACE22 File Offset: 0x000AB022
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000ACE5B File Offset: 0x000AB05B
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000ACE94 File Offset: 0x000AB094
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000ACEB3 File Offset: 0x000AB0B3
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000ACEDE File Offset: 0x000AB0DE
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000ACF07 File Offset: 0x000AB107
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000ACF2C File Offset: 0x000AB12C
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000ACF49 File Offset: 0x000AB149
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000ACF80 File Offset: 0x000AB180
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000ACFB5 File Offset: 0x000AB1B5
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000ACFDC File Offset: 0x000AB1DC
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000AD005 File Offset: 0x000AB205
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000AD02A File Offset: 0x000AB22A
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000AD047 File Offset: 0x000AB247
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000AD07E File Offset: 0x000AB27E
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000AD0B3 File Offset: 0x000AB2B3
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000AD0DC File Offset: 0x000AB2DC
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			if (colors.Count == 0)
			{
				return Color.white;
			}
			int i = 0;
			while (i < colors.Count)
			{
				if (value < colors[i].First)
				{
					if (i == 0)
					{
						return colors[i].Second;
					}
					return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
				}
				else
				{
					i++;
				}
			}
			return colors.Last<Pair<float, Color>>().Second;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000AD188 File Offset: 0x000AB388
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = 0f;
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000AD238 File Offset: 0x000AB438
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000AD268 File Offset: 0x000AB468
		public static void DrawArrowPointingAt(Rect rect)
		{
			Vector2 vector = new Vector2((float)UI.screenWidth, (float)UI.screenHeight) / 2f;
			float angle = Mathf.Atan2(rect.center.x - vector.x, vector.y - rect.center.y) * 57.29578f;
			Vector2 vector2 = new Bounds(rect.center, rect.size).ClosestPoint(vector);
			Rect position = new Rect(vector2 + Vector2.left * (float)GenUI.ArrowTex.width * 0.5f, new Vector2((float)GenUI.ArrowTex.width, (float)GenUI.ArrowTex.height));
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 center = GUIUtility.GUIToScreenPoint(vector2);
			GUI.matrix = matrix;
			UI.RotateAroundPivot(angle, center);
			GUI.DrawTexture(position, GenUI.ArrowTex);
			GUI.matrix = matrix;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000AD374 File Offset: 0x000AB574
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000AD3D8 File Offset: 0x000AB5D8
		public static Rect DrawElementStack<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float rowMargin = 4f, float elementMargin = 5f, bool allowOrderOptimization = true)
		{
			GenUI.tmpRects.Clear();
			GenUI.tmpRects2.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int num = Mathf.FloorToInt(rect.height / rowHeight);
			List<GenUI.StackedElementRect> list = GenUI.tmpRects;
			float num3;
			float num2;
			if (allowOrderOptimization)
			{
				num2 = (num3 = 0f);
				while (num2 < (float)num)
				{
					GenUI.StackedElementRect item = default(GenUI.StackedElementRect);
					int num4 = -1;
					for (int j = 0; j < list.Count; j++)
					{
						GenUI.StackedElementRect stackedElementRect = list[j];
						if (num4 == -1 || (item.rect.width < stackedElementRect.rect.width && stackedElementRect.rect.width < rect.width - num3))
						{
							num4 = j;
							item = stackedElementRect;
						}
					}
					if (num4 == -1)
					{
						if (num3 == 0f)
						{
							break;
						}
						num3 = 0f;
						num2 += 1f;
					}
					else
					{
						num3 += item.rect.width + elementMargin;
						GenUI.tmpRects2.Add(item);
					}
					list.RemoveAt(num4);
					if (list.Count <= 0)
					{
						break;
					}
				}
				list = GenUI.tmpRects2;
			}
			num2 = (num3 = 0f);
			while (list.Count > 0)
			{
				GenUI.StackedElementRect stackedElementRect2 = list[0];
				if (num3 + stackedElementRect2.rect.width > rect.width)
				{
					num3 = 0f;
					num2 += rowHeight + rowMargin;
				}
				drawer(new Rect(rect.x + num3, rect.y + num2, stackedElementRect2.rect.width, stackedElementRect2.rect.height), elements[stackedElementRect2.elementIndex]);
				num3 += stackedElementRect2.rect.width + elementMargin;
				list.RemoveAt(0);
			}
			return new Rect(rect.x, rect.y, rect.width, num2 + rowHeight);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000AD5E8 File Offset: 0x000AB7E8
		public static Rect DrawElementStackVertical<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float elementMargin = 5f)
		{
			GenUI.tmpRects.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int elem = Mathf.FloorToInt(rect.height / rowHeight);
			GenUI.spacingCache.Reset(elem);
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j < GenUI.tmpRects.Count; j++)
			{
				GenUI.StackedElementRect stackedElementRect = GenUI.tmpRects[j];
				if (num3 + stackedElementRect.rect.height > rect.height)
				{
					num3 = 0f;
					num = 0;
				}
				drawer(new Rect(rect.x + GenUI.spacingCache.GetSpaceFor(num), rect.y + num3, stackedElementRect.rect.width, stackedElementRect.rect.height), elements[stackedElementRect.elementIndex]);
				num3 += stackedElementRect.rect.height + elementMargin;
				GenUI.spacingCache.AddSpace(num, stackedElementRect.rect.width + elementMargin);
				num2 = Mathf.Max(num2, GenUI.spacingCache.GetSpaceFor(num));
				num++;
			}
			return new Rect(rect.x, rect.y, num2, num3 + rowHeight);
		}

		// Token: 0x040010B4 RID: 4276
		public const float Pad = 10f;

		// Token: 0x040010B5 RID: 4277
		public const float GapTiny = 4f;

		// Token: 0x040010B6 RID: 4278
		public const float GapSmall = 10f;

		// Token: 0x040010B7 RID: 4279
		public const float Gap = 17f;

		// Token: 0x040010B8 RID: 4280
		public const float GapWide = 26f;

		// Token: 0x040010B9 RID: 4281
		public const float ListSpacing = 28f;

		// Token: 0x040010BA RID: 4282
		public const float MouseAttachIconSize = 32f;

		// Token: 0x040010BB RID: 4283
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x040010BC RID: 4284
		public const float ScrollBarWidth = 16f;

		// Token: 0x040010BD RID: 4285
		public const float HorizontalSliderHeight = 16f;

		// Token: 0x040010BE RID: 4286
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x040010BF RID: 4287
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x040010C0 RID: 4288
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x040010C1 RID: 4289
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x040010C2 RID: 4290
		public const float SmallIconSize = 24f;

		// Token: 0x040010C3 RID: 4291
		public const int RootGUIDepth = 50;

		// Token: 0x040010C4 RID: 4292
		private const float MouseIconSize = 32f;

		// Token: 0x040010C5 RID: 4293
		private const float MouseIconOffset = 12f;

		// Token: 0x040010C6 RID: 4294
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x040010C7 RID: 4295
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x040010C8 RID: 4296
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x040010C9 RID: 4297
		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		// Token: 0x040010CA RID: 4298
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x040010CB RID: 4299
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x040010CC RID: 4300
		private static List<Pawn> clickedPawns = new List<Pawn>();

		// Token: 0x040010CD RID: 4301
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x040010CE RID: 4302
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x040010CF RID: 4303
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x040010D0 RID: 4304
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x040010D1 RID: 4305
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x02001639 RID: 5689
		private struct StackedElementRect
		{
			// Token: 0x0600842B RID: 33835 RVA: 0x002AF8F0 File Offset: 0x002ADAF0
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x04005568 RID: 21864
			public Rect rect;

			// Token: 0x04005569 RID: 21865
			public int elementIndex;
		}

		// Token: 0x0200163A RID: 5690
		public class AnonymousStackElement
		{
			// Token: 0x0400556A RID: 21866
			public Action<Rect> drawer;

			// Token: 0x0400556B RID: 21867
			public float width;
		}

		// Token: 0x0200163B RID: 5691
		private struct SpacingCache
		{
			// Token: 0x0600842D RID: 33837 RVA: 0x002AF900 File Offset: 0x002ADB00
			public void Reset(int elem = 16)
			{
				if (this.spaces == null || this.maxElements != elem)
				{
					this.maxElements = elem;
					this.spaces = new float[this.maxElements];
					return;
				}
				for (int i = 0; i < this.maxElements; i++)
				{
					this.spaces[i] = 0f;
				}
			}

			// Token: 0x0600842E RID: 33838 RVA: 0x002AF955 File Offset: 0x002ADB55
			public float GetSpaceFor(int elem)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					return this.spaces[elem];
				}
				return 0f;
			}

			// Token: 0x0600842F RID: 33839 RVA: 0x002AF98B File Offset: 0x002ADB8B
			public void AddSpace(int elem, float space)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					this.spaces[elem] += space;
				}
			}

			// Token: 0x0400556C RID: 21868
			private int maxElements;

			// Token: 0x0400556D RID: 21869
			private float[] spaces;
		}

		// Token: 0x0200163C RID: 5692
		// (Invoke) Token: 0x06008431 RID: 33841
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x0200163D RID: 5693
		// (Invoke) Token: 0x06008435 RID: 33845
		public delegate float StackElementWidthGetter<T>(T element);
	}
}
