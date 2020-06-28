using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5B RID: 3675
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x001DC19F File Offset: 0x001DA39F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(346f, 350f);
			}
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x001DC1B0 File Offset: 0x001DA3B0
		public Dialog_MedicalDefaults()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x001DC1DC File Offset: 0x001DA3DC
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, 170f, 28f);
			Rect rect2 = new Rect(170f, 0f, 140f, 28f);
			Widgets.Label(rect, "MedGroupColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyHumanlike);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupImprisonedColonist".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyPrisoner);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupColonyAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForColonyAnimal);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupNeutralAnimal".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralAnimal);
			rect.y += 34f;
			rect2.y += 34f;
			Widgets.Label(rect, "MedGroupNeutralFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForNeutralFaction);
			rect.y += 52f;
			rect2.y += 52f;
			Widgets.Label(rect, "MedGroupHostileFaction".Translate());
			MedicalCareUtility.MedicalCareSetter(rect2, ref Find.PlaySettings.defaultCareForHostileFaction);
		}

		// Token: 0x04003042 RID: 12354
		private const float MedicalCareStartX = 170f;

		// Token: 0x04003043 RID: 12355
		private const float VerticalGap = 6f;

		// Token: 0x04003044 RID: 12356
		private const float VerticalBigGap = 24f;
	}
}
