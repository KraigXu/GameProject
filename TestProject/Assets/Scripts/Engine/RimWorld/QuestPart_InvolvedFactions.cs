using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_InvolvedFactions : QuestPart
	{
		
		// (get) Token: 0x06003982 RID: 14722 RVA: 0x00131A59 File Offset: 0x0012FC59
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__0())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				foreach (Faction faction2 in this.factions)
				{
					yield return faction2;
				}
				List<Faction>.Enumerator enumerator2 = default(List<Faction>.Enumerator);
				yield break;
				yield break;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		
		public List<Faction> factions = new List<Faction>();
	}
}
