﻿using System;
using UnityEngine;

namespace Verse
{
	
	internal struct DebugLine
	{
		
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x000347AC File Offset: 0x000329AC
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		
		public Vector3 a;

		
		public Vector3 b;

		
		private int deathTick;

		
		private SimpleColor color;
	}
}
