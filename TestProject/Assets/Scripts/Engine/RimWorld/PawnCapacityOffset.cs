﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PawnCapacityOffset
	{
		
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		
		public PawnCapacityDef capacity;

		
		public float scale = 1f;

		
		public float max = 9999f;
	}
}
