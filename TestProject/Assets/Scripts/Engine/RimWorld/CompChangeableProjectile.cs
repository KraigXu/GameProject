using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x001B21D7 File Offset: 0x001B03D7
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x001B21E4 File Offset: 0x001B03E4
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

		
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x001B21F7 File Offset: 0x001B03F7
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

		
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x001B220E File Offset: 0x001B040E
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0001028D File Offset: 0x0000E48D
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
