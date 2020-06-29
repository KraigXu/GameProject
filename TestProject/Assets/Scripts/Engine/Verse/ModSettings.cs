using System;

namespace Verse
{
	
	public abstract class ModSettings : IExposable
	{
		
		
		
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
