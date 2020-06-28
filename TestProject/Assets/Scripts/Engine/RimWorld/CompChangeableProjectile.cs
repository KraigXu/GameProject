using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFC RID: 3324
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x001B21D7 File Offset: 0x001B03D7
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x17000E2D RID: 3629
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

		// Token: 0x17000E2E RID: 3630
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

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x001B220E File Offset: 0x001B040E
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001B2219 File Offset: 0x001B0419
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x001B2254 File Offset: 0x001B0454
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x001B22AB File Offset: 0x001B04AB
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

		// Token: 0x060050D4 RID: 20692 RVA: 0x001B22D4 File Offset: 0x001B04D4
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x001B22F1 File Offset: 0x001B04F1
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x001B2319 File Offset: 0x001B0519
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x060050D7 RID: 20695 RVA: 0x001B2321 File Offset: 0x001B0521
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x04002CE0 RID: 11488
		private ThingDef loadedShell;

		// Token: 0x04002CE1 RID: 11489
		public int loadedCount;

		// Token: 0x04002CE2 RID: 11490
		public StorageSettings allowedShellsSettings;
	}
}
