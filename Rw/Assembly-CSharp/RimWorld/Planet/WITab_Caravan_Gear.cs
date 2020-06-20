using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02001292 RID: 4754
	public class WITab_Caravan_Gear : WITab
	{
		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06006FDA RID: 28634 RVA: 0x0026F60A File Offset: 0x0026D80A
		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x0026F617 File Offset: 0x0026D817
		public WITab_Caravan_Gear()
		{
			this.labelKey = "TabCaravanGear";
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x0026F62C File Offset: 0x0026D82C
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.leftPaneWidth = 469f;
			this.rightPaneWidth = 345f;
			this.size.x = this.leftPaneWidth + this.rightPaneWidth;
			this.size.y = Mathf.Min(550f, this.PaneTopY - 30f);
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x0026F68E File Offset: 0x0026D88E
		public override void OnOpen()
		{
			base.OnOpen();
			this.draggedItem = null;
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x0026F6A0 File Offset: 0x0026D8A0
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			this.CheckDraggedItemStillValid();
			this.CheckDropDraggedItem();
			Rect position = new Rect(0f, 0f, this.leftPaneWidth, this.size.y);
			GUI.BeginGroup(position);
			this.DoLeftPane();
			GUI.EndGroup();
			GUI.BeginGroup(new Rect(position.xMax, 0f, this.rightPaneWidth, this.size.y));
			this.DoRightPane();
			GUI.EndGroup();
			if (this.draggedItem != null && this.droppedDraggedItem)
			{
				this.droppedDraggedItem = false;
				this.draggedItem = null;
			}
		}

		// Token: 0x06006FDF RID: 28639 RVA: 0x0026F744 File Offset: 0x0026D944
		private void DoLeftPane()
		{
			Rect rect = new Rect(0f, 0f, this.leftPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.leftPaneScrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.leftPaneScrollPosition, rect2, true);
			this.DoPawnRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.leftPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x0026F7E0 File Offset: 0x0026D9E0
		private void DoRightPane()
		{
			Rect rect = new Rect(0f, 0f, this.rightPaneWidth, this.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, this.rightPaneScrollViewHeight);
			if (this.draggedItem != null && rect.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != null)
			{
				Widgets.DrawHighlight(rect);
				if (this.droppedDraggedItem)
				{
					this.MoveDraggedItemToInventory();
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
			}
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.rightPaneScrollPosition, rect2, true);
			this.DoInventoryRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.rightPaneScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06006FE1 RID: 28641 RVA: 0x0026F8C8 File Offset: 0x0026DAC8
		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (this.draggedItem != null)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Rect rect = new Rect(mousePosition.x - this.draggedItemPosOffset.x, mousePosition.y - this.draggedItemPosOffset.y, 32f, 32f);
				Find.WindowStack.ImmediateWindow(1283641090, rect, WindowLayer.Super, delegate
				{
					if (this.draggedItem == null)
					{
						return;
					}
					Widgets.ThingIcon(rect.AtZero(), this.draggedItem, 1f);
				}, false, false, 0f);
			}
			this.CheckDropDraggedItem();
		}

		// Token: 0x06006FE2 RID: 28642 RVA: 0x0026F964 File Offset: 0x0026DB64
		private void DoPawnRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(135f, curY + 6f, 200f, 100f), "DragToRearrange".Translate());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (pawn.IsColonist)
				{
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn);
				}
			}
			bool flag = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn2 = pawns[j];
				if (pawn2.IsPrisoner)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisoners".Translate());
						flag = true;
					}
					this.DoPawnRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
		}

		// Token: 0x06006FE3 RID: 28643 RVA: 0x0026FA60 File Offset: 0x0026DC60
		private void DoPawnRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = this.leftPaneScrollPosition.y - 50f;
			float num2 = this.leftPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoPawnRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		// Token: 0x06006FE4 RID: 28644 RVA: 0x0026FAC8 File Offset: 0x0026DCC8
		private void DoPawnRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanThingsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, p);
			rect2.width -= 24f;
			bool flag = this.draggedItem != null && rect2.Contains(Event.current.mousePosition) && this.CurrentWearerOf(this.draggedItem) != p;
			if ((Mouse.IsOver(rect2) && this.draggedItem == null) || flag)
			{
				Widgets.DrawHighlight(rect2);
			}
			if (flag && this.droppedDraggedItem)
			{
				this.TryEquipDraggedItem(p);
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect(rect3.xMax + 4f, 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			float xMax = bgRect.xMax;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					this.DoEquippedGear(allEquipmentListForReading[i], p, ref xMax);
				}
			}
			if (p.apparel != null)
			{
				WITab_Caravan_Gear.tmpApparel.Clear();
				WITab_Caravan_Gear.tmpApparel.AddRange(p.apparel.WornApparel);
				WITab_Caravan_Gear.tmpApparel.SortBy((Apparel x) => x.def.apparel.LastLayer.drawOrder, (Apparel x) => -x.def.apparel.HumanBodyCoverage);
				for (int j = 0; j < WITab_Caravan_Gear.tmpApparel.Count; j++)
				{
					this.DoEquippedGear(WITab_Caravan_Gear.tmpApparel[j], p, ref xMax);
				}
			}
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, rect.height / 2f, rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}

		// Token: 0x06006FE5 RID: 28645 RVA: 0x0026FD50 File Offset: 0x0026DF50
		private void DoInventoryRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(base.SelCaravan);
			Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanWeaponsAndApparel".Translate());
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (this.IsVisibleWeapon(thing.def))
				{
					if (!flag)
					{
						flag = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				Thing thing2 = list[j];
				if (thing2.def.IsApparel)
				{
					if (!flag2)
					{
						flag2 = true;
					}
					this.DoInventoryRow(ref curY, scrollViewRect, scrollOutRect, thing2);
				}
			}
			if (!flag && !flag2)
			{
				Widgets.NoneLabel(ref curY, scrollViewRect.width, null);
			}
		}

		// Token: 0x06006FE6 RID: 28646 RVA: 0x0026FE14 File Offset: 0x0026E014
		private void DoInventoryRow(ref float curY, Rect viewRect, Rect scrollOutRect, Thing t)
		{
			float num = this.rightPaneScrollPosition.y - 30f;
			float num2 = this.rightPaneScrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoInventoryRow(new Rect(0f, curY, viewRect.width, 30f), t);
			}
			curY += 30f;
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x0026FE7C File Offset: 0x0026E07C
		private void DoInventoryRow(Rect rect, Thing t)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			Widgets.InfoCardButton(rect2.width - 24f, (rect.height - 24f) / 2f, t);
			rect2.width -= 24f;
			if (this.draggedItem == null && Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			float num = (t == this.draggedItem) ? 0.5f : 1f;
			Rect rect3 = new Rect(4f, (rect.height - 27f) / 2f, 27f, 27f);
			Widgets.ThingIcon(rect3, t, num);
			GUI.color = new Color(1f, 1f, 1f, num);
			Rect rect4 = new Rect(rect3.xMax + 4f, 0f, 250f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect4, t.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect2))
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = new Vector2(16f, 16f);
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			GUI.EndGroup();
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x0026FFF4 File Offset: 0x0026E1F4
		private void DoEquippedGear(Thing t, Pawn p, ref float curX)
		{
			Rect rect = new Rect(curX, 9f, 32f, 32f);
			bool flag = Mouse.IsOver(rect);
			float alpha;
			if (t == this.draggedItem)
			{
				alpha = 0.2f;
			}
			else if (flag && this.draggedItem == null)
			{
				alpha = 0.75f;
			}
			else
			{
				alpha = 1f;
			}
			Widgets.ThingIcon(rect, t, alpha);
			curX += 32f;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, t.LabelCap);
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && flag)
			{
				this.draggedItem = t;
				this.droppedDraggedItem = false;
				this.draggedItemPosOffset = Event.current.mousePosition - rect.position;
				Event.current.Use();
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06006FE9 RID: 28649 RVA: 0x002700D4 File Offset: 0x0026E2D4
		private void CheckDraggedItemStillValid()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (this.draggedItem.Destroyed)
			{
				this.draggedItem = null;
				return;
			}
			if (this.CurrentWearerOf(this.draggedItem) != null)
			{
				return;
			}
			if (CaravanInventoryUtility.AllInventoryItems(base.SelCaravan).Contains(this.draggedItem))
			{
				return;
			}
			this.draggedItem = null;
		}

		// Token: 0x06006FEA RID: 28650 RVA: 0x0027012E File Offset: 0x0026E32E
		private void CheckDropDraggedItem()
		{
			if (this.draggedItem == null)
			{
				return;
			}
			if (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseUp)
			{
				this.droppedDraggedItem = true;
			}
		}

		// Token: 0x06006FEB RID: 28651 RVA: 0x0027015A File Offset: 0x0026E35A
		private bool IsVisibleWeapon(ThingDef t)
		{
			return t.IsWeapon && t != ThingDefOf.WoodLog && t != ThingDefOf.Beer;
		}

		// Token: 0x06006FEC RID: 28652 RVA: 0x0027017C File Offset: 0x0026E37C
		private Pawn CurrentWearerOf(Thing t)
		{
			IThingHolder parentHolder = t.ParentHolder;
			if (parentHolder is Pawn_EquipmentTracker || parentHolder is Pawn_ApparelTracker)
			{
				return (Pawn)parentHolder.ParentHolder;
			}
			return null;
		}

		// Token: 0x06006FED RID: 28653 RVA: 0x002701B0 File Offset: 0x0026E3B0
		private void MoveDraggedItemToInventory()
		{
			this.droppedDraggedItem = false;
			Apparel apparel;
			if ((apparel = (this.draggedItem as Apparel)) != null && this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
			{
				Messages.Message("MessageCantUnequipLockedApparel".Translate(), this.CurrentWearerOf(apparel), MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(this.draggedItem, this.Pawns, null, null);
			if (pawn != null)
			{
				this.draggedItem.holdingOwner.TryTransferToContainer(this.draggedItem, pawn.inventory.innerContainer, 1, true);
			}
			else
			{
				Log.Warning("Could not find any pawn to move " + this.draggedItem + " to.", false);
			}
			this.draggedItem = null;
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x00270280 File Offset: 0x0026E480
		private void TryEquipDraggedItem(Pawn p)
		{
			this.droppedDraggedItem = false;
			string str;
			if (!EquipmentUtility.CanEquip(this.draggedItem, p, out str))
			{
				Messages.Message("MessageCantEquipCustom".Translate(str.CapitalizeFirst()), p, MessageTypeDefOf.RejectInput, false);
				this.draggedItem = null;
				return;
			}
			if (this.draggedItem.def.IsWeapon)
			{
				if (p.WorkTagIsDisabled(WorkTags.Violent))
				{
					Messages.Message("MessageCantEquipIncapableOfViolence".Translate(p.LabelShort, p), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					Messages.Message("MessageCantEquipIncapableOfManipulation".Translate(), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
			}
			Apparel apparel = this.draggedItem as Apparel;
			ThingWithComps thingWithComps = this.draggedItem as ThingWithComps;
			if (apparel != null && p.apparel != null)
			{
				if (!ApparelUtility.HasPartsToWear(p, apparel.def))
				{
					Messages.Message("MessageCantWearApparelMissingBodyParts".Translate(p.LabelShort, p), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (this.CurrentWearerOf(apparel) != null && this.CurrentWearerOf(apparel).apparel.IsLocked(apparel))
				{
					Messages.Message("MessageCantUnequipLockedApparel".Translate(), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				if (p.apparel.WouldReplaceLockedApparel(apparel))
				{
					Messages.Message("MessageWouldReplaceLockedApparel".Translate(p.LabelShort, p), p, MessageTypeDefOf.RejectInput, false);
					this.draggedItem = null;
					return;
				}
				WITab_Caravan_Gear.tmpExistingApparel.Clear();
				WITab_Caravan_Gear.tmpExistingApparel.AddRange(p.apparel.WornApparel);
				for (int i = 0; i < WITab_Caravan_Gear.tmpExistingApparel.Count; i++)
				{
					if (!ApparelUtility.CanWearTogether(apparel.def, WITab_Caravan_Gear.tmpExistingApparel[i].def, p.RaceProps.body))
					{
						p.apparel.Remove(WITab_Caravan_Gear.tmpExistingApparel[i]);
						Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingApparel[i], this.Pawns, null, null);
						if (pawn != null)
						{
							pawn.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingApparel[i], true);
						}
						else
						{
							Log.Warning("Could not find any pawn to move " + WITab_Caravan_Gear.tmpExistingApparel[i] + " to.", false);
							WITab_Caravan_Gear.tmpExistingApparel[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				p.apparel.Wear((Apparel)apparel.SplitOff(1), false, false);
				if (p.outfits != null)
				{
					p.outfits.forcedHandler.SetForced(apparel, true);
				}
			}
			else if (thingWithComps != null && p.equipment != null)
			{
				WITab_Caravan_Gear.tmpExistingEquipment.Clear();
				WITab_Caravan_Gear.tmpExistingEquipment.AddRange(p.equipment.AllEquipmentListForReading);
				for (int j = 0; j < WITab_Caravan_Gear.tmpExistingEquipment.Count; j++)
				{
					p.equipment.Remove(WITab_Caravan_Gear.tmpExistingEquipment[j]);
					Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(WITab_Caravan_Gear.tmpExistingEquipment[j], this.Pawns, null, null);
					if (pawn2 != null)
					{
						pawn2.inventory.innerContainer.TryAdd(WITab_Caravan_Gear.tmpExistingEquipment[j], true);
					}
					else
					{
						Log.Warning("Could not find any pawn to move " + WITab_Caravan_Gear.tmpExistingEquipment[j] + " to.", false);
						WITab_Caravan_Gear.tmpExistingEquipment[j].Destroy(DestroyMode.Vanish);
					}
				}
				p.equipment.AddEquipment((ThingWithComps)thingWithComps.SplitOff(1));
			}
			else
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not make ",
					p,
					" equip or wear ",
					this.draggedItem
				}), false);
			}
			this.draggedItem = null;
		}

		// Token: 0x040044C9 RID: 17609
		private Vector2 leftPaneScrollPosition;

		// Token: 0x040044CA RID: 17610
		private float leftPaneScrollViewHeight;

		// Token: 0x040044CB RID: 17611
		private Vector2 rightPaneScrollPosition;

		// Token: 0x040044CC RID: 17612
		private float rightPaneScrollViewHeight;

		// Token: 0x040044CD RID: 17613
		private Thing draggedItem;

		// Token: 0x040044CE RID: 17614
		private Vector2 draggedItemPosOffset;

		// Token: 0x040044CF RID: 17615
		private bool droppedDraggedItem;

		// Token: 0x040044D0 RID: 17616
		private float leftPaneWidth;

		// Token: 0x040044D1 RID: 17617
		private float rightPaneWidth;

		// Token: 0x040044D2 RID: 17618
		private const float PawnRowHeight = 50f;

		// Token: 0x040044D3 RID: 17619
		private const float ItemRowHeight = 30f;

		// Token: 0x040044D4 RID: 17620
		private const float PawnLabelHeight = 18f;

		// Token: 0x040044D5 RID: 17621
		private const float PawnLabelColumnWidth = 100f;

		// Token: 0x040044D6 RID: 17622
		private const float GearLabelColumnWidth = 250f;

		// Token: 0x040044D7 RID: 17623
		private const float SpaceAroundIcon = 4f;

		// Token: 0x040044D8 RID: 17624
		private const float EquippedGearColumnWidth = 250f;

		// Token: 0x040044D9 RID: 17625
		private const float EquippedGearIconSize = 32f;

		// Token: 0x040044DA RID: 17626
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x040044DB RID: 17627
		private static List<ThingWithComps> tmpExistingEquipment = new List<ThingWithComps>();

		// Token: 0x040044DC RID: 17628
		private static List<Apparel> tmpExistingApparel = new List<Apparel>();
	}
}
