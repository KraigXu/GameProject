using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000869 RID: 2153
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600350A RID: 13578 RVA: 0x001227CC File Offset: 0x001209CC
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x0600350B RID: 13579 RVA: 0x001227DB File Offset: 0x001209DB
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x001227EF File Offset: 0x001209EF
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x00122828 File Offset: 0x00120A28
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.Addictive && this.chemical == null)
			{
				yield return "addictive but chemical is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001C3F RID: 7231
		public ChemicalDef chemical;

		// Token: 0x04001C40 RID: 7232
		public float addictiveness;

		// Token: 0x04001C41 RID: 7233
		public float minToleranceToAddict;

		// Token: 0x04001C42 RID: 7234
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x04001C43 RID: 7235
		public float needLevelOffset = 1f;

		// Token: 0x04001C44 RID: 7236
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x04001C45 RID: 7237
		public float largeOverdoseChance;

		// Token: 0x04001C46 RID: 7238
		public bool isCombatEnhancingDrug;

		// Token: 0x04001C47 RID: 7239
		public float listOrder;
	}
}
