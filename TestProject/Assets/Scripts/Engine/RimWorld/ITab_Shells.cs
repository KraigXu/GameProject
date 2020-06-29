using System;

namespace RimWorld
{
	
	public class ITab_Shells : ITab_Storage
	{
		
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

		
		// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
