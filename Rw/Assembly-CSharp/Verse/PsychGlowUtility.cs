using System;

namespace Verse
{
	// Token: 0x02000165 RID: 357
	public static class PsychGlowUtility
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x000365C0 File Offset: 0x000347C0
		public static string GetLabel(this PsychGlow gl)
		{
			switch (gl)
			{
			case PsychGlow.Dark:
				return "Dark".Translate();
			case PsychGlow.Lit:
				return "Lit".Translate();
			case PsychGlow.Overlit:
				return "LitBrightly".Translate();
			default:
				throw new ArgumentException();
			}
		}
	}
}
