using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Apparel : ThingWithComps
	{
		
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

		
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x001A4708 File Offset: 0x001A2908
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
		}

		
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

		
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
		}

		
		public void Notify_PawnResurrected()
		{
			this.wornByCorpseInt = false;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
		}

		
		public virtual void DrawWornExtras()
		{
		}

		
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			return false;
		}

		
		public virtual bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return true;
		}

		
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			yield break;
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{

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

		
		public virtual float GetSpecialApparelScoreOffset()
		{
			return 0f;
		}

		
		private bool wornByCorpseInt;
	}
}
