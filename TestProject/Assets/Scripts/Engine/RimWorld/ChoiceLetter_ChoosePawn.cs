using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E99 RID: 3737
	public class ChoiceLetter_ChoosePawn : ChoiceLetter
	{
		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06005B1F RID: 23327 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001057 RID: 4183
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

		// Token: 0x17001058 RID: 4184
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

		// Token: 0x06005B22 RID: 23330 RVA: 0x001F5F5C File Offset: 0x001F415C
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

		// Token: 0x06005B23 RID: 23331 RVA: 0x001F5FA8 File Offset: 0x001F41A8
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

		// Token: 0x040031C3 RID: 12739
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040031C4 RID: 12740
		public string chosenPawnSignal;
	}
}
