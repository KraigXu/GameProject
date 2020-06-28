using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAC RID: 2732
	public static class LightningBoltMeshPool
	{
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x0015A998 File Offset: 0x00158B98
		public static Mesh RandomBoltMesh
		{
			get
			{
				if (LightningBoltMeshPool.boltMeshes.Count < 20)
				{
					Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
					LightningBoltMeshPool.boltMeshes.Add(mesh);
					return mesh;
				}
				return LightningBoltMeshPool.boltMeshes.RandomElement<Mesh>();
			}
		}

		// Token: 0x0400258A RID: 9610
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x0400258B RID: 9611
		private const int NumBoltMeshesMax = 20;
	}
}
