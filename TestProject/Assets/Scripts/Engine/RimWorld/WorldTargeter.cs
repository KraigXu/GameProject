using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F31 RID: 3889
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x06005F41 RID: 24385 RVA: 0x0020E493 File Offset: 0x0020C693
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x0020E49E File Offset: 0x0020C69E
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
		}

		// Token: 0x06005F43 RID: 24387 RVA: 0x0020E4CD File Offset: 0x0020C6CD
		public void StopTargeting()
		{
			if (this.closeWorldTabWhenFinished)
			{
				CameraJumper.TryHideWorld();
			}
			this.action = null;
			this.canTargetTiles = false;
			this.mouseAttachment = null;
			this.closeWorldTabWhenFinished = false;
			this.onUpdate = null;
			this.extraLabelGetter = null;
		}

		// Token: 0x06005F44 RID: 24388 RVA: 0x0020E508 File Offset: 0x0020C708
		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0 && this.IsTargeting)
				{
					GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
					if (this.action(arg))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.IsTargeting)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x0020E5C4 File Offset: 0x0020C7C4
		public void TargeterOnGUI()
		{
			if (this.IsTargeting && !Mouse.IsInputBlockedNow)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Texture2D image = this.mouseAttachment ?? TexCommand.Attack;
				Rect position = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
				GUI.DrawTexture(position, image);
				if (this.extraLabelGetter != null)
				{
					GUI.color = Color.white;
					string text = this.extraLabelGetter(this.CurrentTargetUnderMouse());
					if (!text.NullOrEmpty())
					{
						Color color = GUI.color;
						GUI.color = Color.white;
						Rect rect = new Rect(position.xMax, position.y, 9999f, 100f);
						Vector2 vector = Text.CalcSize(text);
						GUI.DrawTexture(new Rect(rect.x - vector.x * 0.1f, rect.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
						GUI.color = color;
						Widgets.Label(rect, text);
					}
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x06005F46 RID: 24390 RVA: 0x0020E6F0 File Offset: 0x0020C8F0
		public void TargeterUpdate()
		{
			if (this.IsTargeting)
			{
				Vector3 pos = Vector3.zero;
				GlobalTargetInfo globalTargetInfo = this.CurrentTargetUnderMouse();
				if (globalTargetInfo.HasWorldObject)
				{
					pos = globalTargetInfo.WorldObject.DrawPos;
				}
				else if (globalTargetInfo.Tile >= 0)
				{
					pos = Find.WorldGrid.GetTileCenter(globalTargetInfo.Tile);
				}
				if (globalTargetInfo.IsValid && !Mouse.IsInputBlockedNow)
				{
					WorldRendererUtility.DrawQuadTangentialToPlanet(pos, 0.8f * Find.WorldGrid.averageTileSize, 0.018f, WorldMaterials.CurTargetingMat, false, false, null);
				}
				if (this.onUpdate != null)
				{
					this.onUpdate();
				}
			}
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x0020E78F File Offset: 0x0020C98F
		public bool IsTargetedNow(WorldObject o, List<WorldObject> worldObjectsUnderMouse = null)
		{
			if (!this.IsTargeting)
			{
				return false;
			}
			if (worldObjectsUnderMouse == null)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			return worldObjectsUnderMouse.Any<WorldObject>() && o == worldObjectsUnderMouse[0];
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x0020E7C0 File Offset: 0x0020C9C0
		private GlobalTargetInfo CurrentTargetUnderMouse()
		{
			if (!this.IsTargeting)
			{
				return GlobalTargetInfo.Invalid;
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			if (list.Any<WorldObject>())
			{
				return list[0];
			}
			if (!this.canTargetTiles)
			{
				return GlobalTargetInfo.Invalid;
			}
			int num = GenWorld.MouseTile(false);
			if (num >= 0)
			{
				return new GlobalTargetInfo(num);
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x040033AE RID: 13230
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x040033AF RID: 13231
		private bool canTargetTiles;

		// Token: 0x040033B0 RID: 13232
		private Texture2D mouseAttachment;

		// Token: 0x040033B1 RID: 13233
		public bool closeWorldTabWhenFinished;

		// Token: 0x040033B2 RID: 13234
		private Action onUpdate;

		// Token: 0x040033B3 RID: 13235
		private Func<GlobalTargetInfo, string> extraLabelGetter;

		// Token: 0x040033B4 RID: 13236
		private const float BaseFeedbackTexSize = 0.8f;
	}
}
