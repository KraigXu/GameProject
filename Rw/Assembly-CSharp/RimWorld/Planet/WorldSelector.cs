﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020012A0 RID: 4768
	public class WorldSelector
	{
		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06007076 RID: 28790 RVA: 0x001FC247 File Offset: 0x001FA447
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06007077 RID: 28791 RVA: 0x00273AFF File Offset: 0x00271CFF
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06007078 RID: 28792 RVA: 0x00273B07 File Offset: 0x00271D07
		public WorldObject SingleSelectedObject
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06007079 RID: 28793 RVA: 0x00273B25 File Offset: 0x00271D25
		public WorldObject FirstSelectedObject
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x0600707A RID: 28794 RVA: 0x00273B42 File Offset: 0x00271D42
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x0600707B RID: 28795 RVA: 0x00273B4F File Offset: 0x00271D4F
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile >= 0;
			}
		}

		// Token: 0x0600707C RID: 28796 RVA: 0x00273B67 File Offset: 0x00271D67
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		// Token: 0x0600707D RID: 28797 RVA: 0x00273B9C File Offset: 0x00271D9C
		private void HandleWorldClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MousePositionOnUIInverted;
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Caravan)
					{
						Caravan caravan = (Caravan)this.selected[0];
						if (caravan.IsPlayerControlled && !FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
						{
							this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
						}
					}
					else
					{
						for (int i = 0; i < this.selected.Count; i++)
						{
							Caravan caravan2 = this.selected[i] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled)
							{
								this.AutoOrderToTile(caravan2, GenWorld.MouseTile(false));
							}
						}
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0 && this.dragBox.active)
				{
					this.dragBox.active = false;
					if (!this.dragBox.IsValid)
					{
						this.SelectUnderMouse(true);
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x00273D1F File Offset: 0x00271F1F
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x00273D2D File Offset: 0x00271F2D
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = -1;
		}

		// Token: 0x06007080 RID: 28800 RVA: 0x00273D46 File Offset: 0x00271F46
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x06007081 RID: 28801 RVA: 0x00273D64 File Offset: 0x00271F64
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
				return;
			}
			this.selectedTile = -1;
			if (this.selected.Count >= 80)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				WorldSelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x06007082 RID: 28802 RVA: 0x00273DBC File Offset: 0x00271FBC
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x00273DCA File Offset: 0x00271FCA
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x06007084 RID: 28804 RVA: 0x00273DD8 File Offset: 0x00271FD8
		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool flag = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Caravan> list = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int i = 0; i < list.Count; i++)
				{
					flag = true;
					this.Select(list[i], true);
				}
			}
			if (!flag && Current.ProgramState == ProgramState.Playing)
			{
				List<Thing> list2 = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!flag)
					{
						CameraJumper.TryJumpAndSelect(list2[j]);
						flag = true;
					}
					else
					{
						Find.Selector.Select(list2[j], true, true);
					}
				}
			}
			if (!flag)
			{
				List<WorldObject> list3 = WorldObjectSelectionUtility.MultiSelectableWorldObjectsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<WorldObject>();
				if (list3.Any((WorldObject x) => x is Caravan))
				{
					list3.RemoveAll((WorldObject x) => !(x is Caravan));
					if (list3.Any((WorldObject x) => x.Faction == Faction.OfPlayer))
					{
						list3.RemoveAll((WorldObject x) => x.Faction != Faction.OfPlayer);
					}
				}
				for (int k = 0; k < list3.Count; k++)
				{
					flag = true;
					this.Select(list3[k], true);
				}
			}
			if (!flag)
			{
				bool canSelectTile = this.dragBox.Diagonal < 30f;
				this.SelectUnderMouse(canSelectTile);
			}
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x00273F9C File Offset: 0x0027219C
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			bool flag2;
			return this.SelectableObjectsUnderMouse(out flag, out flag2);
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x00273FB4 File Offset: 0x002721B4
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse(out bool clickedDirectlyOnCaravan, out bool usedColonistBar)
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(mousePositionOnUIInverted);
				if (caravan != null)
				{
					clickedDirectlyOnCaravan = true;
					usedColonistBar = true;
					return Gen.YieldSingle<WorldObject>(caravan);
				}
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			clickedDirectlyOnCaravan = false;
			if (list.Count > 0 && list[0] is Caravan && list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
			{
				clickedDirectlyOnCaravan = true;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					WorldObject worldObject = list[i];
					if (worldObject is Caravan && worldObject.DistanceToMouse(UI.MousePositionOnUI) > GenWorldUI.CaravanDirectClickRadius)
					{
						list.Remove(worldObject);
					}
				}
			}
			usedColonistBar = false;
			return list;
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x00274070 File Offset: 0x00272270
		public static IEnumerable<WorldObject> SelectableObjectsAt(int tileID)
		{
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID))
			{
				if (worldObject.SelectableNow)
				{
					yield return worldObject;
				}
			}
			IEnumerator<WorldObject> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x00274080 File Offset: 0x00272280
		private void SelectUnderMouse(bool canSelectTile = true)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				Pawn pawn = thing as Pawn;
				if (thing != null && (pawn == null || !pawn.IsCaravanMember()))
				{
					if (thing.Spawned)
					{
						CameraJumper.TryJumpAndSelect(thing);
						return;
					}
					CameraJumper.TryJump(thing);
					return;
				}
			}
			bool flag;
			bool flag2;
			List<WorldObject> list = this.SelectableObjectsUnderMouse(out flag, out flag2).ToList<WorldObject>();
			if (flag2 || (flag && list.Count >= 2))
			{
				canSelectTile = false;
			}
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
						return;
					}
				}
			}
			else
			{
				if ((from obj in list
				where this.selected.Contains(obj)
				select obj).FirstOrDefault<WorldObject>() != null)
				{
					if (!this.ShiftIsHeld)
					{
						int tile = canSelectTile ? GenWorld.MouseTile(false) : -1;
						this.SelectFirstOrNextFrom(list, tile);
						return;
					}
					using (List<WorldObject>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WorldObject worldObject = enumerator.Current;
							if (this.selected.Contains(worldObject))
							{
								this.Deselect(worldObject);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true);
			}
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x002741D4 File Offset: 0x002723D4
		public void SelectFirstOrNextAt(int tileID)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tileID).ToList<WorldObject>(), tileID);
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x002741E8 File Offset: 0x002723E8
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count == 0)
			{
				return;
			}
			Type type = list[0].GetType();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (!(type != allWorldObjects[i].GetType()) && (allWorldObjects[i] == list[0] || allWorldObjects[i].AllMatchingObjectsOnScreenMatchesWith(list[0])) && allWorldObjects[i].VisibleToCameraNow())
				{
					this.Select(allWorldObjects[i], true);
				}
			}
		}

		// Token: 0x0600708B RID: 28811 RVA: 0x00274288 File Offset: 0x00272488
		private void AutoOrderToTile(Caravan c, int tile)
		{
			if (tile < 0)
			{
				return;
			}
			if (c.autoJoinable && CaravanExitMapUtility.AnyoneTryingToJoinCaravan(c))
			{
				CaravanExitMapUtility.OpenSomeoneTryingToJoinCaravanDialog(c, delegate
				{
					this.AutoOrderToTileNow(c, tile);
				});
				return;
			}
			this.AutoOrderToTileNow(c, tile);
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x00274300 File Offset: 0x00272500
		private void AutoOrderToTileNow(Caravan c, int tile)
		{
			if (tile < 0 || (tile == c.Tile && !c.pather.Moving))
			{
				return;
			}
			int num = CaravanUtility.BestGotoDestNear(tile, c);
			if (num >= 0)
			{
				c.pather.StartPath(num, null, true, true);
				c.gotoMote.OrderedToTile(num);
				SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x0027435C File Offset: 0x0027255C
		private void SelectFirstOrNextFrom(List<WorldObject> objects, int tile)
		{
			int num = objects.FindIndex((WorldObject x) => this.selected.Contains(x));
			int num2 = -1;
			int num3 = -1;
			if (num != -1)
			{
				if (num == objects.Count - 1 || this.selected.Count >= 2)
				{
					if (this.selected.Count >= 2)
					{
						num3 = 0;
					}
					else if (tile >= 0)
					{
						num2 = tile;
					}
					else
					{
						num3 = 0;
					}
				}
				else
				{
					num3 = num + 1;
				}
			}
			else if (objects.Count == 0)
			{
				num2 = tile;
			}
			else
			{
				num3 = 0;
			}
			this.ClearSelection();
			if (num3 >= 0)
			{
				this.Select(objects[num3], true);
			}
			this.selectedTile = num2;
		}

		// Token: 0x04004523 RID: 17699
		public WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x04004524 RID: 17700
		private List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x04004525 RID: 17701
		public int selectedTile = -1;

		// Token: 0x04004526 RID: 17702
		private const int MaxNumSelected = 80;

		// Token: 0x04004527 RID: 17703
		private const float MaxDragBoxDiagonalToSelectTile = 30f;
	}
}
