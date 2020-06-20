using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200097A RID: 2426
	public class QuestPart_Hyperlinks : QuestPart
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x00131637 File Offset: 0x0012F837
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = this.GetHyperlinks();
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x00131653 File Offset: 0x0012F853
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			if (this.thingDefs != null)
			{
				int num;
				for (int i = 0; i < this.thingDefs.Count; i = num + 1)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.thingDefs[i], -1);
					num = i;
				}
			}
			if (this.pawns != null)
			{
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					if (this.pawns[i].royalty != null && this.pawns[i].royalty.AllTitlesForReading.Any<RoyalTitle>())
					{
						RoyalTitle mostSeniorTitle = this.pawns[i].royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							yield return new Dialog_InfoCard.Hyperlink(mostSeniorTitle.def, mostSeniorTitle.faction, -1);
						}
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x00131664 File Offset: 0x0012F864
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.thingDefs, "thingDefs", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingDefs == null)
				{
					this.thingDefs = new List<ThingDef>();
				}
				this.thingDefs.RemoveAll((ThingDef x) => x == null);
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00131727 File Offset: 0x0012F927
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040021D8 RID: 8664
		public List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x040021D9 RID: 8665
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040021DA RID: 8666
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
