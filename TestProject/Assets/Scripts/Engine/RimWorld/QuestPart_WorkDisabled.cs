using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x0012F5DA File Offset: 0x0012D7DA
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (base.State == QuestPartState.Enabled)
				{
					List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
						{
							yield return list[i];
						}
						num = i;
					}
					list = null;
				}
				yield break;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		
		private void ClearPawnWorkTypesAndSkillsCache()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i] != null)
				{
					this.pawns[i].Notify_DisabledWorkTypesChanged();
					if (this.pawns[i].skills != null)
					{
						this.pawns[i].skills.Notify_SkillDisablesChanged();
					}
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<WorkTags>(ref this.disabledWorkTags, "disabledWorkTags", WorkTags.None, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public List<Pawn> pawns = new List<Pawn>();

		
		public WorkTags disabledWorkTags;
	}
}
