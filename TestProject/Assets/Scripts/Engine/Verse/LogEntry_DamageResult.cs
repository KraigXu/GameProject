using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000115 RID: 277
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x00024201 File Offset: 0x00022401
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002420A File Offset: 0x0002240A
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00024228 File Offset: 0x00022428
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00024288 File Offset: 0x00022488
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.damagedParts != null)
			{
				for (int i = this.damagedParts.Count - 1; i >= 0; i--)
				{
					if (this.damagedParts[i] == null)
					{
						this.damagedParts.RemoveAt(i);
						if (i < this.damagedPartsDestroyed.Count)
						{
							this.damagedPartsDestroyed.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x04000705 RID: 1797
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04000706 RID: 1798
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04000707 RID: 1799
		protected bool deflected;
	}
}
