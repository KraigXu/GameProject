using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001277 RID: 4727
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x0026A6CF File Offset: 0x002688CF
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x0026A6DC File Offset: 0x002688DC
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x0026A6E7 File Offset: 0x002688E7
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x1700129E RID: 4766
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

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x06006ED8 RID: 28376 RVA: 0x0026A70E File Offset: 0x0026890E
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x0026A720 File Offset: 0x00268920
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06006EDA RID: 28378 RVA: 0x0026A75F File Offset: 0x0026895F
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06006EDB RID: 28379 RVA: 0x0026A768 File Offset: 0x00268968
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06006EDC RID: 28380 RVA: 0x0026A786 File Offset: 0x00268986
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x0026A79C File Offset: 0x0026899C
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x0026A7CB File Offset: 0x002689CB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04004447 RID: 17479
		private int ticksLeft;
	}
}
