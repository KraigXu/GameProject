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
	
	public class Selector
	{
		
		
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		
		
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		
		
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		
		
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

		
		
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		
		
		
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

		
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
		}

		
		public void Deselect(object obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		
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

		
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		
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
				IEnumerable<Thing> boxThings = default;

				Func<Thing, bool> predicate2;
				if ((predicate2=default ) == null)
				{
					predicate2 = ( ((Thing t) => predicate(t)));
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
					List<object>.Enumerator enumerator = list.GetEnumerator();
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

		
		[Obsolete("Obsolete, only used to avoid error when patching")]
		private static void MassTakeFirstAutoTakeableOption(Pawn pawn, IntVec3 dest)
		{
			string text;
			Selector.MassTakeFirstAutoTakeableOption_NewTemp(pawn, dest, out text);
		}

		
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

		
		public DragBox dragBox = new DragBox();

		
		private List<object> selected = new List<object>();

		
		private static List<string> cantTakeReasons = new List<string>();

		
		private const float PawnSelectRadius = 1f;

		
		private const int MaxNumSelected = 200;
	}
}
