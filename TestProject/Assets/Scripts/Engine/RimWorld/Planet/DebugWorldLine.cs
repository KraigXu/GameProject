﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class DebugWorldLine
	{
		
		// (get) Token: 0x060069F7 RID: 27127 RVA: 0x0024FDD4 File Offset: 0x0024DFD4
		// (set) Token: 0x060069F8 RID: 27128 RVA: 0x0024FDDC File Offset: 0x0024DFDC
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num < 0.001f)
			{
				return;
			}
			if (this.onPlanetSurface)
			{
				float averageTileSize = Find.WorldGrid.averageTileSize;
				int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
				float num3 = 0.05f;
				for (int i = 0; i < num2; i++)
				{
					Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
					Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
					vector = vector.normalized * (100f + num3);
					vector2 = vector2.normalized * (100f + num3);
					GenDraw.DrawWorldLineBetween(vector, vector2);
				}
				return;
			}
			GenDraw.DrawWorldLineBetween(this.a, this.b);
		}

		
		public Vector3 a;

		
		public Vector3 b;

		
		public int ticksLeft;

		
		private bool onPlanetSurface;
	}
}
