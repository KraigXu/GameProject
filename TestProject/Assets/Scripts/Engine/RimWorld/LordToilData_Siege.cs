using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007A0 RID: 1952
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x060032DA RID: 13018 RVA: 0x0011AD6C File Offset: 0x00118F6C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.siegeCenter, "siegeCenter", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.baseRadius, "baseRadius", 16f, false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.desiredBuilderFraction, "desiredBuilderFraction", 0.5f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			}
			Scribe_Collections.Look<Blueprint>(ref this.blueprints, "blueprints", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04001B6E RID: 7022
		public IntVec3 siegeCenter;

		// Token: 0x04001B6F RID: 7023
		public float baseRadius = 16f;

		// Token: 0x04001B70 RID: 7024
		public float blueprintPoints;

		// Token: 0x04001B71 RID: 7025
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x04001B72 RID: 7026
		public List<Blueprint> blueprints = new List<Blueprint>();
	}
}
