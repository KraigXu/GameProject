using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000109 RID: 265
	public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x000212AE File Offset: 0x0001F4AE
		private string InitiatorName
		{
			get
			{
				if (this.initiator == null)
				{
					return "null";
				}
				return this.initiator.LabelShort;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x000212C9 File Offset: 0x0001F4C9
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

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x000212E4 File Offset: 0x0001F4E4
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x000212EC File Offset: 0x0001F4EC
		public RulePackDef RuleDef
		{
			get
			{
				return this.ruleDef;
			}
			set
			{
				this.ruleDef = value;
				base.ResetCache();
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00020AF5 File Offset: 0x0001ECF5
		public BattleLogEntry_MeleeCombat() : base(null)
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000212FC File Offset: 0x0001F4FC
		public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null) : base(def)
		{
			this.ruleDef = ruleDef;
			this.alwaysShowInCompact = alwaysShowInCompact;
			this.initiator = initiator;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669, false);
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002139A File Offset: 0x0001F59A
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipientPawn;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000213B0 File Offset: 0x0001F5B0
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000213C0 File Offset: 0x0001F5C0
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return (pov == this.initiator && this.recipientPawn != null && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiator));
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x00021410 File Offset: 0x0001F610
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator && this.recipientPawn != null)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiator);
				return;
			}
			if (this.recipientPawn != null)
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00021468 File Offset: 0x0001F668
		public override Texture2D IconFromPOV(Thing pov)
		{
			if (this.damagedParts.NullOrEmpty<BodyPartRecord>())
			{
				return this.def.iconMissTex;
			}
			if (this.deflected)
			{
				return this.def.iconMissTex;
			}
			if (pov == null || pov == this.recipientPawn)
			{
				return this.def.iconDamagedTex;
			}
			if (pov == this.initiator)
			{
				return this.def.iconDamagedFromInstigatorTex;
			}
			return this.def.iconDamagedTex;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000214DA File Offset: 0x0001F6DA
		protected override BodyDef DamagedBody()
		{
			if (this.recipientPawn == null)
			{
				return null;
			}
			return this.recipientPawn.RaceProps.body;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000214F8 File Offset: 0x0001F6F8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, result.Constants, true, true));
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			result.Includes.Add(this.ruleDef);
			if (!this.toolLabel.NullOrEmpty())
			{
				result.Rules.Add(new Rule_String("TOOL_label", this.toolLabel));
				result.Rules.Add(new Rule_String("TOOL_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.toolLabel, false, false)));
				result.Rules.Add(new Rule_String("TOOL_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.toolLabel, false, false)));
				result.Constants["TOOL_gender"] = LanguageDatabase.activeLanguage.ResolveGender(this.toolLabel, null).ToString();
			}
			if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
			{
				if (this.ownerEquipmentDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
				}
				else if (this.ownerHediffDef != null)
				{
					result.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
				}
			}
			if (this.initiator != null && this.initiator.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiator.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
			}
			if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
			{
				result.Constants["IMPLEMENTOWNER_type"] = this.implementType.implementOwnerTypeValue;
			}
			return result;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00021773 File Offset: 0x0001F973
		public override bool ShowInCompactView()
		{
			return this.alwaysShowInCompact || Rand.ChanceSeeded(BattleLogEntry_MeleeCombat.DisplayChanceOnMiss, this.logID);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00021790 File Offset: 0x0001F990
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Values.Look<bool>(ref this.alwaysShowInCompact, "alwaysShowInCompact", false, false);
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Values.Look<string>(ref this.toolLabel, "toolLabel", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002182F File Offset: 0x0001FA2F
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.ruleDef.defName,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x040006A6 RID: 1702
		private RulePackDef ruleDef;

		// Token: 0x040006A7 RID: 1703
		private Pawn initiator;

		// Token: 0x040006A8 RID: 1704
		private Pawn recipientPawn;

		// Token: 0x040006A9 RID: 1705
		private ThingDef recipientThing;

		// Token: 0x040006AA RID: 1706
		private ImplementOwnerTypeDef implementType;

		// Token: 0x040006AB RID: 1707
		private ThingDef ownerEquipmentDef;

		// Token: 0x040006AC RID: 1708
		private HediffDef ownerHediffDef;

		// Token: 0x040006AD RID: 1709
		private string toolLabel;

		// Token: 0x040006AE RID: 1710
		public bool alwaysShowInCompact;

		// Token: 0x040006AF RID: 1711
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChanceOnMiss = 0.5f;
	}
}
