using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A65 RID: 2661
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x0014E0E8 File Offset: 0x0014C2E8
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x0014E0EF File Offset: 0x0014C2EF
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x0014E100 File Offset: 0x0014C300
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
