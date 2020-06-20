using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200060F RID: 1551
	public class Trigger_ImportantTraderCaravanPeopleLost : Trigger
	{
		// Token: 0x06002A40 RID: 10816 RVA: 0x000F6918 File Offset: 0x000F4B18
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.IncappedOrKilled || signal.condition == PawnLostCondition.MadePrisoner))
			{
				if (signal.Pawn.GetTraderCaravanRole() == TraderCaravanRole.Trader || signal.Pawn.RaceProps.packAnimal)
				{
					return true;
				}
				if (lord.numPawnsLostViolently > 0 && (float)lord.numPawnsLostViolently / (float)lord.numPawnsEverGained >= 0.5f)
				{
					return true;
				}
			}
			return false;
		}
	}
}
