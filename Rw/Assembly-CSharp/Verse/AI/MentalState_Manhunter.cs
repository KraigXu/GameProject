using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000553 RID: 1363
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x060026E9 RID: 9961 RVA: 0x000E47EE File Offset: 0x000E29EE
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000E4802 File Offset: 0x000E2A02
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000E481A File Offset: 0x000E2A1A
		public override bool ForceHostileTo(Faction f)
		{
			return f.def.humanlikeFaction || f == Faction.OfMechanoids;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x00010306 File Offset: 0x0000E506
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
