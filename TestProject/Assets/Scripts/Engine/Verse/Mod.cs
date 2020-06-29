using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class Mod
	{
		
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000504C8 File Offset: 0x0004E6C8
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		
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

		
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		
		public virtual string SettingsCategory()
		{
			return "";
		}

		
		private ModSettings modSettings;

		
		private ModContentPack intContent;
	}
}
