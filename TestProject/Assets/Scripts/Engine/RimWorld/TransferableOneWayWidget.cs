using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7D RID: 3709
	[StaticConstructorOnStartup]
	public class TransferableOneWayWidget
	{
		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x001E6FDC File Offset: 0x001E51DC
		public float TotalNumbersColumnsWidths
		{
			get
			{
				float num = 315f;
				if (this.drawMass)
				{
					num += 100f;
				}
				if (this.drawMarketValue)
				{
					num += 100f;
				}
				if (this.drawDaysUntilRot)
				{
					num += 75f;
				}
				if (this.drawItemNutrition)
				{
					num += 75f;
				}
				if (this.drawNutritionEatenPerDay)
				{
					num += 75f;
				}
				if (this.drawForagedFoodPerDay)
				{
					num += 75f;
				}
				return num;
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06005A2C RID: 23084 RVA: 0x001E7050 File Offset: 0x001E5250
		private bool AnyTransferable
		{
			get
			{
				if (!this.transferablesCached)
				{
					this.CacheTransferables();
				}
				for (int i = 0; i < this.sections.Count; i++)
				{
					if (this.sections[i].cachedTransferables.Any<TransferableOneWay>())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x001E709C File Offset: 0x001E529C
		public TransferableOneWayWidget(IEnumerable<TransferableOneWay> transferables, string sourceLabel, string destinationLabel, string sourceCountDesc, bool drawMass = false, IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore, bool includePawnsMassInMassUsage = false, Func<float> availableMassGetter = null, float extraHeaderSpace = 0f, bool ignoreSpawnedCorpseGearAndInventoryMass = false, int tile = -1, bool drawMarketValue = false, bool drawEquippedWeapon = false, bool drawNutritionEatenPerDay = false, bool drawItemNutrition = false, bool drawForagedFoodPerDay = false, bool drawDaysUntilRot = false, bool playerPawnsReadOnly = false)
		{
			if (transferables != null)
			{
				this.AddSection(null, transferables);
			}
			this.sourceLabel = sourceLabel;
			this.destinationLabel = destinationLabel;
			this.sourceCountDesc = sourceCountDesc;
			this.drawMass = drawMass;
			this.ignorePawnInventoryMass = ignorePawnInventoryMass;
			this.includePawnsMassInMassUsage = includePawnsMassInMassUsage;
			this.availableMassGetter = availableMassGetter;
			this.extraHeaderSpace = extraHeaderSpace;
			this.ignoreSpawnedCorpseGearAndInventoryMass = ignoreSpawnedCorpseGearAndInventoryMass;
			this.tile = tile;
			this.drawMarketValue = drawMarketValue;
			this.drawEquippedWeapon = drawEquippedWeapon;
			this.drawNutritionEatenPerDay = drawNutritionEatenPerDay;
			this.drawItemNutrition = drawItemNutrition;
			this.drawForagedFoodPerDay = drawForagedFoodPerDay;
			this.drawDaysUntilRot = drawDaysUntilRot;
			this.playerPawnsReadOnly = playerPawnsReadOnly;
			this.sorter1 = TransferableSorterDefOf.Category;
			this.sorter2 = TransferableSorterDefOf.MarketValue;
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x001E71A0 File Offset: 0x001E53A0
		public void AddSection(string title, IEnumerable<TransferableOneWay> transferables)
		{
			TransferableOneWayWidget.Section item = default(TransferableOneWayWidget.Section);
			item.title = title;
			item.transferables = transferables;
			item.cachedTransferables = new List<TransferableOneWay>();
			this.sections.Add(item);
			this.transferablesCached = false;
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001E71E4 File Offset: 0x001E53E4
		private void CacheTransferables()
		{
			this.transferablesCached = true;
			for (int i = 0; i < this.sections.Count; i++)
			{
				List<TransferableOneWay> cachedTransferables = this.sections[i].cachedTransferables;
				cachedTransferables.Clear();
				cachedTransferables.AddRange(this.sections[i].transferables.OrderBy((TransferableOneWay tr) => tr, this.sorter1.Comparer).ThenBy((TransferableOneWay tr) => tr, this.sorter2.Comparer).ThenBy((TransferableOneWay tr) => TransferableUIUtility.DefaultListOrderPriority(tr)).ToList<TransferableOneWay>());
			}
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x001E72C8 File Offset: 0x001E54C8
		public void OnGUI(Rect inRect)
		{
			bool flag;
			this.OnGUI(inRect, out flag);
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x001E72E0 File Offset: 0x001E54E0
		public void OnGUI(Rect inRect, out bool anythingChanged)
		{
			if (!this.transferablesCached)
			{
				this.CacheTransferables();
			}
			TransferableUIUtility.DoTransferableSorters(this.sorter1, this.sorter2, delegate(TransferableSorterDef x)
			{
				this.sorter1 = x;
				this.CacheTransferables();
			}, delegate(TransferableSorterDef x)
			{
				this.sorter2 = x;
				this.CacheTransferables();
			});
			if (!this.sourceLabel.NullOrEmpty() || !this.destinationLabel.NullOrEmpty())
			{
				float num = inRect.width - 515f;
				Rect position = new Rect(inRect.x + num, inRect.y, inRect.width - num, 37f);
				GUI.BeginGroup(position);
				Text.Font = GameFont.Medium;
				if (!this.sourceLabel.NullOrEmpty())
				{
					Rect rect = new Rect(0f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, this.sourceLabel);
				}
				if (!this.destinationLabel.NullOrEmpty())
				{
					Rect rect2 = new Rect(position.width / 2f, 0f, position.width / 2f, position.height);
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect2, this.destinationLabel);
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.EndGroup();
			}
			Rect mainRect = new Rect(inRect.x, inRect.y + 37f + this.extraHeaderSpace, inRect.width, inRect.height - 37f - this.extraHeaderSpace);
			this.FillMainRect(mainRect, out anythingChanged);
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x001E7460 File Offset: 0x001E5660
		private void FillMainRect(Rect mainRect, out bool anythingChanged)
		{
			anythingChanged = false;
			Text.Font = GameFont.Small;
			if (this.AnyTransferable)
			{
				float num = 6f;
				for (int i = 0; i < this.sections.Count; i++)
				{
					num += (float)this.sections[i].cachedTransferables.Count * 30f;
					if (this.sections[i].title != null)
					{
						num += 30f;
					}
				}
				float num2 = 6f;
				float availableMass = (this.availableMassGetter != null) ? this.availableMassGetter() : float.MaxValue;
				Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, num);
				Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
				float num3 = this.scrollPosition.y - 30f;
				float num4 = this.scrollPosition.y + mainRect.height;
				for (int j = 0; j < this.sections.Count; j++)
				{
					List<TransferableOneWay> cachedTransferables = this.sections[j].cachedTransferables;
					if (cachedTransferables.Any<TransferableOneWay>())
					{
						if (this.sections[j].title != null)
						{
							Widgets.ListSeparator(ref num2, viewRect.width, this.sections[j].title);
							num2 += 5f;
						}
						for (int k = 0; k < cachedTransferables.Count; k++)
						{
							if (num2 > num3 && num2 < num4)
							{
								Rect rect = new Rect(0f, num2, viewRect.width, 30f);
								int countToTransfer = cachedTransferables[k].CountToTransfer;
								this.DoRow(rect, cachedTransferables[k], k, availableMass);
								if (countToTransfer != cachedTransferables[k].CountToTransfer)
								{
									anythingChanged = true;
								}
							}
							num2 += 30f;
						}
					}
				}
				Widgets.EndScrollView();
				return;
			}
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(mainRect, "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06005A33 RID: 23091 RVA: 0x001E767C File Offset: 0x001E587C
		private void DoRow(Rect rect, TransferableOneWay trad, int index, float availableMass)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = rect.width;
			int maxCount = trad.MaxCount;
			Rect rect2 = new Rect(num - 240f, 0f, 240f, rect.height);
			TransferableOneWayWidget.stoppingPoints.Clear();
			if (this.availableMassGetter != null && (!(trad.AnyThing is Pawn) || this.includePawnsMassInMassUsage))
			{
				float num2 = availableMass + this.GetMass(trad.AnyThing) * (float)trad.CountToTransfer;
				int threshold = (num2 <= 0f) ? 0 : Mathf.FloorToInt(num2 / this.GetMass(trad.AnyThing));
				TransferableOneWayWidget.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(threshold, "M<", ">M"));
			}
			Pawn pawn = trad.AnyThing as Pawn;
			bool flag = pawn != null && (pawn.IsColonist || pawn.IsPrisonerOfColony);
			TransferableUIUtility.DoCountAdjustInterface(rect2, trad, index, 0, maxCount, false, TransferableOneWayWidget.stoppingPoints, this.playerPawnsReadOnly && flag);
			num -= 240f;
			if (this.drawMarketValue)
			{
				Rect rect3 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMarketValue(rect3, trad);
				num -= 100f;
			}
			if (this.drawMass)
			{
				Rect rect4 = new Rect(num - 100f, 0f, 100f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawMass(rect4, trad, availableMass);
				num -= 100f;
			}
			if (this.drawDaysUntilRot)
			{
				Rect rect5 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawDaysUntilRot(rect5, trad);
				num -= 75f;
			}
			if (this.drawItemNutrition)
			{
				Rect rect6 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawItemNutrition(rect6, trad);
				num -= 75f;
			}
			if (this.drawForagedFoodPerDay)
			{
				Rect rect7 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				if (!this.DrawGrazeability(rect7, trad))
				{
					this.DrawForagedFoodPerDay(rect7, trad);
				}
				num -= 75f;
			}
			if (this.drawNutritionEatenPerDay)
			{
				Rect rect8 = new Rect(num - 75f, 0f, 75f, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				this.DrawNutritionEatenPerDay(rect8, trad);
				num -= 75f;
			}
			if (this.ShouldShowCount(trad))
			{
				Rect rect9 = new Rect(num - 75f, 0f, 75f, rect.height);
				Widgets.DrawHighlightIfMouseover(rect9);
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect10 = rect9;
				rect10.xMin += 5f;
				rect10.xMax -= 5f;
				Widgets.Label(rect10, maxCount.ToStringCached());
				TooltipHandler.TipRegion(rect9, this.sourceCountDesc);
			}
			num -= 75f;
			if (this.drawEquippedWeapon)
			{
				Rect rect11 = new Rect(num - 30f, 0f, 30f, rect.height);
				Rect iconRect = new Rect(num - 30f, (rect.height - 30f) / 2f, 30f, 30f);
				this.DrawEquippedWeapon(rect11, iconRect, trad);
				num -= 30f;
			}
			TransferableUIUtility.DoExtraAnimalIcons(trad, rect, ref num);
			Rect idRect = new Rect(0f, 0f, num, rect.height);
			TransferableUIUtility.DrawTransferableInfo(trad, idRect, Color.white);
			GenUI.ResetLabelAlign();
			GUI.EndGroup();
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x001E7A2C File Offset: 0x001E5C2C
		private bool ShouldShowCount(TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return true;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			return pawn == null || !pawn.RaceProps.Humanlike || trad.MaxCount != 1;
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x001E7A70 File Offset: 0x001E5C70
		private void DrawDaysUntilRot(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			int num;
			if (!this.cachedTicksUntilRot.TryGetValue(trad, out num))
			{
				num = int.MaxValue;
				for (int i = 0; i < trad.things.Count; i++)
				{
					CompRottable compRottable = trad.things[i].TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						num = Mathf.Min(num, DaysUntilRotCalculator.ApproxTicksUntilRot_AssumeTimePassesBy(compRottable, this.tile, null));
					}
				}
				this.cachedTicksUntilRot.Add(trad, num);
			}
			if (num >= 36000000 || (float)num >= 3.6E+07f)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			float num2 = (float)num / 60000f;
			GUI.color = Color.yellow;
			Widgets.Label(rect, num2.ToString("0.#"));
			GUI.color = Color.white;
			TooltipHandler.TipRegionByKey(rect, "DaysUntilRotTip");
		}

		// Token: 0x06005A36 RID: 23094 RVA: 0x001E7B48 File Offset: 0x001E5D48
		private void DrawItemNutrition(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			if (!trad.ThingDef.IsNutritionGivingIngestible)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = Color.green;
			Widgets.Label(rect, trad.ThingDef.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "ItemNutritionTip".Translate((1.6f * ThingDefOf.Human.race.baseHungerRate).ToString("0.##")));
			}
		}

		// Token: 0x06005A37 RID: 23095 RVA: 0x001E7BF0 File Offset: 0x001E5DF0
		private bool DrawGrazeability(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return false;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || !VirtualPlantsUtility.CanEverEatVirtualPlants(pawn))
			{
				return false;
			}
			rect.width = 40f;
			Rect position = new Rect(rect.x + (float)((int)((rect.width - 28f) / 2f)), rect.y + (float)((int)((rect.height - 28f) / 2f)), 28f, 28f);
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.DrawTexture(position, TransferableOneWayWidget.CanGrazeIcon);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, delegate
				{
					TaggedString taggedString = "AnimalCanGrazeTip".Translate();
					if (this.tile != -1)
					{
						taggedString += "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(this.tile, Find.TickManager.TicksAbs);
					}
					return taggedString;
				}, trad.GetHashCode() ^ 1948571634);
			}
			return true;
		}

		// Token: 0x06005A38 RID: 23096 RVA: 0x001E7CAC File Offset: 0x001E5EAC
		private void DrawForagedFoodPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null)
			{
				return;
			}
			bool flag;
			float foragedNutritionPerDay = ForagedFoodPerDayCalculator.GetBaseForagedNutritionPerDay(p, out flag);
			if (flag)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			GUI.color = ((foragedNutritionPerDay == 0f) ? Color.gray : Color.green);
			Widgets.Label(rect, "+" + foragedNutritionPerDay.ToString("0.##"));
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => "NutritionForagedPerDayTip".Translate(StatDefOf.ForagedNutritionPerDay.Worker.GetExplanationFull(StatRequest.For(p), StatDefOf.ForagedNutritionPerDay.toStringNumberSense, foragedNutritionPerDay)), trad.GetHashCode() ^ 1958671422);
			}
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x001E7D70 File Offset: 0x001E5F70
		private void DrawNutritionEatenPerDay(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn p = trad.AnyThing as Pawn;
			if (p == null || !p.RaceProps.EatsFood || p.Dead || p.needs.food == null)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			string text = (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed, true) * 60000f).ToString("0.##");
			DietCategory resolvedDietCategory = p.RaceProps.ResolvedDietCategory;
			if (resolvedDietCategory != DietCategory.Omnivorous)
			{
				text = text + " (" + resolvedDietCategory.ToStringHumanShort() + ")";
			}
			GUI.color = new Color(1f, 0.5f, 0f);
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => RaceProperties.NutritionEatenPerDayExplanation_NewTemp(p, true, true, false), trad.GetHashCode() ^ 385968958);
			}
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x001E7E84 File Offset: 0x001E6084
		private void DrawMarketValue(Rect rect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.Label(rect, trad.AnyThing.MarketValue.ToStringMoney(null));
			TooltipHandler.TipRegionByKey(rect, "MarketValueTip");
		}

		// Token: 0x06005A3B RID: 23099 RVA: 0x001E7EB8 File Offset: 0x001E60B8
		private void DrawMass(Rect rect, TransferableOneWay trad, float availableMass)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Thing anyThing = trad.AnyThing;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null && !this.includePawnsMassInMassUsage && !MassUtility.CanEverCarryAnything(pawn))
			{
				return;
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (pawn == null || this.includePawnsMassInMassUsage)
			{
				float mass = this.GetMass(anyThing);
				if (Mouse.IsOver(rect))
				{
					if (pawn != null)
					{
						float gearMass = 0f;
						float invMass = 0f;
						gearMass = MassUtility.GearMass(pawn);
						if (!InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
						{
							invMass = MassUtility.InventoryMass(pawn);
						}
						TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, 0f, mass - gearMass - invMass, gearMass, invMass), trad.GetHashCode() * 59);
					}
					else
					{
						TooltipHandler.TipRegion(rect, "ItemWeightTip".Translate());
					}
				}
				if (mass > availableMass)
				{
					GUI.color = ColoredText.RedReadable;
				}
				else
				{
					GUI.color = TransferableOneWayWidget.ItemMassColor;
				}
				Widgets.Label(rect, mass.ToStringMass());
			}
			else
			{
				float cap = MassUtility.Capacity(pawn, null);
				float gearMass = MassUtility.GearMass(pawn);
				float invMass = InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass) ? 0f : MassUtility.InventoryMass(pawn);
				float num = cap - gearMass - invMass;
				if (num > 0f)
				{
					GUI.color = Color.green;
				}
				else if (num < 0f)
				{
					GUI.color = ColoredText.RedReadable;
				}
				else
				{
					GUI.color = Color.gray;
				}
				Widgets.Label(rect, num.ToStringMassOffset());
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.GetPawnMassTip(trad, cap, 0f, gearMass, invMass), trad.GetHashCode() * 59);
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x001E80F0 File Offset: 0x001E62F0
		private void DrawEquippedWeapon(Rect rect, Rect iconRect, TransferableOneWay trad)
		{
			if (!trad.HasAnyThing)
			{
				return;
			}
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn == null || pawn.equipment == null || pawn.equipment.Primary == null)
			{
				return;
			}
			ThingWithComps primary = pawn.equipment.Primary;
			Widgets.DrawHighlightIfMouseover(rect);
			Widgets.ThingIcon(iconRect, primary, 1f);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, primary.LabelCap);
			}
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x001E8164 File Offset: 0x001E6364
		private string GetPawnMassTip(TransferableOneWay trad, float capacity, float pawnMass, float gearMass, float invMass)
		{
			if (!trad.HasAnyThing)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (capacity != 0f)
			{
				stringBuilder.Append("MassCapacity".Translate() + ": " + capacity.ToStringMass());
			}
			else
			{
				stringBuilder.Append("Mass".Translate() + ": " + pawnMass.ToStringMass());
			}
			if (gearMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("EquipmentAndApparelMass".Translate() + ": " + gearMass.ToStringMass());
			}
			if (invMass != 0f)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("InventoryMass".Translate() + ": " + invMass.ToStringMass());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x001E8264 File Offset: 0x001E6464
		private float GetMass(Thing thing)
		{
			if (thing == null)
			{
				return 0f;
			}
			float num = thing.GetStatValue(StatDefOf.Mass, true);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, this.ignorePawnInventoryMass))
				{
					num -= MassUtility.InventoryMass(pawn);
				}
			}
			else if (this.ignoreSpawnedCorpseGearAndInventoryMass)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.Spawned)
				{
					num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn);
				}
			}
			return num;
		}

		// Token: 0x040030E6 RID: 12518
		private List<TransferableOneWayWidget.Section> sections = new List<TransferableOneWayWidget.Section>();

		// Token: 0x040030E7 RID: 12519
		private string sourceLabel;

		// Token: 0x040030E8 RID: 12520
		private string destinationLabel;

		// Token: 0x040030E9 RID: 12521
		private string sourceCountDesc;

		// Token: 0x040030EA RID: 12522
		private bool drawMass;

		// Token: 0x040030EB RID: 12523
		private IgnorePawnsInventoryMode ignorePawnInventoryMass = IgnorePawnsInventoryMode.DontIgnore;

		// Token: 0x040030EC RID: 12524
		private bool includePawnsMassInMassUsage;

		// Token: 0x040030ED RID: 12525
		private Func<float> availableMassGetter;

		// Token: 0x040030EE RID: 12526
		private float extraHeaderSpace;

		// Token: 0x040030EF RID: 12527
		private bool ignoreSpawnedCorpseGearAndInventoryMass;

		// Token: 0x040030F0 RID: 12528
		private int tile;

		// Token: 0x040030F1 RID: 12529
		private bool drawMarketValue;

		// Token: 0x040030F2 RID: 12530
		private bool drawEquippedWeapon;

		// Token: 0x040030F3 RID: 12531
		private bool drawNutritionEatenPerDay;

		// Token: 0x040030F4 RID: 12532
		private bool drawItemNutrition;

		// Token: 0x040030F5 RID: 12533
		private bool drawForagedFoodPerDay;

		// Token: 0x040030F6 RID: 12534
		private bool drawDaysUntilRot;

		// Token: 0x040030F7 RID: 12535
		private bool playerPawnsReadOnly;

		// Token: 0x040030F8 RID: 12536
		private bool transferablesCached;

		// Token: 0x040030F9 RID: 12537
		private Vector2 scrollPosition;

		// Token: 0x040030FA RID: 12538
		private TransferableSorterDef sorter1;

		// Token: 0x040030FB RID: 12539
		private TransferableSorterDef sorter2;

		// Token: 0x040030FC RID: 12540
		private Dictionary<TransferableOneWay, int> cachedTicksUntilRot = new Dictionary<TransferableOneWay, int>();

		// Token: 0x040030FD RID: 12541
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x040030FE RID: 12542
		private const float TopAreaHeight = 37f;

		// Token: 0x040030FF RID: 12543
		protected readonly Vector2 AcceptButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003100 RID: 12544
		protected readonly Vector2 OtherBottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04003101 RID: 12545
		private const float ColumnWidth = 120f;

		// Token: 0x04003102 RID: 12546
		private const float FirstTransferableY = 6f;

		// Token: 0x04003103 RID: 12547
		private const float RowInterval = 30f;

		// Token: 0x04003104 RID: 12548
		public const float CountColumnWidth = 75f;

		// Token: 0x04003105 RID: 12549
		public const float AdjustColumnWidth = 240f;

		// Token: 0x04003106 RID: 12550
		public const float MassColumnWidth = 100f;

		// Token: 0x04003107 RID: 12551
		public static readonly Color ItemMassColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04003108 RID: 12552
		private const float MarketValueColumnWidth = 100f;

		// Token: 0x04003109 RID: 12553
		private const float ExtraSpaceAfterSectionTitle = 5f;

		// Token: 0x0400310A RID: 12554
		private const float DaysUntilRotColumnWidth = 75f;

		// Token: 0x0400310B RID: 12555
		private const float NutritionEatenPerDayColumnWidth = 75f;

		// Token: 0x0400310C RID: 12556
		private const float ItemNutritionColumnWidth = 75f;

		// Token: 0x0400310D RID: 12557
		private const float ForagedFoodPerDayColumnWidth = 75f;

		// Token: 0x0400310E RID: 12558
		private const float GrazeabilityInnerColumnWidth = 40f;

		// Token: 0x0400310F RID: 12559
		private const float EquippedWeaponIconSize = 30f;

		// Token: 0x04003110 RID: 12560
		public const float TopAreaWidth = 515f;

		// Token: 0x04003111 RID: 12561
		private static readonly Texture2D CanGrazeIcon = ContentFinder<Texture2D>.Get("UI/Icons/CanGraze", true);

		// Token: 0x02001D78 RID: 7544
		private struct Section
		{
			// Token: 0x04006F7A RID: 28538
			public string title;

			// Token: 0x04006F7B RID: 28539
			public IEnumerable<TransferableOneWay> transferables;

			// Token: 0x04006F7C RID: 28540
			public List<TransferableOneWay> cachedTransferables;
		}
	}
}
