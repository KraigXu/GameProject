using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EF RID: 495
	public abstract class Mod
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000504C8 File Offset: 0x0004E6C8
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000504D0 File Offset: 0x0004E6D0
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000504E0 File Offset: 0x0004E6E0
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)), false);
				return default(T);
			}
			if (this.modSettings != null)
			{
				return (T)((object)this.modSettings);
			}
			this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.FolderName, base.GetType().Name);
			this.modSettings.Mod = this;
			return this.modSettings as T;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000505A1 File Offset: 0x0004E7A1
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x04000A9D RID: 2717
		private ModSettings modSettings;

		// Token: 0x04000A9E RID: 2718
		private ModContentPack intContent;
	}
}
