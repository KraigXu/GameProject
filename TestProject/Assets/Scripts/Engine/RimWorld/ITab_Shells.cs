using System;

namespace RimWorld
{
	// Token: 0x02000EB3 RID: 3763
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06005BD8 RID: 23512 RVA: 0x001FB628 File Offset: 0x001F9828
		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				if (selStoreSettingsParent != null)
				{
					return selStoreSettingsParent;
				}
				Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
				if (building_TurretGun != null)
				{
					return base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
				}
				return null;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005BDA RID: 23514 RVA: 0x001FB65E File Offset: 0x001F985E
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
