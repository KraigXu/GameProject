using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006FD RID: 1789
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x06002F53 RID: 12115 RVA: 0x0010A278 File Offset: 0x00108478
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x0010A2A8 File Offset: 0x001084A8
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null) || pawn.Map.weatherManager.curWeather.rainRate > 0.1f)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
