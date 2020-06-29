using System;
using RimWorld;

namespace Verse
{
	
	public class HediffComp
	{
		
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0005CD3A File Offset: 0x0005AF3A
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x0005CD47 File Offset: 0x0005AF47
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0005ACED File Offset: 0x00058EED
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		
		public virtual void CompPostMake()
		{
		}

		
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		
		public virtual void CompExposeData()
		{
		}

		
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		
		public virtual void CompPostPostRemoved()
		{
		}

		
		public virtual void CompPostMerged(Hediff other)
		{
		}

		
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		
		public virtual void Notify_PawnDied()
		{
		}

		
		public virtual void Notify_PawnKilled()
		{
		}

		
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		
		public virtual string CompDebugString()
		{
			return null;
		}

		
		public HediffWithComps parent;

		
		public HediffCompProperties props;
	}
}
