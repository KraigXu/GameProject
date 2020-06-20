using System;

namespace RimWorld
{
	// Token: 0x0200088E RID: 2190
	[Flags]
	public enum FoodTypeFlags
	{
		// Token: 0x04001CD3 RID: 7379
		None = 0,
		// Token: 0x04001CD4 RID: 7380
		VegetableOrFruit = 1,
		// Token: 0x04001CD5 RID: 7381
		Meat = 2,
		// Token: 0x04001CD6 RID: 7382
		Fluid = 4,
		// Token: 0x04001CD7 RID: 7383
		Corpse = 8,
		// Token: 0x04001CD8 RID: 7384
		Seed = 16,
		// Token: 0x04001CD9 RID: 7385
		AnimalProduct = 32,
		// Token: 0x04001CDA RID: 7386
		Plant = 64,
		// Token: 0x04001CDB RID: 7387
		Tree = 128,
		// Token: 0x04001CDC RID: 7388
		Meal = 256,
		// Token: 0x04001CDD RID: 7389
		Processed = 512,
		// Token: 0x04001CDE RID: 7390
		Liquor = 1024,
		// Token: 0x04001CDF RID: 7391
		Kibble = 2048,
		// Token: 0x04001CE0 RID: 7392
		VegetarianAnimal = 3857,
		// Token: 0x04001CE1 RID: 7393
		VegetarianRoughAnimal = 3921,
		// Token: 0x04001CE2 RID: 7394
		CarnivoreAnimal = 2826,
		// Token: 0x04001CE3 RID: 7395
		CarnivoreAnimalStrict = 10,
		// Token: 0x04001CE4 RID: 7396
		OmnivoreAnimal = 3867,
		// Token: 0x04001CE5 RID: 7397
		OmnivoreRoughAnimal = 3931,
		// Token: 0x04001CE6 RID: 7398
		DendrovoreAnimal = 2705,
		// Token: 0x04001CE7 RID: 7399
		OvivoreAnimal = 2848,
		// Token: 0x04001CE8 RID: 7400
		OmnivoreHuman = 3903
	}
}
