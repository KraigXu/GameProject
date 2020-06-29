using System;

namespace Verse
{
	
	public abstract class ModSettings : IExposable
	{
		
		// (get) Token: 0x06000EBA RID: 3770 RVA: 0x00053FB0 File Offset: 0x000521B0
		// (set) Token: 0x06000EBB RID: 3771 RVA: 0x00053FB8 File Offset: 0x000521B8
		public Mod Mod { get; internal set; }

		
		public virtual void ExposeData()
		{
		}

		
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
