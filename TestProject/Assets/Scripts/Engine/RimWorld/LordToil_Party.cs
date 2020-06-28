using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	public class LordToil_Party : LordToil_Gathering
	{
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003300 RID: 13056 RVA: 0x0011B445 File Offset: 0x00119645
		private LordToilData_Party Data
		{
			get
			{
				return (LordToilData_Party)this.data;
			}
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x0011B452 File Offset: 0x00119652
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
			this.data = new LordToilData_Party();
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x0011B47C File Offset: 0x0011967C
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					ownedPawns[i].needs.joy.GainJoy(this.joyPerTick, JoyKindDefOf.Social);
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x04001B7D RID: 7037
		private float joyPerTick = 3.5E-05f;

		// Token: 0x04001B7E RID: 7038
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
