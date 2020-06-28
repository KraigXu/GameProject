using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7E RID: 3966
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x06006085 RID: 24709 RVA: 0x00217035 File Offset: 0x00215235
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x04003910 RID: 14608
		public static RoomRoleDef None;

		// Token: 0x04003911 RID: 14609
		public static RoomRoleDef Bedroom;

		// Token: 0x04003912 RID: 14610
		public static RoomRoleDef Barracks;

		// Token: 0x04003913 RID: 14611
		public static RoomRoleDef PrisonCell;

		// Token: 0x04003914 RID: 14612
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x04003915 RID: 14613
		public static RoomRoleDef DiningRoom;

		// Token: 0x04003916 RID: 14614
		public static RoomRoleDef RecRoom;

		// Token: 0x04003917 RID: 14615
		public static RoomRoleDef Hospital;

		// Token: 0x04003918 RID: 14616
		public static RoomRoleDef Laboratory;

		// Token: 0x04003919 RID: 14617
		[MayRequireRoyalty]
		public static RoomRoleDef ThroneRoom;
	}
}
