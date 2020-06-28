using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000530 RID: 1328
	public static class Toils_Effects
	{
		// Token: 0x06002617 RID: 9751 RVA: 0x000E10BC File Offset: 0x000DF2BC
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}
	}
}
