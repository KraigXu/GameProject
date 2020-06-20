using System;

namespace Verse
{
	// Token: 0x0200017B RID: 379
	public enum ThingRequestGroup : byte
	{
		// Token: 0x04000862 RID: 2146
		Undefined,
		// Token: 0x04000863 RID: 2147
		Nothing,
		// Token: 0x04000864 RID: 2148
		Everything,
		// Token: 0x04000865 RID: 2149
		HaulableEver,
		// Token: 0x04000866 RID: 2150
		HaulableAlways,
		// Token: 0x04000867 RID: 2151
		FoodSource,
		// Token: 0x04000868 RID: 2152
		FoodSourceNotPlantOrTree,
		// Token: 0x04000869 RID: 2153
		Corpse,
		// Token: 0x0400086A RID: 2154
		Blueprint,
		// Token: 0x0400086B RID: 2155
		BuildingArtificial,
		// Token: 0x0400086C RID: 2156
		BuildingFrame,
		// Token: 0x0400086D RID: 2157
		Pawn,
		// Token: 0x0400086E RID: 2158
		PotentialBillGiver,
		// Token: 0x0400086F RID: 2159
		Medicine,
		// Token: 0x04000870 RID: 2160
		Filth,
		// Token: 0x04000871 RID: 2161
		AttackTarget,
		// Token: 0x04000872 RID: 2162
		Weapon,
		// Token: 0x04000873 RID: 2163
		Refuelable,
		// Token: 0x04000874 RID: 2164
		HaulableEverOrMinifiable,
		// Token: 0x04000875 RID: 2165
		Drug,
		// Token: 0x04000876 RID: 2166
		Shell,
		// Token: 0x04000877 RID: 2167
		HarvestablePlant,
		// Token: 0x04000878 RID: 2168
		Fire,
		// Token: 0x04000879 RID: 2169
		Bed,
		// Token: 0x0400087A RID: 2170
		Plant,
		// Token: 0x0400087B RID: 2171
		Construction,
		// Token: 0x0400087C RID: 2172
		HasGUIOverlay,
		// Token: 0x0400087D RID: 2173
		Apparel,
		// Token: 0x0400087E RID: 2174
		MinifiedThing,
		// Token: 0x0400087F RID: 2175
		Grave,
		// Token: 0x04000880 RID: 2176
		Art,
		// Token: 0x04000881 RID: 2177
		ThingHolder,
		// Token: 0x04000882 RID: 2178
		ActiveDropPod,
		// Token: 0x04000883 RID: 2179
		Transporter,
		// Token: 0x04000884 RID: 2180
		LongRangeMineralScanner,
		// Token: 0x04000885 RID: 2181
		AffectsSky,
		// Token: 0x04000886 RID: 2182
		WindSource,
		// Token: 0x04000887 RID: 2183
		AlwaysFlee,
		// Token: 0x04000888 RID: 2184
		ResearchBench,
		// Token: 0x04000889 RID: 2185
		Facility,
		// Token: 0x0400088A RID: 2186
		AffectedByFacilities,
		// Token: 0x0400088B RID: 2187
		CreatesInfestations,
		// Token: 0x0400088C RID: 2188
		WithCustomRectForSelector,
		// Token: 0x0400088D RID: 2189
		ProjectileInterceptor,
		// Token: 0x0400088E RID: 2190
		ConditionCauser,
		// Token: 0x0400088F RID: 2191
		MusicalInstrument,
		// Token: 0x04000890 RID: 2192
		Throne,
		// Token: 0x04000891 RID: 2193
		FoodDispenser,
		// Token: 0x04000892 RID: 2194
		Projectile,
		// Token: 0x04000893 RID: 2195
		MeditationFocus
	}
}
