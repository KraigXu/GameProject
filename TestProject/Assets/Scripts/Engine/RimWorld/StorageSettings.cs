using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6B RID: 3179
	public class StorageSettings : IExposable
	{
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x0019929F File Offset: 0x0019749F
		private IHaulDestination HaulDestinationOwner
		{
			get
			{
				return this.owner as IHaulDestination;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x001992AC File Offset: 0x001974AC
		private ISlotGroupParent SlotGroupParentOwner
		{
			get
			{
				return this.owner as ISlotGroupParent;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x001992B9 File Offset: 0x001974B9
		// (set) Token: 0x06004C25 RID: 19493 RVA: 0x001992C4 File Offset: 0x001974C4
		public StoragePriority Priority
		{
			get
			{
				return this.priorityInt;
			}
			set
			{
				this.priorityInt = value;
				if (Current.ProgramState == ProgramState.Playing && this.HaulDestinationOwner != null && this.HaulDestinationOwner.Map != null)
				{
					this.HaulDestinationOwner.Map.haulDestinationManager.Notify_HaulDestinationChangedPriority();
				}
				if (Current.ProgramState == ProgramState.Playing && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.Map != null)
				{
					this.SlotGroupParentOwner.Map.listerHaulables.RecalcAllInCells(this.SlotGroupParentOwner.AllSlotCells());
				}
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00199347 File Offset: 0x00197547
		public StorageSettings()
		{
			this.filter = new ThingFilter(new Action(this.TryNotifyChanged));
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00199370 File Offset: 0x00197570
		public StorageSettings(IStoreSettingsParent owner) : this()
		{
			this.owner = owner;
			if (owner != null)
			{
				StorageSettings parentStoreSettings = owner.GetParentStoreSettings();
				if (parentStoreSettings != null)
				{
					this.priorityInt = parentStoreSettings.priorityInt;
				}
			}
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001993A3 File Offset: 0x001975A3
		public void ExposeData()
		{
			Scribe_Values.Look<StoragePriority>(ref this.priorityInt, "priority", StoragePriority.Unstored, false);
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", new object[]
			{
				new Action(this.TryNotifyChanged)
			});
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001993DC File Offset: 0x001975DC
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			this.filter.SetFromPreset(preset);
			this.TryNotifyChanged();
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001993F0 File Offset: 0x001975F0
		public void CopyFrom(StorageSettings other)
		{
			this.Priority = other.Priority;
			this.filter.CopyAllowancesFrom(other.filter);
			this.TryNotifyChanged();
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x00199418 File Offset: 0x00197618
		public bool AllowedToAccept(Thing t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x00199458 File Offset: 0x00197658
		public bool AllowedToAccept(ThingDef t)
		{
			if (!this.filter.Allows(t))
			{
				return false;
			}
			if (this.owner != null)
			{
				StorageSettings parentStoreSettings = this.owner.GetParentStoreSettings();
				if (parentStoreSettings != null && !parentStoreSettings.AllowedToAccept(t))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x00199498 File Offset: 0x00197698
		private void TryNotifyChanged()
		{
			if (this.owner != null && this.SlotGroupParentOwner != null && this.SlotGroupParentOwner.GetSlotGroup() != null && this.SlotGroupParentOwner.Map != null)
			{
				this.SlotGroupParentOwner.Map.listerHaulables.Notify_SlotGroupChanged(this.SlotGroupParentOwner.GetSlotGroup());
			}
		}

		// Token: 0x04002AE6 RID: 10982
		public IStoreSettingsParent owner;

		// Token: 0x04002AE7 RID: 10983
		public ThingFilter filter;

		// Token: 0x04002AE8 RID: 10984
		[LoadAlias("priority")]
		private StoragePriority priorityInt = StoragePriority.Normal;
	}
}
