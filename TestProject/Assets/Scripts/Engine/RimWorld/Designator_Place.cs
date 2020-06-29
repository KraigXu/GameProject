using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public abstract class Designator_Place : Designator
	{
		
		// (get) Token: 0x06005837 RID: 22583
		public abstract BuildableDef PlacingDef { get; }

		
		public Designator_Place()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragBuilding;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlaceBuilding;
		}

		
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			Map currentMap = Find.CurrentMap;
			ThingDef thingDef;
			if (currentMap == null || (thingDef = (this.PlacingDef as ThingDef)) == null || thingDef.displayNumbersBetweenSameDefDistRange.max <= 0f)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			Designator_Place.tmpThings.Clear();
			Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsOfDef(thingDef));
			Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint));
			foreach (Thing thing in Designator_Place.tmpThings)
			{
				if ((thing.def == thingDef || thing.def.entityDefToBuild == thingDef) && (thing.Position.x == intVec.x || thing.Position.z == intVec.z) && this.CanDrawNumbersBetween(thing, thingDef, intVec, thing.Position, currentMap))
				{
					IntVec3 intVec2 = thing.Position - intVec;
					intVec2.x = Mathf.Abs(intVec2.x) + 1;
					intVec2.z = Mathf.Abs(intVec2.z) + 1;
					if (intVec2.x >= 3)
					{
						Vector2 screenPos = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
						screenPos.y = thing.Position.ToUIPosition().y;
						Color textColor = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.x) ? Color.white : Color.red;
						Widgets.DrawNumberOnMap(screenPos, intVec2.x, textColor);
					}
					if (intVec2.z >= 3)
					{
						Vector2 screenPos2 = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
						screenPos2.x = thing.Position.ToUIPosition().x;
						Color textColor2 = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.z) ? Color.white : Color.red;
						Widgets.DrawNumberOnMap(screenPos2, intVec2.z, textColor2);
					}
				}
			}
			Designator_Place.tmpThings.Clear();
		}

		
		protected virtual bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return !GenThing.CloserThingBetween(def, a, b, map, null);
		}

		
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
				Find.WindowStack.ImmediateWindow(73095, winRect, WindowLayer.GameUI, delegate
				{
					RotationDirection rotationDirection = RotationDirection.None;
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Medium;
					Rect rect = new Rect(winRect.width / 2f - 64f - 5f, 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect, TexUI.RotLeftTex, true))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Counterclockwise;
						Event.current.Use();
					}
					Widgets.Label(rect, KeyBindingDefOf.Designator_RotateLeft.MainKeyLabel);
					Rect rect2 = new Rect(winRect.width / 2f + 5f, 15f, 64f, 64f);
					if (Widgets.ButtonImage(rect2, TexUI.RotRightTex, true))
					{
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						rotationDirection = RotationDirection.Clockwise;
						Event.current.Use();
					}
					Widgets.Label(rect2, KeyBindingDefOf.Designator_RotateRight.MainKeyLabel);
					if (rotationDirection != RotationDirection.None)
					{
						this.placingRot.Rotate(rotationDirection);
					}
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
				}, true, false, 1f);
			}
		}

		
		public override void SelectedProcessInput(Event ev)
		{
			base.SelectedProcessInput(ev);
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				this.HandleRotationShortcuts();
			}
		}

		
		public override void SelectedUpdate()
		{
			GenDraw.DrawNoBuildEdgeLines();
			IntVec3 intVec = UI.MouseCell();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted) && intVec.InBounds(base.Map))
			{
				if (this.PlacingDef is TerrainDef)
				{
					GenUI.RenderMouseoverBracket();
					return;
				}
				Color ghostCol;
				if (this.CanDesignateCell(intVec).Accepted)
				{
					ghostCol = Designator_Place.CanPlaceColor;
				}
				else
				{
					ghostCol = Designator_Place.CannotPlaceColor;
				}
				this.DrawGhost(ghostCol);
				if (this.CanDesignateCell(intVec).Accepted && this.PlacingDef.specialDisplayRadius > 0.01f)
				{
					GenDraw.DrawRadiusRing(intVec, this.PlacingDef.specialDisplayRadius);
				}
				GenDraw.DrawInteractionCell((ThingDef)this.PlacingDef, intVec, this.placingRot);
			}
		}

		
		protected virtual void DrawGhost(Color ghostCol)
		{
			ThingDef def;
			if ((def = (this.PlacingDef as ThingDef)) != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, def, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
			}
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, null, ghostCol, AltitudeLayer.Blueprint, null);
		}

		
		private void HandleRotationShortcuts()
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Place.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Place.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection == RotationDirection.Clockwise)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Clockwise);
			}
			if (rotationDirection == RotationDirection.Counterclockwise)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				this.placingRot.Rotate(RotationDirection.Counterclockwise);
			}
		}

		
		public override void Selected()
		{
			this.placingRot = this.PlacingDef.defaultPlacingRot;
		}

		
		protected Rot4 placingRot = Rot4.North;

		
		protected static float middleMouseDownTime;

		
		private const float RotButSize = 64f;

		
		private const float RotButSpacing = 10f;

		
		public static readonly Color CanPlaceColor = new Color(0.5f, 1f, 0.6f, 0.4f);

		
		public static readonly Color CannotPlaceColor = new Color(1f, 0f, 0f, 0.4f);

		
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
