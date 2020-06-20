using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000086 RID: 134
	public class CompProperties
	{
		// Token: 0x060004BB RID: 1211 RVA: 0x00017D5B File Offset: 0x00015F5B
		public CompProperties()
		{
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00017D73 File Offset: 0x00015F73
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00017D92 File Offset: 0x00015F92
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00017DA9 File Offset: 0x00015FA9
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x0400021A RID: 538
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
