using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011F4 RID: 4596
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		// Token: 0x06006A4E RID: 27214
		protected abstract bool ShouldSkip(WorldObject worldObject);

		// Token: 0x06006A4F RID: 27215 RVA: 0x0025108A File Offset: 0x0024F28A
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.<>n__0())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (!worldObject.def.useDynamicDrawer && !this.ShouldSkip(worldObject))
				{
					Material material = worldObject.Material;
					if (material == null)
					{
						Log.ErrorOnce("World object " + worldObject + " returned null material.", Gen.HashCombineInt(1948576891, worldObject.ID), false);
					}
					else
					{
						LayerSubMesh subMesh = base.GetSubMesh(material);
						Rand.PushState();
						Rand.Seed = worldObject.ID;
						worldObject.Print(subMesh);
						Rand.PopState();
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}
	}
}
