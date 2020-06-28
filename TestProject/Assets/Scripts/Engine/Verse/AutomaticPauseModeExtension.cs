using System;

namespace Verse
{
	// Token: 0x0200046E RID: 1134
	public static class AutomaticPauseModeExtension
	{
		// Token: 0x060021A3 RID: 8611 RVA: 0x000CCE88 File Offset: 0x000CB088
		public static string ToStringHuman(this AutomaticPauseMode mode)
		{
			switch (mode)
			{
			case AutomaticPauseMode.Never:
				return "AutomaticPauseMode_Never".Translate();
			case AutomaticPauseMode.MajorThreat:
				return "AutomaticPauseMode_MajorThreat".Translate();
			case AutomaticPauseMode.AnyThreat:
				return "AutomaticPauseMode_AnyThreat".Translate();
			case AutomaticPauseMode.AnyLetter:
				return "AutomaticPauseMode_AnyLetter".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
