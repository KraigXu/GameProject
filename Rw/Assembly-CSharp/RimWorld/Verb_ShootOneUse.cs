using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001036 RID: 4150
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x0600633A RID: 25402 RVA: 0x00227D87 File Offset: 0x00225F87
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x00227DBC File Offset: 0x00225FBC
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x0600633C RID: 25404 RVA: 0x00227DE6 File Offset: 0x00225FE6
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
