using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5C RID: 3676
	public class Dialog_ModSettings : Window
	{
		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06005923 RID: 22819 RVA: 0x001DAEE7 File Offset: 0x001D90E7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x001DC1B0 File Offset: 0x001DA3B0
		public Dialog_ModSettings()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x001DC39D File Offset: 0x001DA59D
		public override void PreClose()
		{
			base.PreClose();
			if (this.selMod != null)
			{
				this.selMod.WriteSettings();
			}
		}

		// Token: 0x06005926 RID: 22822 RVA: 0x001DC3B8 File Offset: 0x001DA5B8
		public override void DoWindowContents(Rect inRect)
		{
			if (Widgets.ButtonText(new Rect(0f, 0f, 150f, 35f), "SelectMod".Translate(), true, true, true))
			{
				if (Dialog_ModSettings.HasSettings())
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Mod mod2 in from mod in LoadedModManager.ModHandles
					where !mod.SettingsCategory().NullOrEmpty()
					orderby mod.SettingsCategory()
					select mod)
					{
						Mod localMod = mod2;
						list.Add(new FloatMenuOption(mod2.SettingsCategory(), delegate
						{
							if (this.selMod != null)
							{
								this.selMod.WriteSettings();
							}
							this.selMod = localMod;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				else
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					list2.Add(new FloatMenuOption("NoConfigurableMods".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			if (this.selMod != null)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(167f, 0f, inRect.width - 150f - 17f, 35f), this.selMod.SettingsCategory());
				Text.Font = GameFont.Small;
				Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y);
				this.selMod.DoSettingsWindowContents(inRect2);
			}
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x001DC5A4 File Offset: 0x001DA7A4
		public static bool HasSettings()
		{
			return LoadedModManager.ModHandles.Any((Mod mod) => !mod.SettingsCategory().NullOrEmpty());
		}

		// Token: 0x04003045 RID: 12357
		private Mod selMod;

		// Token: 0x04003046 RID: 12358
		private const float TopAreaHeight = 40f;

		// Token: 0x04003047 RID: 12359
		private const float TopButtonHeight = 35f;

		// Token: 0x04003048 RID: 12360
		private const float TopButtonWidth = 150f;
	}
}
