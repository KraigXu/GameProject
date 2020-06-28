using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000BF RID: 191
	public class JobDef : Def
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x0001B894 File Offset: 0x00019A94
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.joySkill != null && this.joyXpPerTick == 0f)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400040E RID: 1038
		public Type driverClass;

		// Token: 0x0400040F RID: 1039
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x04000410 RID: 1040
		public bool playerInterruptible = true;

		// Token: 0x04000411 RID: 1041
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x04000412 RID: 1042
		public bool alwaysShowWeapon;

		// Token: 0x04000413 RID: 1043
		public bool neverShowWeapon;

		// Token: 0x04000414 RID: 1044
		public bool suspendable = true;

		// Token: 0x04000415 RID: 1045
		public bool casualInterruptible = true;

		// Token: 0x04000416 RID: 1046
		public bool allowOpportunisticPrefix;

		// Token: 0x04000417 RID: 1047
		public bool collideWithPawns;

		// Token: 0x04000418 RID: 1048
		public bool isIdle;

		// Token: 0x04000419 RID: 1049
		public TaleDef taleOnCompletion;

		// Token: 0x0400041A RID: 1050
		public bool neverFleeFromEnemies;

		// Token: 0x0400041B RID: 1051
		public bool makeTargetPrisoner;

		// Token: 0x0400041C RID: 1052
		public int waitAfterArriving;

		// Token: 0x0400041D RID: 1053
		public int joyDuration = 4000;

		// Token: 0x0400041E RID: 1054
		public int joyMaxParticipants = 1;

		// Token: 0x0400041F RID: 1055
		public float joyGainRate = 1f;

		// Token: 0x04000420 RID: 1056
		public SkillDef joySkill;

		// Token: 0x04000421 RID: 1057
		public float joyXpPerTick;

		// Token: 0x04000422 RID: 1058
		public JoyKindDef joyKind;

		// Token: 0x04000423 RID: 1059
		public Rot4 faceDir = Rot4.Invalid;
	}
}
