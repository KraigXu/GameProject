using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class EnterCooldownComp : WorldObjectComp
	{
		
		// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x0026A6CF File Offset: 0x002688CF
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x0026A6DC File Offset: 0x002688DC
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		
		// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x0026A6E7 File Offset: 0x002688E7
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		
		// (get) Token: 0x06006ED7 RID: 28375 RVA: 0x0026A6FC File Offset: 0x002688FC
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.ticksLeft;
			}
		}

		
		// (get) Token: 0x06006ED8 RID: 28376 RVA: 0x0026A70E File Offset: 0x0026890E
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		
		private int ticksLeft;
	}
}
