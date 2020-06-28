using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFC RID: 3836
	[StaticConstructorOnStartup]
	public static class TexCommand
	{
		// Token: 0x04003303 RID: 13059
		public static readonly Texture2D DesirePower = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true);

		// Token: 0x04003304 RID: 13060
		public static readonly Texture2D Draft = ContentFinder<Texture2D>.Get("UI/Commands/Draft", true);

		// Token: 0x04003305 RID: 13061
		public static readonly Texture2D ReleaseAnimals = ContentFinder<Texture2D>.Get("UI/Commands/ReleaseAnimals", true);

		// Token: 0x04003306 RID: 13062
		public static readonly Texture2D HoldOpen = ContentFinder<Texture2D>.Get("UI/Commands/HoldOpen", true);

		// Token: 0x04003307 RID: 13063
		public static readonly Texture2D GatherSpotActive = ContentFinder<Texture2D>.Get("UI/Commands/GatherSpotActive", true);

		// Token: 0x04003308 RID: 13064
		public static readonly Texture2D Install = ContentFinder<Texture2D>.Get("UI/Commands/Install", true);

		// Token: 0x04003309 RID: 13065
		public static readonly Texture2D SquadAttack = ContentFinder<Texture2D>.Get("UI/Commands/SquadAttack", true);

		// Token: 0x0400330A RID: 13066
		public static readonly Texture2D AttackMelee = ContentFinder<Texture2D>.Get("UI/Commands/AttackMelee", true);

		// Token: 0x0400330B RID: 13067
		public static readonly Texture2D Attack = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);

		// Token: 0x0400330C RID: 13068
		public static readonly Texture2D FireAtWill = ContentFinder<Texture2D>.Get("UI/Commands/FireAtWill", true);

		// Token: 0x0400330D RID: 13069
		public static readonly Texture2D ToggleVent = ContentFinder<Texture2D>.Get("UI/Commands/Vent", true);

		// Token: 0x0400330E RID: 13070
		public static readonly Texture2D PauseCaravan = ContentFinder<Texture2D>.Get("UI/Commands/PauseCaravan", true);

		// Token: 0x0400330F RID: 13071
		public static readonly Texture2D ForbidOff = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);

		// Token: 0x04003310 RID: 13072
		public static readonly Texture2D ForbidOn = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);

		// Token: 0x04003311 RID: 13073
		public static readonly Texture2D RearmTrap = ContentFinder<Texture2D>.Get("UI/Designators/RearmTrap", true);

		// Token: 0x04003312 RID: 13074
		public static readonly Texture2D CannotShoot = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003313 RID: 13075
		public static readonly Texture2D ClearPrioritizedWork = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003314 RID: 13076
		public static readonly Texture2D RemoveRoutePlannerWaypoint = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04003315 RID: 13077
		public static readonly Texture2D OpenLinkedQuestTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
