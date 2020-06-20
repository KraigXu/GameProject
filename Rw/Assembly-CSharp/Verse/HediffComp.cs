using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200023D RID: 573
	public class HediffComp
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0005CD3A File Offset: 0x0005AF3A
		public Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x0005CD47 File Offset: 0x0005AF47
		public HediffDef Def
		{
			get
			{
				return this.parent.def;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompLabelInBracketsExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompTipStringExtra
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0005ACED File Offset: 0x00058EED
		public virtual TextureAndColor CompStateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CompShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostMake()
		{
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostTick(ref float severityAdjustment)
		{
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompExposeData()
		{
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostPostRemoved()
		{
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostMerged(Hediff other)
		{
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool CompDisallowVisible()
		{
			return false;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPostInjuryHeal(float amount)
		{
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompTended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompDebugString()
		{
			return null;
		}

		// Token: 0x04000BD1 RID: 3025
		public HediffWithComps parent;

		// Token: 0x04000BD2 RID: 3026
		public HediffCompProperties props;
	}
}
