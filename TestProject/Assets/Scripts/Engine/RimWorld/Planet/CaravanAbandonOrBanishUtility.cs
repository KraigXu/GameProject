using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200122B RID: 4651
	public static class CaravanAbandonOrBanishUtility
	{
		// Token: 0x06006C4A RID: 27722 RVA: 0x0025BE4C File Offset: 0x0025A04C
		public static void TryAbandonOrBanishViaInterface(Thing t, Caravan caravan)
		{
			Pawn p = t as Pawn;
			if (p == null)
			{
				Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.Label), delegate
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + t, false);
						return;
					}
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
					caravan.RecacheImmobilizedNow();
					caravan.RecacheDaysWorthOfFood();
				}, true, null);
				Find.WindowStack.Add(window);
				return;
			}
			if (!caravan.PawnsListForReading.Any((Pawn x) => x != p && caravan.IsOwner(x)))
			{
				Messages.Message("MessageCantBanishLastColonist".Translate(), caravan, MessageTypeDefOf.RejectInput, false);
				return;
			}
			PawnBanishUtility.ShowBanishPawnConfirmationDialog(p);
		}

		// Token: 0x06006C4B RID: 27723 RVA: 0x0025BF10 File Offset: 0x0025A110
		public static void TryAbandonOrBanishViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(pawn, caravan);
				return;
			}
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.LabelWithTotalStackCount), delegate
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					Thing thing = t.things[i];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing, false);
						return;
					}
					ownerOf.inventory.innerContainer.Remove(thing);
					thing.Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, true, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x0025BF8C File Offset: 0x0025A18C
		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.LabelNoCount), 1, t.stackCount, delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + t, false);
					return;
				}
				if (x >= t.stackCount)
				{
					ownerOf.inventory.innerContainer.Remove(t);
					t.Destroy(DestroyMode.Vanish);
				}
				else
				{
					t.SplitOff(x).Destroy(DestroyMode.Vanish);
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, int.MinValue));
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x0025BFF8 File Offset: 0x0025A1F8
		public static void TryAbandonSpecificCountViaInterface(TransferableImmutable t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.Label), 1, t.TotalStackCount, delegate(int x)
			{
				int num = x;
				int num2 = 0;
				while (num2 < t.things.Count && num > 0)
				{
					Thing thing = t.things[num2];
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + thing, false);
						return;
					}
					if (num >= thing.stackCount)
					{
						num -= thing.stackCount;
						ownerOf.inventory.innerContainer.Remove(thing);
						thing.Destroy(DestroyMode.Vanish);
					}
					else
					{
						thing.SplitOff(num).Destroy(DestroyMode.Vanish);
						num = 0;
					}
					num2++;
				}
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}, int.MinValue));
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x0025C064 File Offset: 0x0025A264
		public static string GetAbandonOrBanishButtonTooltip(Thing t, bool abandonSpecificCount)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.stackCount, abandonSpecificCount);
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x0025C090 File Offset: 0x0025A290
		public static string GetAbandonOrBanishButtonTooltip(TransferableImmutable t, bool abandonSpecificCount)
		{
			Pawn pawn = t.AnyThing as Pawn;
			if (pawn != null)
			{
				return PawnBanishUtility.GetBanishButtonTip(pawn);
			}
			return CaravanAbandonOrBanishUtility.GetAbandonItemButtonTooltip(t.TotalStackCount, abandonSpecificCount);
		}

		// Token: 0x06006C50 RID: 27728 RVA: 0x0025C0C0 File Offset: 0x0025A2C0
		private static string GetAbandonItemButtonTooltip(int currentStackCount, bool abandonSpecificCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (currentStackCount == 1)
			{
				stringBuilder.AppendLine("AbandonTip".Translate());
			}
			else if (abandonSpecificCount)
			{
				stringBuilder.AppendLine("AbandonSpecificCountTip".Translate());
			}
			else
			{
				stringBuilder.AppendLine("AbandonAllTip".Translate());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("AbandonItemTipExtraText".Translate());
			return stringBuilder.ToString();
		}
	}
}
