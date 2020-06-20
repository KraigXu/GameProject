using System;

namespace Verse
{
	// Token: 0x02000470 RID: 1136
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060021A4 RID: 8612 RVA: 0x000CCEF4 File Offset: 0x000CB0F4
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return "None".Translate();
			case AnimalNameDisplayMode.TameNamed:
				return "AnimalNameDisplayMode_TameNamed".Translate();
			case AnimalNameDisplayMode.TameAll:
				return "AnimalNameDisplayMode_TameAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
