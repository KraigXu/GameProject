using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class ITab_ContentsBase : ITab
	{
		
		
		public abstract IList<Thing> container { get; }

		
		
		public override bool IsVisible
		{
			get
			{
				return base.SelThing.Faction == Faction.OfPlayer;
			}
		}

		
		public ITab_ContentsBase()
		{
			this.size = new Vector2(460f, 450f);
		}

		
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Text.Font = GameFont.Small;
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		
		protected virtual void DoItemsLists(Rect inRect, ref float curY)
		{
			GUI.BeginGroup(inRect);
			Widgets.ListSeparator(ref curY, inRect.width, this.containedItemsKey.Translate());
			IList<Thing> container = this.container;
			bool flag = false;
			for (int i = 0; i < container.Count; i++)
			{
				Thing t = container[i];
				if (t != null)
				{
					flag = true;
					ITab_ContentsBase.tmpSingleThing.Clear();
					ITab_ContentsBase.tmpSingleThing.Add(t);
					this.DoThingRow(t.def, t.stackCount, ITab_ContentsBase.tmpSingleThing, inRect.width, ref curY, delegate(int x)
					{
						this.OnDropThing(t, x);
					});
					ITab_ContentsBase.tmpSingleThing.Clear();
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref curY, inRect.width, null);
			}
			GUI.EndGroup();
		}

		
		protected virtual void OnDropThing(Thing t, int count)
		{
			Thing thing;
			GenDrop.TryDropSpawn_NewTmp(t.SplitOff(count), base.SelThing.Position, base.SelThing.Map, ThingPlaceMode.Near, out thing, null, null, true);
		}

		
		protected void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY, Action<int> discardAction)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (this.canRemoveThings)
			{
				if (count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
				{
					Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(thingDef.label), 1, count, discardAction, int.MinValue));
				}
				rect.width -= 24f;
				if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true))
				{
					string value = thingDef.label;
					if (things.Count == 1 && things[0] is Pawn)
					{
						value = ((Pawn)things[0]).LabelShortCap;
					}
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate
					{
						discardAction(count);
					}, false, null));
				}
				rect.width -= 24f;
			}
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_ContentsBase.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef, null, 1f);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_ContentsBase.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1 && count == things[0].stackCount)
			{
				str = things[0].LabelCap;
			}
			else
			{
				str = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		
		private Vector2 scrollPosition;

		
		private float lastDrawnHeight;

		
		private List<Thing> thingsToSelect = new List<Thing>();

		
		public bool canRemoveThings = true;

		
		protected static List<Thing> tmpSingleThing = new List<Thing>();

		
		protected const float TopPadding = 20f;

		
		protected const float SpaceBetweenItemsLists = 10f;

		
		protected const float ThingRowHeight = 28f;

		
		protected const float ThingIconSize = 28f;

		
		protected const float ThingLeftX = 36f;

		
		protected static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		
		protected static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		
		public string containedItemsKey;
	}
}
