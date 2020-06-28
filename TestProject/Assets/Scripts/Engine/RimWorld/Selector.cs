using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EB8 RID: 3768
	public class Selector
	{
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x001FC247 File Offset: 0x001FA447
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06005BF5 RID: 23541 RVA: 0x001FC261 File Offset: 0x001FA461
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x001FC261 File Offset: 0x001FA461
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x001FC269 File Offset: 0x001FA469
		public Thing SingleSelectedThing
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				if (this.selected[0] is Thing)
				{
					return (Thing)this.selected[0];
				}
				return null;
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x001FC2A1 File Offset: 0x001FA4A1
		public object FirstSelectedObject
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

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x001FC2BE File Offset: 0x001FA4BE
		public object SingleSelectedObject
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

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06005BFA RID: 23546 RVA: 0x001FC2DC File Offset: 0x001FA4DC
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06005BFB RID: 23547 RVA: 0x001FC2E9 File Offset: 0x001FA4E9
		// (set) Token: 0x06005BFC RID: 23548 RVA: 0x001FC30B File Offset: 0x001FA50B
		public Zone SelectedZone
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Zone;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x001FC320 File Offset: 0x001FA520
		public void SelectorOnGUI()
		{
			this.HandleMapClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
			if (this.NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldRenderedNow)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, false);
			}
		}

		// Token: 0x06005BFE RID: 23550 RVA: 0x001FC38C File Offset: 0x001FA58C
		private void HandleMapClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MouseMapPosition();
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Pawn)
					{
						FloatMenuMakerMap.TryMakeFloatMenu((Pawn)this.selected[0]);
					}
					else
					{
						Selector.cantTakeReasons.Clear();
						for (int i = 0; i < this.selected.Count; i++)
						{
							Pawn pawn = this.selected[i] as Pawn;
							if (pawn != null)
							{
								string text;
								Selector.MassTakeFirstAutoTakeableOption_NewTemp(pawn, UI.MouseCell(), out text);
								if (text != null)
								{
									Selector.cantTakeReasons.Add(text);
								}
							}
						}
						if (Selector.cantTakeReasons.Count == this.selected.Count)
						{
							FloatMenu window = new FloatMenu((from r in Selector.cantTakeReasons.Distinct<string>()
							select new FloatMenuOption(r, null, MenuOptionPriority.Default, null, null, 0f, null, null)).ToList<FloatMenuOption>());
							Find.WindowStack.Add(window);
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
						this.SelectUnderMouse();
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x001FC563 File Offset: 0x001FA763
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x06005C00 RID: 23552 RVA: 0x001FC571 File Offset: 0x001FA771
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x001FC583 File Offset: 0x001FA783
		public void Deselect(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x001FC5A0 File Offset: 0x001FA7A0
		public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
				return;
			}
			Thing thing = obj as Thing;
			if (thing == null && !(obj is Zone))
			{
				Log.Error("Tried to select " + obj + " which is neither a Thing nor a Zone.", false);
				return;
			}
			if (thing != null && thing.Destroyed)
			{
				Log.Error("Cannot select destroyed thing.", false);
				return;
			}
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.IsWorldPawn())
			{
				Log.Error("Cannot select world pawns.", false);
				return;
			}
			if (forceDesignatorDeselect)
			{
				Find.DesignatorManager.Deselect();
			}
			if (this.SelectedZone != null && !(obj is Zone))
			{
				this.ClearSelection();
			}
			if (obj is Zone && this.SelectedZone == null)
			{
				this.ClearSelection();
			}
			Map map = (thing != null) ? thing.Map : ((Zone)obj).Map;
			for (int i = this.selected.Count - 1; i >= 0; i--)
			{
				Thing thing2 = this.selected[i] as Thing;
				if (((thing2 != null) ? thing2.Map : ((Zone)this.selected[i]).Map) != map)
				{
					this.Deselect(this.selected[i]);
				}
			}
			if (this.selected.Count >= 200)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = map;
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					IntVec3 cell = (thing != null) ? thing.Position : ((Zone)obj).Cells[0];
					Find.CameraDriver.JumpToCurrentMapLoc(cell);
				}
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				SelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x001FC74D File Offset: 0x001FA94D
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x001FC75C File Offset: 0x001FA95C
		private void PlaySelectionSoundFor(object obj)
		{
			if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
			{
				SoundDefOf.ColonistSelected.PlayOneShotOnCamera(null);
				return;
			}
			if (obj is Thing || obj is Zone)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
				return;
			}
			Log.Warning("Can't determine selection sound for " + obj, false);
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x001FC7D0 File Offset: 0x001FA9D0
		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool selectedSomething = false;
			List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
			for (int i = 0; i < list.Count; i++)
			{
				selectedSomething = true;
				this.Select(list[i], true, true);
			}
			if (selectedSomething)
			{
				return;
			}
			List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
			for (int j = 0; j < list2.Count; j++)
			{
				if (!selectedSomething)
				{
					CameraJumper.TryJumpAndSelect(list2[j]);
					selectedSomething = true;
				}
				else
				{
					Find.WorldSelector.Select(list2[j], true);
				}
			}
			if (selectedSomething)
			{
				return;
			}
			List<Thing> boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Thing>();
			Func<Predicate<Thing>, bool> func = delegate(Predicate<Thing> predicate)
			{
				IEnumerable<Thing> boxThings = boxThings;
				Func<Thing, bool> <>9__6;
				Func<Thing, bool> predicate2;
				if ((predicate2 = <>9__6) == null)
				{
					predicate2 = (<>9__6 = ((Thing t) => predicate(t)));
				}
				foreach (Thing obj2 in boxThings.Where(predicate2))
				{
					this.Select(obj2, true, true);
					selectedSomething = true;
				}
				return selectedSomething;
			};
			Predicate<Thing> arg = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike && t.Faction == Faction.OfPlayer;
			if (func(arg))
			{
				return;
			}
			Predicate<Thing> arg2 = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
			if (func(arg2))
			{
				return;
			}
			Predicate<Thing> arg3 = (Thing t) => t.def.CountAsResource;
			if (func(arg3))
			{
				return;
			}
			Predicate<Thing> arg4 = (Thing t) => t.def.category == ThingCategory.Pawn;
			if (func(arg4))
			{
				return;
			}
			if (func((Thing t) => t.def.selectable))
			{
				return;
			}
			foreach (Zone obj in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Zone>())
			{
				selectedSomething = true;
				this.Select(obj, true, true);
			}
			if (selectedSomething)
			{
				return;
			}
			this.SelectUnderMouse();
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x001FCA24 File Offset: 0x001FAC24
		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			Thing thing = Find.ColonistBar.ColonistOrCorpseAt(mousePositionOnUIInverted);
			if (thing != null && thing.Spawned)
			{
				yield return thing;
				yield break;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap))
			{
				yield break;
			}
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.mustBeSelectable = true;
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			List<Thing> selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, targetingParameters);
			if (selectableList.Count > 0 && selectableList[0] is Pawn && (selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
			{
				for (int j = selectableList.Count - 1; j >= 0; j--)
				{
					Thing thing2 = selectableList[j];
					if (thing2.def.category == ThingCategory.Pawn && (thing2.DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
					{
						selectableList.Remove(thing2);
					}
				}
			}
			int num;
			for (int i = 0; i < selectableList.Count; i = num + 1)
			{
				yield return selectableList[i];
				num = i;
			}
			Zone zone = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
			if (zone != null)
			{
				yield return zone;
			}
			yield break;
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x001FCA2D File Offset: 0x001FAC2D
		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				Thing thing = thingList[i];
				if (ThingSelectionUtility.SelectableByMapClick(thing))
				{
					yield return thing;
				}
				num = i;
			}
			Zone zone = map.zoneManager.ZoneAt(c);
			if (zone != null)
			{
				yield return zone;
			}
			yield break;
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x001FCA44 File Offset: 0x001FAC44
		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect(caravan);
				return;
			}
			Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
			if (thing != null && !thing.Spawned)
			{
				CameraJumper.TryJump(thing);
				return;
			}
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					return;
				}
			}
			else if (list.Count == 1)
			{
				object obj4 = list[0];
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					this.Select(obj4, true, true);
					return;
				}
				if (!this.selected.Contains(obj4))
				{
					this.Select(obj4, true, true);
					return;
				}
				this.Deselect(obj4);
				return;
			}
			else if (list.Count > 1)
			{
				object obj2 = (from obj in list
				where this.selected.Contains(obj)
				select obj).FirstOrDefault<object>();
				if (obj2 != null)
				{
					if (!this.ShiftIsHeld)
					{
						int num = list.IndexOf(obj2) + 1;
						if (num >= list.Count)
						{
							num -= list.Count;
						}
						this.ClearSelection();
						this.Select(list[num], true, true);
						return;
					}
					using (List<object>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							if (this.selected.Contains(obj3))
							{
								this.Deselect(obj3);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true, true);
			}
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x001FCBE4 File Offset: 0x001FADE4
		public void SelectNextAt(IntVec3 c, Map map)
		{
			if (this.SelectedObjects.Count<object>() != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.", false);
				return;
			}
			List<object> list = Selector.SelectableObjectsAt(c, map).ToList<object>();
			int num = list.IndexOf(this.SingleSelectedThing) + 1;
			if (num >= list.Count)
			{
				num -= list.Count;
			}
			this.ClearSelection();
			this.Select(list[num], true, true);
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x001FCC50 File Offset: 0x001FAE50
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				return;
			}
			Thing clickedThing = list.FirstOrDefault((object o) => o is Pawn && ((Pawn)o).Faction == Faction.OfPlayer && !((Pawn)o).IsPrisoner) as Thing;
			clickedThing = (list.FirstOrDefault((object o) => o is Pawn) as Thing);
			if (clickedThing == null)
			{
				clickedThing = ((from o in list
				where o is Thing && !((Thing)o).def.neverMultiSelect
				select o).FirstOrDefault<object>() as Thing);
			}
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			if (clickedThing != null)
			{
				IEnumerable enumerable = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect);
				Predicate<Thing> predicate = delegate(Thing t)
				{
					if (t.def != clickedThing.GetInnerIfMinified().def || t.Faction != clickedThing.Faction || this.IsSelected(t))
					{
						return false;
					}
					Pawn pawn = clickedThing as Pawn;
					if (pawn != null)
					{
						Pawn pawn2 = t as Pawn;
						if (pawn2.RaceProps != pawn.RaceProps)
						{
							return false;
						}
						if (pawn2.HostFaction != pawn.HostFaction)
						{
							return false;
						}
					}
					return true;
				};
				foreach (object obj in enumerable)
				{
					Thing thing = (Thing)obj;
					if (predicate(thing.GetInnerIfMinified()))
					{
						this.Select(thing, true, true);
					}
				}
				return;
			}
			if (list.FirstOrDefault((object o) => o is Zone && ((Zone)o).IsMultiselectable) == null)
			{
				return;
			}
			foreach (Zone obj2 in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect))
			{
				if (!this.IsSelected(obj2))
				{
					this.Select(obj2, true, true);
				}
			}
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x001FCE2C File Offset: 0x001FB02C
		[Obsolete("Obsolete, only used to avoid error when patching")]
		private static void MassTakeFirstAutoTakeableOption(Pawn pawn, IntVec3 dest)
		{
			string text;
			Selector.MassTakeFirstAutoTakeableOption_NewTemp(pawn, dest, out text);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x001FCE44 File Offset: 0x001FB044
		private static void MassTakeFirstAutoTakeableOption_NewTemp(Pawn pawn, IntVec3 dest, out string cantTakeReason)
		{
			FloatMenuOption floatMenuOption = null;
			cantTakeReason = null;
			foreach (FloatMenuOption floatMenuOption2 in FloatMenuMakerMap.ChoicesAtFor(dest.ToVector3Shifted(), pawn))
			{
				if (floatMenuOption2.Disabled || !floatMenuOption2.autoTakeable)
				{
					cantTakeReason = floatMenuOption2.Label;
				}
				else if (floatMenuOption == null || floatMenuOption2.autoTakeablePriority > floatMenuOption.autoTakeablePriority)
				{
					floatMenuOption = floatMenuOption2;
				}
			}
			if (floatMenuOption != null)
			{
				floatMenuOption.Chosen(true, null);
			}
		}

		// Token: 0x0400323A RID: 12858
		public DragBox dragBox = new DragBox();

		// Token: 0x0400323B RID: 12859
		private List<object> selected = new List<object>();

		// Token: 0x0400323C RID: 12860
		private static List<string> cantTakeReasons = new List<string>();

		// Token: 0x0400323D RID: 12861
		private const float PawnSelectRadius = 1f;

		// Token: 0x0400323E RID: 12862
		private const int MaxNumSelected = 200;
	}
}
