using System;

namespace Verse
{
	// Token: 0x020001FF RID: 511
	public abstract class ModSettings : IExposable
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00053FB0 File Offset: 0x000521B0
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00053FB8 File Offset: 0x000521B8
		public Mod Mod { get; internal set; }

		// Token: 0x06000EBC RID: 3772 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00053FC1 File Offset: 0x000521C1
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
