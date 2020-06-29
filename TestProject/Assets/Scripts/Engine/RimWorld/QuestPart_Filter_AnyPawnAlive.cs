using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Filter_AnyPawnAlive : QuestPart_Filter
	{
		
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator();
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Destroyed)
					{
						return true;
					}
				}
			}
			return false;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public List<Pawn> pawns;
	}
}
