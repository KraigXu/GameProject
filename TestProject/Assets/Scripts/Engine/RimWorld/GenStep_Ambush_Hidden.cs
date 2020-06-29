using System;
using Verse;

namespace RimWorld
{
	
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x0014E0E8 File Offset: 0x0014C2E8
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			if (root.IsValid)
			{
				signalAction_Ambush.spawnNear = root;
			}
			else
			{
				signalAction_Ambush.spawnAround = rectToDefend;
			}
			return signalAction_Ambush;
		}
	}
}
