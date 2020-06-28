using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200010A RID: 266
	public class BattleLogEntry_RangedFire : LogEntry
	{
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00021878 File Offset: 0x0001FA78
		private string InitiatorName
		{
			get
			{
				if (this.initiatorPawn == null)
				{
					return "null";
				}
				return this.initiatorPawn.LabelShort;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x00021893 File Offset: 0x0001FA93
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

		// Token: 0x06000750 RID: 1872 RVA: 0x00020C57 File Offset: 0x0001EE57
		public BattleLogEntry_RangedFire() : base(null)
		{
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000218B0 File Offset: 0x0001FAB0
		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst) : base(null)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
			this.burst = burst;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00021925 File Offset: 0x0001FB25
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002193B File Offset: 0x0001FB3B
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

		// Token: 0x06000754 RID: 1876 RVA: 0x0002194C File Offset: 0x0001FB4C
		public override bool CanBeClickedFromPOV(Thing pov)
		{
			return this.recipientPawn != null && ((pov == this.initiatorPawn && CameraJumper.CanJump(this.recipientPawn)) || (pov == this.recipientPawn && CameraJumper.CanJump(this.initiatorPawn)));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002199C File Offset: 0x0001FB9C
		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn == null)
			{
				return;
			}
			if (pov == this.initiatorPawn)
			{
				CameraJumper.TryJumpAndSelect(this.recipientPawn);
				return;
			}
			if (pov == this.recipientPawn)
			{
				CameraJumper.TryJumpAndSelect(this.initiatorPawn);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000219EC File Offset: 0x0001FBEC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709, false);
			}
			if (this.weaponDef != null && this.weaponDef.Verbs[0].rangedFireRulepack != null)
			{
				result.Includes.Add(this.weaponDef.Verbs[0].rangedFireRulepack);
			}
			else
			{
				result.Includes.Add(RulePackDefOf.Combat_RangedFire);
			}
			if (this.initiatorPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiatorPawn, result.Constants, true, true));
			}
			else if (this.initiatorThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", this.initiatorThing));
			}
			else
			{
				result.Constants["INITIATOR_missing"] = "True";
			}
			if (this.recipientPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants, true, true));
			}
			else if (this.recipientThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", this.recipientThing));
			}
			else
			{
				result.Constants["RECIPIENT_missing"] = "True";
			}
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", this.weaponDef, this.projectileDef));
			if (this.initiatorPawn != null && this.initiatorPawn.skills != null)
			{
				result.Constants["INITIATOR_skill"] = this.initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			if (this.recipientPawn != null && this.recipientPawn.skills != null)
			{
				result.Constants["RECIPIENT_skill"] = this.recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
			}
			result.Constants["BURST"] = this.burst.ToString();
			return result;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00021C14 File Offset: 0x0001FE14
		public override bool ShowInCompactView()
		{
			return Rand.ChanceSeeded(BattleLogEntry_RangedFire.DisplayChance, this.logID);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00021C28 File Offset: 0x0001FE28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Values.Look<bool>(ref this.burst, "burst", false, false);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00021CAF File Offset: 0x0001FEAF
		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}

		// Token: 0x040006B0 RID: 1712
		private Pawn initiatorPawn;

		// Token: 0x040006B1 RID: 1713
		private ThingDef initiatorThing;

		// Token: 0x040006B2 RID: 1714
		private Pawn recipientPawn;

		// Token: 0x040006B3 RID: 1715
		private ThingDef recipientThing;

		// Token: 0x040006B4 RID: 1716
		private ThingDef weaponDef;

		// Token: 0x040006B5 RID: 1717
		private ThingDef projectileDef;

		// Token: 0x040006B6 RID: 1718
		private bool burst;

		// Token: 0x040006B7 RID: 1719
		[TweakValue("LogFilter", 0f, 1f)]
		private static float DisplayChance = 0.25f;
	}
}
