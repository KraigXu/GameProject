using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		
		// (get) Token: 0x06005B1F RID: 23327 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06005B20 RID: 23328 RVA: 0x001F5EF4 File Offset: 0x001F40F4
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.chosenPawnSignal.NullOrEmpty())
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull())
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		
		// (get) Token: 0x06005B21 RID: 23329 RVA: 0x001F5F4A File Offset: 0x001F414A
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					int num;
					for (int i = 0; i < this.pawns.Count; i = num + 1)
					{
						if (!this.pawns[i].DestroyedOrNull())
						{
							yield return this.Option_ChoosePawn(this.pawns[i]);
						}
						num = i;
					}
					yield return base.Option_Postpone;
				}
				else
				{
					yield return base.Option_Close;
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocationAndPostpone;
				}
				if (this.quest != null)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", true);
				}
				yield break;
			}
		}

		
		private DiaOption Option_ChoosePawn(Pawn p)
		{
			return new DiaOption(p.LabelCap)
			{
				action = delegate
				{
					if (!this.chosenPawnSignal.NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.chosenPawnSignal, p.Named("CHOSEN")));
					}
					Find.LetterStack.RemoveLetter(this);
				},
				resolveTree = true
			};
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public string chosenPawnSignal;
	}
}
