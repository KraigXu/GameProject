﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class WorldLayer_WorldObjects : WorldLayer
	{
		
		protected abstract bool ShouldSkip(WorldObject worldObject);

		
		public override IEnumerable Regenerate()
		{
			foreach (object obj in this.n__0())
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
