using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200009A RID: 154
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x060004F3 RID: 1267 RVA: 0x00019090 File Offset: 0x00017290
		public Texture2D GetIcon(Pawn forPawn)
		{
			if (this.def.iconTex != null)
			{
				return this.def.iconTex;
			}
			int count = forPawn.RaceProps.lifeStageAges.Count;
			int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
			if (num == count - 1)
			{
				return LifeStageAge.AdultIcon;
			}
			if (num == 0)
			{
				return LifeStageAge.VeryYoungIcon;
			}
			return LifeStageAge.YoungIcon;
		}

		// Token: 0x040002AA RID: 682
		public LifeStageDef def;

		// Token: 0x040002AB RID: 683
		public float minAge;

		// Token: 0x040002AC RID: 684
		public SoundDef soundCall;

		// Token: 0x040002AD RID: 685
		public SoundDef soundAngry;

		// Token: 0x040002AE RID: 686
		public SoundDef soundWounded;

		// Token: 0x040002AF RID: 687
		public SoundDef soundDeath;

		// Token: 0x040002B0 RID: 688
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x040002B1 RID: 689
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x040002B2 RID: 690
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}
