using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		
		
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		
		
		public ThingDef LoadedShell
		{
			get
			{
				if (this.loadedCount <= 0)
				{
					return null;
				}
				return this.loadedShell;
			}
		}

		
		
		public ThingDef Projectile
		{
			get
			{
				if (!this.Loaded)
				{
					return null;
				}
				return this.LoadedShell.projectileWhenLoaded;
			}
		}

		
		
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		
		
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		
		public virtual void Notify_ProjectileLaunched()
		{
			if (this.loadedCount > 0)
			{
				this.loadedCount--;
			}
			if (this.loadedCount <= 0)
			{
				this.loadedShell = null;
			}
		}

		
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		
		private ThingDef loadedShell;

		
		public int loadedCount;

		
		public StorageSettings allowedShellsSettings;
	}
}
