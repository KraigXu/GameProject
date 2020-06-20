using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000106 RID: 262
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00020ADA File Offset: 0x0001ECDA
		private string RecipientName
		{
			get
			{
				if (this.recipientPawn == null)
				{
					return "null";
				}
				return this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00020AF5 File Offset: 0x0001ECF5
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00020AFE File Offset: 0x0001ECFE
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00020B1C File Offset: 0x0001ED1C
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00020B32 File Offset: 0x0001ED32
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00020B42 File Offset: 0x0001ED42
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return CameraJumper.CanJump(this.recipientPawn);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00020B54 File Offset: 0x0001ED54
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00020B66 File Offset: 0x0001ED66
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00020B6D File Offset: 0x0001ED6D
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00020B8C File Offset: 0x0001ED8C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709, false);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			return result;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00020BF0 File Offset: 0x0001EDF0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00020C2A File Offset: 0x0001EE2A
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}

		// Token: 0x04000697 RID: 1687
		private Pawn initiatorPawn;

		// Token: 0x04000698 RID: 1688
		private Pawn recipientPawn;

		// Token: 0x04000699 RID: 1689
		private RulePackDef ruleDef;
	}
}
