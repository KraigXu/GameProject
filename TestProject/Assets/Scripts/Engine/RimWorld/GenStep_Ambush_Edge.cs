﻿using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		
		// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x0014E0C7 File Offset: 0x0014C2C7
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
