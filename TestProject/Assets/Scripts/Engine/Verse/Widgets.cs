﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003D6 RID: 982
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x06001CC6 RID: 7366 RVA: 0x000AF9A4 File Offset: 0x000ADBA4
		static Widgets()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
			Widgets.LineMat = (Material)typeof(GUI).GetMethod("get_blendMaterial", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000AFE4C File Offset: 0x000AE04C
		public static bool CanDrawIconFor(Def def)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				return buildableDef.uiIcon != null;
			}
			FactionDef factionDef;
			return (factionDef = (def as FactionDef)) != null && factionDef.FactionIcon != null;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000AFE88 File Offset: 0x000AE088
		public static void DefIcon(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, bool drawPlaceholder = false)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				rect.position += new Vector2(buildableDef.uiIconOffset.x * rect.size.x, buildableDef.uiIconOffset.y * rect.size.y);
			}
			ThingDef thingDef;
			if ((thingDef = (def as ThingDef)) != null)
			{
				Widgets.ThingIcon(rect, thingDef, stuffDef, scale);
				return;
			}
			RecipeDef recipeDef;
			if ((recipeDef = (def as RecipeDef)) != null && recipeDef.ProducedThingDef != null)
			{
				Widgets.ThingIcon(rect, recipeDef.ProducedThingDef, null, scale);
				return;
			}
			TerrainDef terrainDef;
			if ((terrainDef = (def as TerrainDef)) != null && terrainDef.uiIcon != null)
			{
				GUI.color = terrainDef.uiIconColor;
				Widgets.DrawTextureFitted(rect, terrainDef.uiIcon, scale, Vector2.one, Widgets.CroppedTerrainTextureRect(terrainDef.uiIcon), 0f, null);
				GUI.color = Color.white;
				return;
			}
			FactionDef factionDef;
			if ((factionDef = (def as FactionDef)) != null)
			{
				if (!factionDef.colorSpectrum.NullOrEmpty<Color>())
				{
					GUI.color = factionDef.colorSpectrum.FirstOrDefault<Color>();
				}
				Widgets.DrawTextureFitted(rect, factionDef.FactionIcon, scale);
				GUI.color = Color.white;
				return;
			}
			if (drawPlaceholder)
			{
				Widgets.DrawTextureFitted(rect, Widgets.PlaceholderIconTex, scale);
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000AFFC0 File Offset: 0x000AE1C0
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f)
		{
			thing = thing.GetInnerIfMinified();
			GUI.color = thing.DrawColor;
			float resolvedIconAngle = 0f;
			Texture resolvedIcon;
			if (!thing.def.uiIconPath.NullOrEmpty())
			{
				resolvedIcon = thing.def.uiIcon;
				resolvedIconAngle = thing.def.uiIconAngle;
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Pawn pawn = thing as Pawn;
				if (pawn == null)
				{
					pawn = ((Corpse)thing).InnerPawn;
				}
				if (!pawn.RaceProps.Humanlike)
				{
					if (!pawn.Drawer.renderer.graphics.AllResolved)
					{
						pawn.Drawer.renderer.graphics.ResolveAllGraphics();
					}
					Material material = pawn.Drawer.renderer.graphics.nakedGraphic.MatAt(Rot4.East, null);
					resolvedIcon = material.mainTexture;
					GUI.color = material.color;
				}
				else
				{
					rect = rect.ScaledBy(1.8f);
					rect.y += 3f;
					rect = rect.Rounded();
					resolvedIcon = PortraitsCache.Get(pawn, new Vector2(rect.width, rect.height), default(Vector3), 1f, true, true);
				}
			}
			else
			{
				resolvedIcon = thing.Graphic.ExtractInnerGraphicFor(thing).MatAt(thing.def.defaultPlacingRot, null).mainTexture;
			}
			if (alpha != 1f)
			{
				Color color = GUI.color;
				color.a *= alpha;
				GUI.color = color;
			}
			Widgets.ThingIconWorker(rect, thing.def, resolvedIcon, resolvedIconAngle, 1f);
			GUI.color = Color.white;
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000B01B4 File Offset: 0x000AE3B4
		public static void ThingIcon(Rect rect, ThingDef thingDef, ThingDef stuffDef = null, float scale = 1f)
		{
			if (thingDef.uiIcon == null || thingDef.uiIcon == BaseContent.BadTex)
			{
				return;
			}
			Texture2D resolvedIcon = thingDef.uiIcon;
			Graphic_Appearances graphic_Appearances;
			if ((graphic_Appearances = (thingDef.graphic as Graphic_Appearances)) != null)
			{
				resolvedIcon = (Texture2D)graphic_Appearances.SubGraphicFor(stuffDef).MatAt(thingDef.defaultPlacingRot, null).mainTexture;
			}
			if (stuffDef != null)
			{
				GUI.color = thingDef.GetColorForStuff(stuffDef);
			}
			else
			{
				GUI.color = (thingDef.MadeFromStuff ? thingDef.GetColorForStuff(GenStuff.DefaultStuffFor(thingDef)) : thingDef.uiIconColor);
			}
			Widgets.ThingIconWorker(rect, thingDef, resolvedIcon, thingDef.uiIconAngle, scale);
			GUI.color = Color.white;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000B0264 File Offset: 0x000AE464
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle, float scale = 1f)
		{
			Vector2 texProportions = new Vector2((float)resolvedIcon.width, (float)resolvedIcon.height);
			Rect texCoords = Widgets.DefaultTexCoords;
			if (thingDef.graphicData != null)
			{
				texProportions = thingDef.graphicData.drawSize.RotatedBy(thingDef.defaultPlacingRot);
				if (thingDef.uiIconPath.NullOrEmpty() && thingDef.graphicData.linkFlags != LinkFlags.None)
				{
					texCoords = Widgets.LinkedTexCoords;
				}
			}
			Widgets.DrawTextureFitted(rect, resolvedIcon, GenUI.IconDrawScale(thingDef) * scale, texProportions, texCoords, resolvedIconAngle, null);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000B02DF File Offset: 0x000AE4DF
		public static Rect CroppedTerrainTextureRect(Texture2D tex)
		{
			return new Rect(0f, 0f, 64f / (float)tex.width, 64f / (float)tex.height);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000B030A File Offset: 0x000AE50A
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000B0318 File Offset: 0x000AE518
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000B038C File Offset: 0x000AE58C
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect screenRect = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			Graphics.DrawTexture(screenRect, Widgets.LineTexAA, Widgets.LineRect, 0, 0, 0, 0, color, Widgets.LineMat);
			GL.PopMatrix();
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000B0498 File Offset: 0x000AE698
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, length, 1f), BaseContent.WhiteTex);
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000B04B1 File Offset: 0x000AE6B1
		public static void DrawLineVertical(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, 1f, length), BaseContent.WhiteTex);
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000B04CA File Offset: 0x000AE6CA
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000B04E8 File Offset: 0x000AE6E8
		public static void DrawBox(Rect rect, int thickness = 1)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				float x = vector.x;
				vector.x = vector2.x;
				vector2.x = x;
			}
			if (vector.y > vector2.y)
			{
				float y = vector.y;
				vector.y = vector2.y;
				vector2.y = y;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(new Rect(vector.x, vector.y, (float)thickness, vector3.y), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness), BaseContent.WhiteTex);
			GUI.DrawTexture(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness), BaseContent.WhiteTex);
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000B0634 File Offset: 0x000AE834
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000B068C File Offset: 0x000AE88C
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000B069C File Offset: 0x000AE89C
		public static void Label(Rect rect, string label)
		{
			Rect position = rect;
			float num = Prefs.UIScale / 2f;
			if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > 1.401298E-45f)
			{
				position.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
				position.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
				position.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
				position.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
			}
			GUI.Label(position, label, Text.CurFontStyle);
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000B073B File Offset: 0x000AE93B
		public static void Label(Rect rect, TaggedString label)
		{
			Widgets.Label(rect, label.Resolve());
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000B074C File Offset: 0x000AE94C
		public static void LongLabel(float x, float width, string label, ref float curY, bool draw = true)
		{
			if (label.Length < 2500)
			{
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 1000f), label);
				}
				curY += Text.CalcHeight(label, width);
				return;
			}
			int num = 0;
			int num2 = -1;
			bool flag = false;
			for (int i = 0; i < label.Length; i++)
			{
				if (label[i] == '\n')
				{
					num++;
					if (num >= 50)
					{
						string text = label.Substring(num2 + 1, i - num2 - 1);
						num2 = i;
						num = 0;
						if (flag)
						{
							curY += Text.SpaceBetweenLines;
						}
						if (draw)
						{
							Widgets.Label(new Rect(x, curY, width, 10000f), text);
						}
						curY += Text.CalcHeight(text, width);
						flag = true;
					}
				}
			}
			if (num2 != label.Length - 1)
			{
				if (flag)
				{
					curY += Text.SpaceBetweenLines;
				}
				string text2 = label.Substring(num2 + 1);
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 10000f), text2);
				}
				curY += Text.CalcHeight(text2, width);
			}
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000B084C File Offset: 0x000AEA4C
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false, bool takeScrollbarSpaceEvenIfNoScrollbar = true, bool longLabel = false)
		{
			bool flag = takeScrollbarSpaceEvenIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			bool flag2 = flag && (!dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width - 16f) > rect.height);
			float num = rect.width;
			if (flag)
			{
				num -= 16f;
			}
			float num2;
			if (longLabel)
			{
				num2 = 0f;
				Widgets.LongLabel(0f, num, label, ref num2, false);
			}
			else
			{
				num2 = Text.CalcHeight(label, num);
			}
			Rect rect2 = new Rect(0f, 0f, num, Mathf.Max(num2 + 5f, rect.height));
			if (flag2)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				GUI.BeginGroup(rect);
			}
			if (longLabel)
			{
				float y = rect2.y;
				Widgets.LongLabel(rect2.x, rect2.width, label, ref y, true);
			}
			else
			{
				Widgets.Label(rect2, label);
			}
			if (flag2)
			{
				Widgets.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000B0948 File Offset: 0x000AEB48
		public static void DefLabelWithIcon(Rect rect, Def def, float iconMargin = 2f, float textOffsetX = 6f)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, def.description);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			Widgets.DefIcon(rect2, def, null, 1f, true);
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, def.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.EndGroup();
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000B09F8 File Offset: 0x000AEBF8
		public static void HyperlinkWithIcon(Rect rect, Dialog_InfoCard.Hyperlink hyperlink, string text = null, float iconMargin = 2f, float textOffsetX = 6f)
		{
			string label = text ?? hyperlink.Label.CapitalizeFirst();
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			if (hyperlink.thing != null)
			{
				Widgets.ThingIcon(rect2, hyperlink.thing, 1f);
			}
			else
			{
				Widgets.DefIcon(rect2, hyperlink.def, null, 1f, true);
			}
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.ButtonText(rect3, label, false, false, false);
			if (Widgets.ButtonInvisible(rect3, true))
			{
				hyperlink.OpenDialog();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.EndGroup();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000B0AD4 File Offset: 0x000AECD4
		public static void DrawNumberOnMap(Vector2 screenPos, int number, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, number.ToStringCached());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000B0B46 File Offset: 0x000AED46
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000B0B64 File Offset: 0x000AED64
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				MouseoverSounds.DoRegion(rect);
				bool flag = false;
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
				if (draggableResult == Widgets.DraggableResult.Pressed)
				{
					checkOn = !checkOn;
					flag = true;
				}
				else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
				{
					checkOn = !checkOn;
					flag = true;
					Widgets.checkboxPainting = true;
					Widgets.checkboxPaintingState = checkOn;
				}
				if (paintable && Mouse.IsOver(rect) && Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
				{
					checkOn = Widgets.checkboxPaintingState;
					flag = true;
				}
				if (flag)
				{
					if (checkOn)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000B0C3C File Offset: 0x000AEE3C
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Widgets.Label(rect, label);
			if (!disabled && Widgets.ButtonInvisible(rect, true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y, checkOn, disabled, 24f, null, null);
			Text.Anchor = anchor;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000B0CE8 File Offset: 0x000AEEE8
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			if (Widgets.ButtonInvisible(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000B0DD8 File Offset: 0x000AEFD8
		private static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((texChecked != null) ? texChecked : Widgets.CheckboxOnTex);
			}
			else
			{
				image = ((texUnchecked != null) ? texUnchecked : Widgets.CheckboxOffTex);
			}
			GUI.DrawTexture(new Rect(x, y, size, size), image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000B0E40 File Offset: 0x000AF040
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state == MultiCheckboxState.Off) ? MultiCheckboxState.On : MultiCheckboxState.Off;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = (Widgets.checkboxPaintingState ? MultiCheckboxState.On : MultiCheckboxState.Off);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				return multiCheckboxState;
			}
			return state;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000B0EEE File Offset: 0x000AF0EE
		public static bool RadioButton(Vector2 topLeft, bool chosen)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000B0F02 File Offset: 0x000AF102
		public static bool RadioButton(float x, float y, bool chosen)
		{
			Rect butRect = new Rect(x, y, 24f, 24f);
			Widgets.RadioButtonDraw(x, y, chosen);
			bool flag = Widgets.ButtonInvisible(butRect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000B0F34 File Offset: 0x000AF134
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, labelText);
			Text.Anchor = anchor;
			bool flag = Widgets.ButtonInvisible(rect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen);
			return flag;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000B0FA8 File Offset: 0x000AF1A8
		private static void RadioButtonDraw(float x, float y, bool chosen)
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			GUI.DrawTexture(new Rect(x, y, 24f, 24f), image);
			GUI.color = color;
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000B0FF1 File Offset: 0x000AF1F1
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = true, bool active = true)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000B1003 File Offset: 0x000AF203
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false).AnyPressed();
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x000B1018 File Offset: 0x000AF218
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x06001CEA RID: 7402 RVA: 0x000B102A File Offset: 0x000AF22A
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000B1040 File Offset: 0x000AF240
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Texture2D atlas = Widgets.ButtonBGAtlas;
				if (Mouse.IsOver(rect))
				{
					atlas = Widgets.ButtonBGAtlasMouseover;
					if (Input.GetMouseButton(0))
					{
						atlas = Widgets.ButtonBGAtlasClick;
					}
				}
				Widgets.DrawAtlas(rect, atlas);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			if (active && draggable)
			{
				return Widgets.ButtonInvisibleDraggable(rect, false);
			}
			if (!active)
			{
				return Widgets.DraggableResult.Idle;
			}
			if (!Widgets.ButtonInvisible(rect, false))
			{
				return Widgets.DraggableResult.Idle;
			}
			return Widgets.DraggableResult.Pressed;
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000B1113 File Offset: 0x000AF313
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x000B113C File Offset: 0x000AF33C
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, bool cacheHeight = false, int borderSize = 1, bool doMouseoverSound = true, bool active = true)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += (float)borderSize;
			position.y += (float)borderSize;
			position.width -= (float)(borderSize * 2);
			position.height -= (float)(borderSize * 2);
			Widgets.DrawRectFast(rect, borderColor, null);
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000B1228 File Offset: 0x000AF428
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2))
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Widgets.FillableBar(rect.ContractedBy(1f), barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect3 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect3.x += textLeftMargin;
			if (flag)
			{
				rect3.x += 2f;
				rect3.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			return Widgets.ButtonInvisible(rect2, false);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000B133C File Offset: 0x000AF53C
		public static bool ButtonImage(Rect butRect, Texture2D tex, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white, doMouseoverSound);
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000B134B File Offset: 0x000AF54B
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor, doMouseoverSound);
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000B135B File Offset: 0x000AF55B
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool doMouseoverSound = true)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, doMouseoverSound);
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000B1388 File Offset: 0x000AF588
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x000B1396 File Offset: 0x000AF596
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x000B13A5 File Offset: 0x000AF5A5
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000B13D1 File Offset: 0x000AF5D1
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000B13DF File Offset: 0x000AF5DF
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000B13EE File Offset: 0x000AF5EE
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, true);
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000B1420 File Offset: 0x000AF620
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, true, true);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000B14C8 File Offset: 0x000AF6C8
		public static bool CloseButtonFor(Rect rectToClose)
		{
			return Widgets.ButtonImage(new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f), TexButton.CloseXSmall, true);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000B1518 File Offset: 0x000AF718
		public static bool BackButtonFor(Rect rectToBack)
		{
			return Widgets.ButtonText(new Rect(rectToBack.x + rectToBack.width - 18f - 4f - 120f - 16f, rectToBack.y + 18f, 120f, 40f), "Back".Translate(), true, true, true);
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000B157F File Offset: 0x000AF77F
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = true)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000B159C File Offset: 0x000AF79C
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (!Mouse.IsOver(butRect))
					{
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged)
					{
						return Widgets.DraggableResult.Pressed;
					}
					return Widgets.DraggableResult.DraggedThenPressed;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000B1649 File Offset: 0x000AF849
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000B1664 File Offset: 0x000AF864
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator)
		{
			string text2 = Widgets.TextField(rect, text);
			if (text2.Length <= maxLength && inputValidator.IsMatch(text2))
			{
				return text2;
			}
			return text;
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000B168E File Offset: 0x000AF88E
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000B16B0 File Offset: 0x000AF8B0
		public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
			Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			string result = Widgets.TextArea(rect2, text, readOnly);
			Widgets.EndScrollView();
			return result;
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000B1710 File Offset: 0x000AF910
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (rect.height <= 30f)
			{
				return Widgets.TextField(rect3, text);
			}
			return Widgets.TextArea(rect3, text, false);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000B176C File Offset: 0x000AF96C
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			string text = "TextField" + rect.y.ToString("F0") + rect.x.ToString("F0");
			GUI.SetNextControlName(text);
			string text2 = GUI.TextField(rect, buffer, Text.CurTextFieldStyle);
			if (GUI.GetNameOfFocusedControl() != text)
			{
				Widgets.ResolveParseNow<T>(buffer, ref val, ref buffer, min, max, true);
				return;
			}
			if (text2 != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text2, min, max))
			{
				buffer = text2;
				if (text2.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text2, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000B181C File Offset: 0x000AFA1C
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
				int num;
				if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else
			{
				Log.Error("TextField<T> does not support " + typeof(T), false);
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000B1918 File Offset: 0x000AFB18
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000B1978 File Offset: 0x000AFB78
		private static string ToStringTypedIn<T>(T val)
		{
			if (typeof(T) == typeof(float))
			{
				return ((float)((object)val)).ToString("0.##########");
			}
			return val.ToString();
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000B19C8 File Offset: 0x000AFBC8
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			return s == "" || ((s[0] != '-' || min < 0f) && (s.Length <= 1 || s[s.Length - 1] != '-') && !(s == "00") && s.Length <= 12 && ((typeof(T) == typeof(float) && s.CharacterCount('.') <= 1 && s.ContainsOnlyCharacters("-.0123456789")) || s.IsFullyTypedNumber<T>()));
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000B1A74 File Offset: 0x000AFC74
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			if (s == "")
			{
				return false;
			}
			if (typeof(T) == typeof(float))
			{
				string[] array = s.Split(new char[]
				{
					'.'
				});
				if (array.Length > 2 || array.Length < 1)
				{
					return false;
				}
				if (!array[0].ContainsOnlyCharacters("-0123456789"))
				{
					return false;
				}
				if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
				{
					return false;
				}
			}
			return !(typeof(T) == typeof(int)) || s.ContainsOnlyCharacters("-0123456789");
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000B1B28 File Offset: 0x000AFD28
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000B1B58 File Offset: 0x000AFD58
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000B1B88 File Offset: 0x000AFD88
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000B1BD0 File Offset: 0x000AFDD0
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Widgets.Label(new Rect(rect2.xMax, rect.y, 25f, rect2.height), "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000B1C6C File Offset: 0x000AFE6C
		public static T ChangeType<T>(this object obj)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (T)((object)Convert.ChangeType(obj, typeof(T), invariantCulture));
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000B1C98 File Offset: 0x000AFE98
		public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 16f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Tiny;
				float num2 = label.NullOrEmpty() ? 18f : Text.CalcSize(label).y;
				rect.y = rect.y - num2 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			if (value != num)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			return num;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000B1DD8 File Offset: 0x000AFFD8
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate((1f / freq).ToString("0.##"));
			}
			else
			{
				label = "EveryDays".Translate(freq.ToString("0.##"));
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000B1F24 File Offset: 0x000B0124
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, true, true))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, true, true))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), "+" + (10 * multiplier).ToStringCached(), true, true, true))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), "+" + multiplier.ToStringCached(), true, true, true))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000B20E0 File Offset: 0x000B02E0
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			rect3.height = Mathf.Max(rect3.height, Text.CalcHeight(text, rect3.width));
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
			float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num5 = Mathf.Clamp(num5, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = num5;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000B2478 File Offset: 0x000B0678
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt(Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min, (float)min, (float)max));
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num6 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num6)
							{
								range.max = num6;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = num5;
						if (range.max < min + minWidth)
						{
							range.max = min + minWidth;
						}
						int num7 = Mathf.Min(max, range.max - minWidth);
						if (range.min > num7)
						{
							range.min = num7;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000B2834 File Offset: 0x000B0A34
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000B2860 File Offset: 0x000B0A60
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(range.min.GetLabel());
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			int length = Enum.GetValues(typeof(QualityCategory)).Length;
			float num = rect2.x + rect2.width / (float)(length - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(length - 1) * (float)range.max;
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(length - 1));
					num5 = Mathf.Clamp(num5, 0, length - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num5)
						{
							range.min = (QualityCategory)num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && range.max != (QualityCategory)num5)
					{
						range.max = (QualityCategory)num5;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000B2C38 File Offset: 0x000B0E38
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			rect3.y += 4f;
			rect3.height += 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch, true))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000B2DB5 File Offset: 0x000B0FB5
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000B2DC4 File Offset: 0x000B0FC4
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000B2E00 File Offset: 0x000B1000
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000B2E50 File Offset: 0x000B1050
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000B2EB8 File Offset: 0x000B10B8
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000B2ED8 File Offset: 0x000B10D8
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate == 0)
			{
				return;
			}
			if (changeRate > Widgets.MaxFillableBarChangeRate)
			{
				changeRate = Widgets.MaxFillableBarChangeRate;
			}
			if (changeRate < -Widgets.MaxFillableBarChangeRate)
			{
				changeRate = -Widgets.MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Mathf.Abs(changeRate);
			float y = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = 8f;
				image = Widgets.FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - 8f - 2f;
				num4 = -8f;
				image = Widgets.FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				GUI.DrawTexture(new Rect(num3, y, 8f, num), image);
				num3 += num4;
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000B2FB9 File Offset: 0x000B11B9
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000B2FEB File Offset: 0x000B11EB
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000B301D File Offset: 0x000B121D
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000B304F File Offset: 0x000B124F
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000B3081 File Offset: 0x000B1281
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000B30B3 File Offset: 0x000B12B3
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000B30CC File Offset: 0x000B12CC
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000B3116 File Offset: 0x000B1316
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000B3120 File Offset: 0x000B1320
		private static Rect AdjustRectToUIScaling(Rect rect)
		{
			Rect result = rect;
			result.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
			result.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
			result.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax);
			result.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax);
			return result;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000B317C File Offset: 0x000B137C
		public static float AdjustCoordToUIScalingFloor(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Floor(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000B31A4 File Offset: 0x000B13A4
		public static float AdjustCoordToUIScalingCeil(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Ceiling(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000B31CC File Offset: 0x000B13CC
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = Widgets.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = Widgets.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			GUI.BeginGroup(rect);
			Rect drawRect;
			Rect uvRect;
			if (drawTop)
			{
				drawRect = new Rect(0f, 0f, num, num);
				uvRect = new Rect(0f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
				drawRect = new Rect(rect.width - num, 0f, num, num);
				uvRect = new Rect(0.75f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(0f, rect.height - num, num, num);
			uvRect = new Rect(0f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, rect.height - num, num, num);
			uvRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			if (drawTop)
			{
				drawRect = new Rect(num, 0f, rect.width - num * 2f, num);
				uvRect = new Rect(0.25f, 0f, 0.5f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			uvRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			GUI.EndGroup();
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000B350F File Offset: 0x000B170F
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000B354E File Offset: 0x000B174E
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000B3574 File Offset: 0x000B1774
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000B35F8 File Offset: 0x000B17F8
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
				return;
			}
			scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x000B368D File Offset: 0x000B188D
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000B369F File Offset: 0x000B189F
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.", false);
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000B36C3 File Offset: 0x000B18C3
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000B36D0 File Offset: 0x000B18D0
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000B36E0 File Offset: 0x000B18E0
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000B36ED File Offset: 0x000B18ED
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000B36FA File Offset: 0x000B18FA
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x000B3708 File Offset: 0x000B1908
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					return Widgets.InfoCardButton(x, y, thingDef, constructible.EntityToBuildStuff());
				}
				return Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
			}
			else
			{
				if (Widgets.InfoCardButtonWorker(x, y))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing));
					return true;
				}
				return false;
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000B3771 File Offset: 0x000B1971
		public static bool InfoCardButton(float x, float y, Def def)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def));
				return true;
			}
			return false;
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000B378F File Offset: 0x000B198F
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef));
				return true;
			}
			return false;
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000B37AE File Offset: 0x000B19AE
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				return true;
			}
			return false;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000B37CC File Offset: 0x000B19CC
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000B37F8 File Offset: 0x000B19F8
		public static bool InfoCardButton(float x, float y, Faction faction)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
				return true;
			}
			return false;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000B3818 File Offset: 0x000B1A18
		private static bool InfoCardButtonWorker(float x, float y)
		{
			Rect rect = new Rect(x, y, 24f, 24f);
			MouseoverSounds.DoRegion(rect);
			TooltipHandler.TipRegionByKey(rect, "DefInfoTip");
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color, true);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000B3865 File Offset: 0x000B1A65
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, null);
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000B38A4 File Offset: 0x000B1AA4
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f, Material mat = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect = new Rect(0f, 0f, texProportions.x, texProportions.y);
			float num;
			if (rect.width / rect.height < outerRect.width / outerRect.height)
			{
				num = outerRect.height / rect.height;
			}
			else
			{
				num = outerRect.width / rect.width;
			}
			num *= scale;
			rect.width *= num;
			rect.height *= num;
			rect.x = outerRect.x + outerRect.width / 2f - rect.width / 2f;
			rect.y = outerRect.y + outerRect.height / 2f - rect.height / 2f;
			Matrix4x4 matrix = Matrix4x4.identity;
			if (angle != 0f)
			{
				matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
			}
			GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);
			if (angle != 0f)
			{
				GUI.matrix = matrix;
			}
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000B39D4 File Offset: 0x000B1BD4
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Widgets.DrawTextureRotated(new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2), tex, angle);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000B3A1F File Offset: 0x000B1C1F
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (angle == 0f)
			{
				GUI.DrawTexture(rect, tex);
				return;
			}
			Matrix4x4 matrix = GUI.matrix;
			UI.RotateAroundPivot(angle, rect.center);
			GUI.DrawTexture(rect, tex);
			GUI.matrix = matrix;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000B3A5D File Offset: 0x000B1C5D
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000B3A68 File Offset: 0x000B1C68
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000B3ACA File Offset: 0x000B1CCA
		public static void NoneLabelCenteredVertically(Rect rect, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000B3B08 File Offset: 0x000B1D08
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.Dropdown<Target, Payload>(rect, target, Color.white, getPayload, menuGenerator, buttonLabel, buttonIcon, dragLabel, dragIcon, dropdownOpened, paintable);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000B3B30 File Offset: 0x000B1D30
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Color iconColor, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			MouseoverSounds.DoRegion(rect);
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = iconColor;
				Widgets.DrawTextureFitted(rect, buttonIcon, 1f);
				GUI.color = Color.white;
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
					return;
				}
			}
			else
			{
				if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
				{
					Widgets.dropdownPainting = true;
					Widgets.dropdownPainting_Payload = getPayload(target);
					Widgets.dropdownPainting_Type = typeof(Payload);
					Widgets.dropdownPainting_Text = ((dragLabel != null) ? dragLabel : buttonLabel);
					Widgets.dropdownPainting_Icon = ((dragIcon != null) ? dragIcon : buttonIcon);
					return;
				}
				if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
				{
					FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
					where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
					select opt.option).FirstOrDefault<FloatMenuOption>();
					if (floatMenuOption != null && !floatMenuOption.Disabled)
					{
						Payload x = getPayload(target);
						floatMenuOption.action();
						Payload y = getPayload(target);
						if (!EqualityComparer<Payload>.Default.Equals(x, y))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					}
				}
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000B3CFC File Offset: 0x000B1EFC
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.checkboxPaintingState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null, false, default(Color));
			}
		}

		// Token: 0x04001172 RID: 4466
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x04001173 RID: 4467
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x04001174 RID: 4468
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x04001175 RID: 4469
		private static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x04001176 RID: 4470
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x04001177 RID: 4471
		private static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04001178 RID: 4472
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x04001179 RID: 4473
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x0400117A RID: 4474
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x0400117B RID: 4475
		public const float CheckboxSize = 24f;

		// Token: 0x0400117C RID: 4476
		public const float RadioButtonSize = 24f;

		// Token: 0x0400117D RID: 4477
		private static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x0400117E RID: 4478
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x0400117F RID: 4479
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x04001180 RID: 4480
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x04001181 RID: 4481
		private static readonly Texture2D PlaceholderIconTex = ContentFinder<Texture2D>.Get("UI/Icons/MenuOptionNoIcon", true);

		// Token: 0x04001182 RID: 4482
		private const int FillableBarBorderWidth = 3;

		// Token: 0x04001183 RID: 4483
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x04001184 RID: 4484
		private const int FillChangeArrowWidth = 8;

		// Token: 0x04001185 RID: 4485
		public const float CloseButtonSize = 18f;

		// Token: 0x04001186 RID: 4486
		public const float CloseButtonMargin = 4f;

		// Token: 0x04001187 RID: 4487
		public const float BackButtonWidth = 120f;

		// Token: 0x04001188 RID: 4488
		public const float BackButtonHeight = 40f;

		// Token: 0x04001189 RID: 4489
		public const float BackButtonMargin = 16f;

		// Token: 0x0400118A RID: 4490
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x0400118B RID: 4491
		private static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x0400118C RID: 4492
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x0400118D RID: 4493
		private static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x0400118E RID: 4494
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x0400118F RID: 4495
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x04001190 RID: 4496
		private static readonly Rect DefaultTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001191 RID: 4497
		private static readonly Rect LinkedTexCoords = new Rect(0f, 0.5f, 0.25f, 0.25f);

		// Token: 0x04001192 RID: 4498
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x04001193 RID: 4499
		private static Texture2D LineTexAA = null;

		// Token: 0x04001194 RID: 4500
		private static readonly Rect LineRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001195 RID: 4501
		private static readonly Material LineMat = null;

		// Token: 0x04001196 RID: 4502
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04001197 RID: 4503
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x04001198 RID: 4504
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x04001199 RID: 4505
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x0400119A RID: 4506
		private const float TileSize = 64f;

		// Token: 0x0400119B RID: 4507
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x0400119C RID: 4508
		private static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x0400119D RID: 4509
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x0400119E RID: 4510
		public const float ListSeparatorHeight = 25f;

		// Token: 0x0400119F RID: 4511
		private static bool checkboxPainting = false;

		// Token: 0x040011A0 RID: 4512
		private static bool checkboxPaintingState = false;

		// Token: 0x040011A1 RID: 4513
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x040011A2 RID: 4514
		private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(78, 109, 129, 130).ToColor);

		// Token: 0x040011A3 RID: 4515
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x040011A4 RID: 4516
		private static int buttonInvisibleDraggable_activeControl = 0;

		// Token: 0x040011A5 RID: 4517
		private static bool buttonInvisibleDraggable_dragged = false;

		// Token: 0x040011A6 RID: 4518
		private static Vector3 buttonInvisibleDraggable_mouseStart = Vector3.zero;

		// Token: 0x040011A7 RID: 4519
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x040011A8 RID: 4520
		public const float RangeControlCompactHeight = 28f;

		// Token: 0x040011A9 RID: 4521
		private const float RangeSliderSize = 16f;

		// Token: 0x040011AA RID: 4522
		private static readonly Color RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x040011AB RID: 4523
		private static int draggingId = 0;

		// Token: 0x040011AC RID: 4524
		private static Widgets.RangeEnd curDragEnd = Widgets.RangeEnd.None;

		// Token: 0x040011AD RID: 4525
		private static float lastDragSliderSoundTime = -1f;

		// Token: 0x040011AE RID: 4526
		private static float FillableBarChangeRateDisplayRatio = 1E+08f;

		// Token: 0x040011AF RID: 4527
		public static int MaxFillableBarChangeRate = 3;

		// Token: 0x040011B0 RID: 4528
		private static readonly Color WindowBGBorderColor = new ColorInt(97, 108, 122).ToColor;

		// Token: 0x040011B1 RID: 4529
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x040011B2 RID: 4530
		private static readonly Color MenuSectionBGFillColor = new ColorInt(42, 43, 44).ToColor;

		// Token: 0x040011B3 RID: 4531
		private static readonly Color MenuSectionBGBorderColor = new ColorInt(135, 135, 135).ToColor;

		// Token: 0x040011B4 RID: 4532
		private static readonly Color TutorWindowBGFillColor = new ColorInt(133, 85, 44).ToColor;

		// Token: 0x040011B5 RID: 4533
		private static readonly Color TutorWindowBGBorderColor = new ColorInt(176, 139, 61).ToColor;

		// Token: 0x040011B6 RID: 4534
		private static readonly Color OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);

		// Token: 0x040011B7 RID: 4535
		private static readonly Color OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;

		// Token: 0x040011B8 RID: 4536
		private static readonly Color OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);

		// Token: 0x040011B9 RID: 4537
		private static readonly Color OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;

		// Token: 0x040011BA RID: 4538
		public const float InfoCardButtonSize = 24f;

		// Token: 0x040011BB RID: 4539
		private static bool dropdownPainting = false;

		// Token: 0x040011BC RID: 4540
		private static object dropdownPainting_Payload = null;

		// Token: 0x040011BD RID: 4541
		private static Type dropdownPainting_Type = null;

		// Token: 0x040011BE RID: 4542
		private static string dropdownPainting_Text = "";

		// Token: 0x040011BF RID: 4543
		private static Texture2D dropdownPainting_Icon = null;

		// Token: 0x02001648 RID: 5704
		public enum DraggableResult
		{
			// Token: 0x04005594 RID: 21908
			Idle,
			// Token: 0x04005595 RID: 21909
			Pressed,
			// Token: 0x04005596 RID: 21910
			Dragged,
			// Token: 0x04005597 RID: 21911
			DraggedThenPressed
		}

		// Token: 0x02001649 RID: 5705
		private enum RangeEnd : byte
		{
			// Token: 0x04005599 RID: 21913
			None,
			// Token: 0x0400559A RID: 21914
			Min,
			// Token: 0x0400559B RID: 21915
			Max
		}

		// Token: 0x0200164A RID: 5706
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x0400559C RID: 21916
			public FloatMenuOption option;

			// Token: 0x0400559D RID: 21917
			public Payload payload;
		}
	}
}
