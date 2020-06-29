using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class LordToil_Party : LordToil_Gathering
	{
		
		
		private LordToilData_Party Data
		{
			get
			{
				return (LordToilData_Party)this.data;
			}
		}

		
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
			this.data = new LordToilData_Party();
		}

		
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

		
		private float joyPerTick = 3.5E-05f;

		
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
