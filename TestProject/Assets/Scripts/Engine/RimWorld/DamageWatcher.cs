using System;
using Verse;

namespace RimWorld
{
	
	public class DamageWatcher : IExposable
	{
		
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x00145F24 File Offset: 0x00144124
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		
		private float everDamage;
	}
}
