using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	
	public class HediffComp_Link : HediffComp
	{
		
		
		public HediffCompProperties_Link Props
		{
			get
			{
				return (HediffCompProperties_Link)this.props;
			}
		}

		
		
		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				if (this.other == null || !this.parent.pawn.Spawned || !this.other.Spawned)
				{
					return true;
				}
				if (this.Props.maxDistance > 0f && !this.parent.pawn.Position.InHorDistOf(this.other.Position, this.Props.maxDistance))
				{
					return true;
				}
				foreach (Hediff hediff in this.other.health.hediffSet.hediffs)
				{
					HediffWithComps hediffWithComps = hediff as HediffWithComps;
					if (hediffWithComps != null && hediffWithComps.comps.FirstOrDefault(delegate(HediffComp c)
					{
						HediffComp_Link hediffComp_Link = c as HediffComp_Link;
						return hediffComp_Link != null && hediffComp_Link.other == this.parent.pawn && hediffComp_Link.parent.def == this.parent.def;
					}) != null)
					{
						return false;
					}
				}
				return true;
			}
		}

		
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.drawConnection)
			{
				if (this.mote == null)
				{
					this.mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkLine, this.parent.pawn, this.other);
				}
				this.mote.Maintain();
			}
		}

		
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Pawn>(ref this.other, "other", false);
		}

		
		
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showName || this.other == null)
				{
					return null;
				}
				return this.other.LabelShort;
			}
		}

		
		public Pawn other;

		
		private MoteDualAttached mote;

		
		public bool drawConnection;
	}
}
