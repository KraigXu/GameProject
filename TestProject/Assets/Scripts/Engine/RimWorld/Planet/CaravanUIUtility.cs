using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public static class CaravanUIUtility
	{
		
		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreSpawnedCorpsesGearAndInventoryMass, int tile, bool playerPawnsReadOnly = false)
		{
			pawnsTransfer = new TransferableOneWayWidget(null, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, true, true, false, true, false, playerPawnsReadOnly);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			itemsTransfer = new TransferableOneWayWidget(from x in transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, false, false, true, false, true, false);
		}

		
		public static void AddPawnsSections(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
		{
			IEnumerable<TransferableOneWay> source = from x in transferables
			where x.ThingDef.category == ThingCategory.Pawn
			select x;
			widget.AddSection("ColonistsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsFreeColonist
			select x);
			widget.AddSection("PrisonersSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsPrisoner
			select x);
			widget.AddSection("CaptureSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).Downed && CaravanUtility.ShouldAutoCapture((Pawn)x.AnyThing, Faction.OfPlayer)
			select x);
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).RaceProps.Animal
			select x);
		}

		
		private static string GetDaysWorthOfFoodLabel(Pair<float, float> daysWorthOfFood, bool multiline)
		{
			if (daysWorthOfFood.First >= 600f)
			{
				return "InfiniteDaysWorthOfFoodInfo".Translate();
			}
			string text = daysWorthOfFood.First.ToString("0.#");
			string str = multiline ? "\n" : " ";
			if (daysWorthOfFood.Second < 600f && daysWorthOfFood.Second < daysWorthOfFood.First)
			{
				text += str + "(" + "DaysWorthOfFoodInfoRot".Translate(daysWorthOfFood.Second.ToString("0.#") + ")");
			}
			return text;
		}

		
		private static Color GetDaysWorthOfFoodColor(Pair<float, float> daysWorthOfFood, int? ticksToArrive)
		{
			if (daysWorthOfFood.First >= 600f)
			{
				return Color.white;
			}
			float num = Mathf.Min(daysWorthOfFood.First, daysWorthOfFood.Second);
			if (ticksToArrive != null)
			{
				return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodKnownRouteColor, num / ((float)ticksToArrive.Value / 60000f));
			}
			return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodColor, num);
		}

		
		public static void DrawCaravanInfo(CaravanUIUtility.CaravanInfo info, CaravanUIUtility.CaravanInfo? info2, int currentTile, int? ticksToArrive, float lastMassFlashTime, Rect rect, bool lerpMassColor = true, string extraDaysWorthOfFoodTipInfo = null, bool multiline = false)
		{
			CaravanUIUtility.tmpInfo.Clear();
			TaggedString taggedString = info.massUsage.ToStringEnsureThreshold(info.massCapacity, 0) + " / " + info.massCapacity.ToString("F0") + " " + "kg".Translate();
			TaggedString taggedString2;
			if (info2 == null)
			{
				taggedString2 = null;
			}
			else
			{
				string str = info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 0);
				string str2 = " / ";
				CaravanUIUtility.CaravanInfo value = info2.Value;
				taggedString2 = str + str2 + value.massCapacity.ToString("F0") + " " + "kg".Translate();
			}
			TaggedString taggedString3 = taggedString2;
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Mass".Translate(), taggedString, CaravanUIUtility.GetMassColor(info.massUsage, info.massCapacity, lerpMassColor), CaravanUIUtility.GetMassTip(info.massUsage, info.massCapacity, info.massCapacityExplanation, (info2 != null) ? new float?(info2.Value.massUsage) : null, (info2 != null) ? new float?(info2.Value.massCapacity) : null, (info2 != null) ? info2.Value.massCapacityExplanation : null), taggedString3, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.massUsage, info2.Value.massCapacity, lerpMassColor) : Color.white, lastMassFlashTime));
			if (info.extraMassUsage != -1f)
			{
				TaggedString taggedString4 = info.extraMassUsage.ToStringEnsureThreshold(info.extraMassCapacity, 0) + " / " + info.extraMassCapacity.ToString("F0") + " " + "kg".Translate();
				TaggedString taggedString5;
				if (info2 == null)
				{
					taggedString5 = null;
				}
				else
				{
					string str3 = info2.Value.extraMassUsage.ToStringEnsureThreshold(info2.Value.extraMassCapacity, 0);
					string str4 = " / ";
					CaravanUIUtility.CaravanInfo value = info2.Value;
					taggedString5 = str3 + str4 + value.extraMassCapacity.ToString("F0") + " " + "kg".Translate();
				}
				TaggedString taggedString6 = taggedString5;
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMass".Translate(), taggedString4, CaravanUIUtility.GetMassColor(info.extraMassUsage, info.extraMassCapacity, true), CaravanUIUtility.GetMassTip(info.extraMassUsage, info.extraMassCapacity, info.extraMassCapacityExplanation, (info2 != null) ? new float?(info2.Value.extraMassUsage) : null, (info2 != null) ? new float?(info2.Value.extraMassCapacity) : null, (info2 != null) ? info2.Value.extraMassCapacityExplanation : null), taggedString6, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.extraMassUsage, info2.Value.extraMassCapacity, true) : Color.white, -9999f));
			}
			string text = "CaravanMovementSpeedTip".Translate();
			if (!info.tilesPerDayExplanation.NullOrEmpty())
			{
				text = text + "\n\n" + info.tilesPerDayExplanation;
			}
			if (info2 != null && !info2.Value.tilesPerDayExplanation.NullOrEmpty())
			{
				text = text + "\n\n-----\n\n" + info2.Value.tilesPerDayExplanation;
			}
			List<TransferableUIUtility.ExtraInfo> list = CaravanUIUtility.tmpInfo;
			string key = "CaravanMovementSpeed".Translate();
			string value2 = info.tilesPerDay.ToString("0.#") + " " + "TilesPerDay".Translate();
			Color color = GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, info.tilesPerDay);
			string tip = text;
			TaggedString taggedString7;
			if (info2 == null)
			{
				taggedString7 = null;
			}
			else
			{
				CaravanUIUtility.CaravanInfo value = info2.Value;
				taggedString7 = value.tilesPerDay.ToString("0.#") + " " + "TilesPerDay".Translate();
			}
			list.Add(new TransferableUIUtility.ExtraInfo(key, value2, color, tip, taggedString7, (info2 != null) ? GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, info2.Value.tilesPerDay) : Color.white, -9999f));
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("DaysWorthOfFood".Translate(), CaravanUIUtility.GetDaysWorthOfFoodLabel(info.daysWorthOfFood, multiline), CaravanUIUtility.GetDaysWorthOfFoodColor(info.daysWorthOfFood, ticksToArrive), "DaysWorthOfFoodTooltip".Translate() + extraDaysWorthOfFoodTipInfo + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(currentTile, Find.TickManager.TicksAbs), (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodLabel(info2.Value.daysWorthOfFood, multiline) : null, (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodColor(info2.Value.daysWorthOfFood, ticksToArrive) : Color.white, -9999f));
			string text2 = info.foragedFoodPerDay.Second.ToString("0.#");
			string text3;
			if (info2 == null)
			{
				text3 = null;
			}
			else
			{
				CaravanUIUtility.CaravanInfo value = info2.Value;
				text3 = value.foragedFoodPerDay.Second.ToString("0.#");
			}
			string text4 = text3;
			TaggedString taggedString8 = "ForagedFoodPerDayTip".Translate();
			taggedString8 += "\n\n" + info.foragedFoodPerDayExplanation;
			if (info2 != null)
			{
				taggedString8 += "\n\n-----\n\n" + info2.Value.foragedFoodPerDayExplanation;
			}
			if (info.foragedFoodPerDay.Second <= 0f)
			{
				if (info2 == null)
				{
					goto IL_6A8;
				}
				CaravanUIUtility.CaravanInfo value = info2.Value;
				if (value.foragedFoodPerDay.Second <= 0f)
				{
					goto IL_6A8;
				}
			}
			string text5 = multiline ? "\n" : " ";
			if (info2 == null)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					text5,
					"(",
					info.foragedFoodPerDay.First.label,
					")"
				});
			}
			else
			{
				string[] array = new string[5];
				array[0] = text4;
				array[1] = text5;
				array[2] = "(";
				int num = 3;
				CaravanUIUtility.CaravanInfo value = info2.Value;
				array[num] = value.foragedFoodPerDay.First.label.Truncate(50f, null);
				array[4] = ")";
				text4 = string.Concat(array);
			}
			IL_6A8:
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("ForagedFoodPerDay".Translate(), text2, Color.white, taggedString8, text4, Color.white, -9999f));
			string text6 = "CaravanVisibilityTip".Translate();
			if (!info.visibilityExplanation.NullOrEmpty())
			{
				text6 = text6 + "\n\n" + info.visibilityExplanation;
			}
			if (info2 != null && !info2.Value.visibilityExplanation.NullOrEmpty())
			{
				text6 = text6 + "\n\n-----\n\n" + info2.Value.visibilityExplanation;
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Visibility".Translate(), info.visibility.ToStringPercent(), GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info.visibility), text6, (info2 != null) ? info2.Value.visibility.ToStringPercent() : null, (info2 != null) ? GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info2.Value.visibility) : Color.white, -9999f));
			TransferableUIUtility.DrawExtraInfo(CaravanUIUtility.tmpInfo, rect);
		}

		
		private static Color GetMassColor(float massUsage, float massCapacity, bool lerpMassColor)
		{
			if (massCapacity == 0f)
			{
				return Color.white;
			}
			if (massUsage > massCapacity)
			{
				return Color.red;
			}
			if (lerpMassColor)
			{
				return GenUI.LerpColor(CaravanUIUtility.MassColor, massUsage / massCapacity);
			}
			return Color.white;
		}

		
		private static string GetMassTip(float massUsage, float massCapacity, string massCapacityExplanation, float? massUsage2, float? massCapacity2, string massCapacity2Explanation)
		{
			TaggedString taggedString = "MassCarriedSimple".Translate() + ": " + massUsage.ToStringEnsureThreshold(massCapacity, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity.ToString("F2") + " " + "kg".Translate();
			if (massUsage2 != null)
			{
				taggedString += "\n <-> \n" + "MassCarriedSimple".Translate() + ": " + massUsage2.Value.ToStringEnsureThreshold(massCapacity2.Value, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity2.Value.ToString("F2") + " " + "kg".Translate();
			}
			taggedString += "\n\n" + "CaravanMassUsageTooltip".Translate();
			if (!massCapacityExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacityExplanation;
			}
			if (!massCapacity2Explanation.NullOrEmpty())
			{
				taggedString += "\n\n-----\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacity2Explanation;
			}
			return taggedString;
		}

		
		private static readonly List<Pair<float, Color>> MassColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.37f, Color.green),
			new Pair<float, Color>(0.82f, Color.yellow),
			new Pair<float, Color>(1f, new Color(1f, 0.6f, 0f))
		};

		
		private static readonly List<Pair<float, Color>> TilesPerDayColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.001f, ColoredText.RedReadable),
			new Pair<float, Color>(1f, Color.yellow),
			new Pair<float, Color>(2f, Color.white)
		};

		
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(1f, Color.red),
			new Pair<float, Color>(2f, Color.white)
		};

		
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodKnownRouteColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.3f, ColoredText.RedReadable),
			new Pair<float, Color>(0.9f, Color.yellow),
			new Pair<float, Color>(1.02f, Color.green)
		};

		
		private static readonly List<Pair<float, Color>> VisibilityColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.01f, Color.green),
			new Pair<float, Color>(0.2f, Color.green),
			new Pair<float, Color>(1f, Color.white),
			new Pair<float, Color>(1.2f, ColoredText.RedReadable)
		};

		
		private static List<TransferableUIUtility.ExtraInfo> tmpInfo = new List<TransferableUIUtility.ExtraInfo>();

		
		public struct CaravanInfo
		{
			
			public CaravanInfo(float massUsage, float massCapacity, string massCapacityExplanation, float tilesPerDay, string tilesPerDayExplanation, Pair<float, float> daysWorthOfFood, Pair<ThingDef, float> foragedFoodPerDay, string foragedFoodPerDayExplanation, float visibility, string visibilityExplanation, float extraMassUsage = -1f, float extraMassCapacity = -1f, string extraMassCapacityExplanation = null)
			{
				this.massUsage = massUsage;
				this.massCapacity = massCapacity;
				this.massCapacityExplanation = massCapacityExplanation;
				this.tilesPerDay = tilesPerDay;
				this.tilesPerDayExplanation = tilesPerDayExplanation;
				this.daysWorthOfFood = daysWorthOfFood;
				this.foragedFoodPerDay = foragedFoodPerDay;
				this.foragedFoodPerDayExplanation = foragedFoodPerDayExplanation;
				this.visibility = visibility;
				this.visibilityExplanation = visibilityExplanation;
				this.extraMassUsage = extraMassUsage;
				this.extraMassCapacity = extraMassCapacity;
				this.extraMassCapacityExplanation = extraMassCapacityExplanation;
			}

			
			public float massUsage;

			
			public float massCapacity;

			
			public string massCapacityExplanation;

			
			public float tilesPerDay;

			
			public string tilesPerDayExplanation;

			
			public Pair<float, float> daysWorthOfFood;

			
			public Pair<ThingDef, float> foragedFoodPerDay;

			
			public string foragedFoodPerDayExplanation;

			
			public float visibility;

			
			public string visibilityExplanation;

			
			public float extraMassUsage;

			
			public float extraMassCapacity;

			
			public string extraMassCapacityExplanation;
		}
	}
}
