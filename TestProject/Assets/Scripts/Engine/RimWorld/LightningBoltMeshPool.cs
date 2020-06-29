using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class LightningBoltMeshPool
	{
		
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

		
		private static List<Mesh> boltMeshes = new List<Mesh>();

		
		private const int NumBoltMeshesMax = 20;
	}
}
