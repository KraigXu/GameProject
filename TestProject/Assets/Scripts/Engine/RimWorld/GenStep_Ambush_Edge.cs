using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A64 RID: 2660
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x0014E0C7 File Offset: 0x0014C2C7
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0014E0CE File Offset: 0x0014C2CE
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
