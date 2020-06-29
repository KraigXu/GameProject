using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		
		public override void MentalStateTick()
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
				return;
			}
			if (this.target.Spawned && this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				base.MentalStateTick();
				return;
			}
			Thing target = this.target;
			if (!this.TryFindNewTarget())
			{
				base.RecoverFromState();
				return;
			}
			Messages.Message("MessageTargetedTantrumChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			base.MentalStateTick();
		}

		
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		
		public override string GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored.Resolve(), this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
		}

		
		public const int MinMarketValue = 300;

		
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
