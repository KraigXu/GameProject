using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000961 RID: 2401
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		// Token: 0x17000A30 RID: 2608
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

		// Token: 0x060038DA RID: 14554 RVA: 0x0012F5EA File Offset: 0x0012D7EA
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0012F5F9 File Offset: 0x0012D7F9
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x0012F608 File Offset: 0x0012D808
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

		// Token: 0x060038DD RID: 14557 RVA: 0x0012F674 File Offset: 0x0012D874
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

		// Token: 0x04002187 RID: 8583
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002188 RID: 8584
		public WorkTags disabledWorkTags;
	}
}
