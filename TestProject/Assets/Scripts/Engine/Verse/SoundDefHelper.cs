using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000EC RID: 236
	public static class SoundDefHelper
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x0001E255 File Offset: 0x0001C455
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001E264 File Offset: 0x0001C464
		public static bool CorrectContextNow(SoundDef def, Map sourceMap)
		{
			if (sourceMap != null && (Find.CurrentMap != sourceMap || WorldRendererUtility.WorldRenderedNow))
			{
				return false;
			}
			switch (def.context)
			{
			case SoundContext.Any:
				return true;
			case SoundContext.MapOnly:
				return Current.ProgramState == ProgramState.Playing && !WorldRendererUtility.WorldRenderedNow;
			case SoundContext.WorldOnly:
				return WorldRendererUtility.WorldRenderedNow;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
