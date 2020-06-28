using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097D RID: 2429
	public class QuestPart_InvolvedFactions : QuestPart
	{
		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003982 RID: 14722 RVA: 0x00131A59 File Offset: 0x0012FC59
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__0())
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

		// Token: 0x06003983 RID: 14723 RVA: 0x00131A69 File Offset: 0x0012FC69
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040021E5 RID: 8677
		public List<Faction> factions = new List<Faction>();
	}
}
