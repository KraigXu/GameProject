using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		
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

		
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		
		// (get) Token: 0x060039D8 RID: 14808 RVA: 0x00133561 File Offset: 0x00131761
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.minimumTitle, this.faction, -1);
				yield break;
			}
		}

		
		public RoyalTitleDef minimumTitle;

		
		public Faction faction;
	}
}
