using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000256 RID: 598
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x0005DDFF File Offset: 0x0005BFFF
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0005DE0C File Offset: 0x0005C00C
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, source);
			HediffComp_Link hediffComp_Link = this.parent.TryGetComp<HediffComp_Link>();
			if (hediffComp_Link != null && hediffComp_Link.other != source && hediffComp_Link.other.psychicEntropy != null)
			{
				hediffComp_Link.other.psychicEntropy.TryAddEntropy(baseAmount * this.Props.entropyTransferAmount, this.parent.pawn, false, false);
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, hediffComp_Link.other);
			}
		}
	}
}
