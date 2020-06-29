using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Hyperlinks : QuestPart
	{
		
		
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

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		
		public List<ThingDef> thingDefs = new List<ThingDef>();

		
		public List<Pawn> pawns = new List<Pawn>();

		
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
