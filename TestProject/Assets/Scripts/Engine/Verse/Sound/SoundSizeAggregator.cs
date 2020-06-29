﻿using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	
	public class SoundSizeAggregator
	{
		
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000DAA24 File Offset: 0x000D8C24
		public float AggregateSize
		{
			get
			{
				if (this.reporters.Count == 0)
				{
					return this.testSize;
				}
				float num = 0f;
				foreach (ISizeReporter sizeReporter in this.reporters)
				{
					num += sizeReporter.CurrentSize();
				}
				return num;
			}
		}

		
		public SoundSizeAggregator()
		{
			this.testSize = Rand.Value * 3f;
			this.testSize *= this.testSize;
		}

		
		public void RegisterReporter(ISizeReporter newRep)
		{
			this.reporters.Add(newRep);
		}

		
		public void RemoveReporter(ISizeReporter oldRep)
		{
			this.reporters.Remove(oldRep);
		}

		
		private List<ISizeReporter> reporters = new List<ISizeReporter>();

		
		private float testSize;
	}
}
