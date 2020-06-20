using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000873 RID: 2163
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x00122EED File Offset: 0x001210ED
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x00122F1B File Offset: 0x0012111B
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
			yield break;
			yield break;
		}

		// Token: 0x04001C7B RID: 7291
		public float width = 8f;

		// Token: 0x04001C7C RID: 7292
		public Color color = Color.white;

		// Token: 0x04001C7D RID: 7293
		public SoundDef sound;
	}
}
