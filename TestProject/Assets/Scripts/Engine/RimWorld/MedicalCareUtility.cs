using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BBB RID: 3003
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		// Token: 0x060046ED RID: 18157 RVA: 0x0017FF02 File Offset: 0x0017E102
		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x0017FF28 File Offset: 0x0017E128
		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				MouseoverSounds.DoRegion(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3);
				}
				if (Mouse.IsOver(rect2))
				{
					TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				}
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00180035 File Offset: 0x0017E235
		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00180054 File Offset: 0x0017E254
		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.NormalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x001800CC File Offset: 0x0017E2CC
		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu), null, MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare], null, null, null, true);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x0018010E File Offset: 0x0017E30E
		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x0018011B File Offset: 0x0017E31B
		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			int num;
			for (int i = 0; i < 5; i = num + 1)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption(mc.GetLabel(), delegate
					{
						p.playerSettings.medCare = mc;
					}, MenuOptionPriority.Default, null, null, 0f, null, null),
					payload = mc
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x040028B9 RID: 10425
		private static Texture2D[] careTextures;

		// Token: 0x040028BA RID: 10426
		public const float CareSetterHeight = 28f;

		// Token: 0x040028BB RID: 10427
		public const float CareSetterWidth = 140f;

		// Token: 0x040028BC RID: 10428
		private static bool medicalCarePainting;
	}
}
