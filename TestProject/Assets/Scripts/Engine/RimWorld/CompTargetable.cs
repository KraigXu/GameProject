using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class CompTargetable : CompUseEffect
	{
		
		// (get) Token: 0x06005474 RID: 21620 RVA: 0x001C317E File Offset: 0x001C137E
		public CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		
		// (get) Token: 0x06005475 RID: 21621
		protected abstract bool PlayerChoosesTarget { get; }

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		
		public override bool SelectedUseOption(Pawn p)
		{
			if (this.PlayerChoosesTarget)
			{
				Find.Targeter.BeginTargeting(this.GetTargetingParameters(), delegate(LocalTargetInfo t)
				{
					this.target = t.Thing;
					this.parent.GetComp<CompUsable>().TryStartUseJob(p, this.target);
				}, p, null, null);
				return true;
			}
			this.target = null;
			return false;
		}

		
		public override void DoEffect(Pawn usedBy)
		{
			if (this.PlayerChoosesTarget && this.target == null)
			{
				return;
			}
			if (this.target != null && !this.GetTargetingParameters().CanTarget(this.target))
			{
				return;
			}
			base.DoEffect(usedBy);
			foreach (Thing thing in this.GetTargets(this.target))
			{
				foreach (CompTargetEffect compTargetEffect in this.parent.GetComps<CompTargetEffect>())
				{
					compTargetEffect.DoEffectOn(usedBy, thing);
				}
				if (this.Props.moteOnTarget != null)
				{
					MoteMaker.MakeAttachedOverlay(thing, this.Props.moteOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.moteConnecting != null)
				{
					MoteMaker.MakeConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.moteConnecting, usedBy.Map, 1f);
				}
			}
			this.target = null;
		}

		
		protected abstract TargetingParameters GetTargetingParameters();

		
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		
		public bool BaseTargetValidator(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.Props.psychicSensitiveTargetsOnly && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) <= 0f)
				{
					return false;
				}
				if (this.Props.ignoreQuestLodgerPawns && pawn.IsQuestLodger())
				{
					return false;
				}
				if (this.Props.ignorePlayerFactionPawns && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = t as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					return false;
				}
			}
			if (this.Props.nonDessicatedCorpsesOnly)
			{
				Corpse corpse2 = t as Corpse;
				if (corpse2 != null && corpse2.GetRotStage() == RotStage.Dessicated)
				{
					return false;
				}
			}
			return true;
		}

		
		private Thing target;
	}
}
