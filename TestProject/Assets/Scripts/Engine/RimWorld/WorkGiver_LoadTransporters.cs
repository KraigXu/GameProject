using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200074B RID: 1867
	public class WorkGiver_LoadTransporters : WorkGiver_Scanner
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060030ED RID: 12525 RVA: 0x0011220B File Offset: 0x0011040B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Transporter);
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060030EE RID: 12526 RVA: 0x0007C4F4 File Offset: 0x0007A6F4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000E3FA9 File Offset: 0x000E21A9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x00112214 File Offset: 0x00110414
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x00112230 File Offset: 0x00110430
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompTransporter transporter = t.TryGetComp<CompTransporter>();
			return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
		}
	}
}
