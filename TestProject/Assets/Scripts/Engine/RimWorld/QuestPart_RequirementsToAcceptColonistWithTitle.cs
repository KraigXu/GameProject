using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098B RID: 2443
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x00133450 File Offset: 0x00131650
		public override AcceptanceReport CanAccept()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
			{
				if (this.CanPawnAccept(p))
				{
					return true;
				}
			}
			return new AcceptanceReport("QuestNoColonistWithTitle".Translate(this.minimumTitle.GetLabelCapForBothGenders()));
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x001334D4 File Offset: 0x001316D4
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x00133538 File Offset: 0x00131738
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060039D8 RID: 14808 RVA: 0x00133561 File Offset: 0x00131761
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.minimumTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x04002218 RID: 8728
		public RoyalTitleDef minimumTitle;

		// Token: 0x04002219 RID: 8729
		public Faction faction;
	}
}
