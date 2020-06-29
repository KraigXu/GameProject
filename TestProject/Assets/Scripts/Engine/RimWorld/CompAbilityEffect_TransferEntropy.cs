using System;
using Verse;

namespace RimWorld
{
	
	public class CompAbilityEffect_TransferEntropy : CompAbilityEffect
	{
		
		
		public new CompProperties_AbilityTransferEntropy Props
		{
			get
			{
				return (CompProperties_AbilityTransferEntropy)this.props;
			}
		}

		
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

		
		public override bool GizmoDisabled(out string reason)
		{
			if (this.parent.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = "AbilityNoEntropyToDump".Translate();
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		
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
