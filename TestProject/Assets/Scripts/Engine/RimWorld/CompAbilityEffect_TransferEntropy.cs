using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ADA RID: 2778
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x0015F1CD File Offset: 0x0015D3CD
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0015F1DC File Offset: 0x0015D3DC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = this.parent.pawn;
				if (this.Props.targetReceivesEntropy)
				{
					pawn.psychicEntropy.TryAddEntropy(pawn2.psychicEntropy.EntropyValue, pawn2, false, true);
				}
				pawn2.psychicEntropy.RemoveAllEntropy();
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, pawn);
				return;
			}
			Log.Error("CompAbilityEffect_TransferEntropy is only applicable to pawns.", false);
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0015F267 File Offset: 0x0015D467
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0015F2A0 File Offset: 0x0015D4A0
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null && pawn.InMentalState)
			{
				if (throwMessages)
				{
					Messages.Message("AbilityCantApplyToMentallyBroken".Translate(pawn.LabelShort), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
