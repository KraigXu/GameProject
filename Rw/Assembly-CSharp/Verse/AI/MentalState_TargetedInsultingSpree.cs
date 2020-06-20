using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000548 RID: 1352
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x000E3984 File Offset: 0x000E1B84
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060026A7 RID: 9895 RVA: 0x000E39A1 File Offset: 0x000E1BA1
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000E39AC File Offset: 0x000E1BAC
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || !this.target.Awake()))
			{
				Pawn target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick();
				return;
			}
			else
			{
				if (this.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, false))
				{
					base.RecoverFromState();
					return;
				}
				base.MentalStateTick();
				return;
			}
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000E3AD3 File Offset: 0x000E1CD3
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000E3AE2 File Offset: 0x000E1CE2
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000E3B10 File Offset: 0x000E1D10
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000E3BA0 File Offset: 0x000E1DA0
		public override string GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored.Resolve(), this.target.NameShortColored.Resolve(), this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
		}

		// Token: 0x04001739 RID: 5945
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
