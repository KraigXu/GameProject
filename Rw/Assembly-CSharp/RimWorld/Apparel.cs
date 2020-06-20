using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA0 RID: 3232
	public class Apparel : ThingWithComps
	{
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06004E17 RID: 19991 RVA: 0x001A46E4 File Offset: 0x001A28E4
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x001A4708 File Offset: 0x001A2908
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06004E19 RID: 19993 RVA: 0x001A4710 File Offset: 0x001A2910
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text += "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x001A474D File Offset: 0x001A294D
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x001A4768 File Offset: 0x001A2968
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x001A4771 File Offset: 0x001A2971
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawWornExtras()
		{
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x001A478B File Offset: 0x001A298B
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x001A4794 File Offset: 0x001A2994
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__0())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			RoyalTitleDef royalTitleDef = (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.requiredApparel != null && t.requiredApparel.Any((RoyalTitleDef.ApparelRequirement req) => req.ApparelMeetsRequirement(this.def, false))
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
			if (royalTitleDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Apparel_MaxSatisfiedTitle_Desc".Translate(), 2752, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(royalTitleDef, -1)
				}, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x001A47A4 File Offset: 0x001A29A4
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		// Token: 0x04002BE3 RID: 11235
		private bool wornByCorpseInt;
	}
}
