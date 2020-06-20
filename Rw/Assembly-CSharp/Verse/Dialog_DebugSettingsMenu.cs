using System;
using System.Reflection;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000361 RID: 865
	public class Dialog_DebugSettingsMenu : Dialog_DebugOptionLister
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000A0990 File Offset: 0x0009EB90
		public Dialog_DebugSettingsMenu()
		{
			this.forcePause = true;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000A09A0 File Offset: 0x0009EBA0
		protected override void DoListingItems()
		{
			if (KeyBindingDefOf.Dev_ToggleDebugSettingsMenu.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Small;
			this.listing.Label("Gameplay", -1f, null);
			foreach (FieldInfo fi in typeof(DebugSettings).GetFields())
			{
				this.DoField(fi);
			}
			this.listing.Gap(36f);
			Text.Font = GameFont.Small;
			this.listing.Label("View", -1f, null);
			foreach (FieldInfo fi2 in typeof(DebugViewSettings).GetFields())
			{
				this.DoField(fi2);
			}
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000A0A68 File Offset: 0x0009EC68
		private void DoField(FieldInfo fi)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			string label = GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
			bool flag = (bool)fi.GetValue(null);
			bool flag2 = flag;
			base.CheckboxLabeledDebug(label, ref flag);
			if (flag != flag2)
			{
				fi.SetValue(null, flag);
				MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}
	}
}
