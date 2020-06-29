﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class JoyGiverDef : Def
	{
		
		
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
			}
			IEnumerator<string> enumerator = null;
			if (this.jobDef != null && this.jobDef.joyKind != this.joyKind)
			{
				yield return string.Concat(new object[]
				{
					"jobDef ",
					this.jobDef,
					" has joyKind ",
					this.jobDef.joyKind,
					" which does not match our joyKind ",
					this.joyKind
				});
			}
			yield break;
			yield break;
		}

		
		public Type giverClass;

		
		public float baseChance;

		
		public bool requireChair = true;

		
		public List<ThingDef> thingDefs;

		
		public JobDef jobDef;

		
		public bool desireSit = true;

		
		public float pctPawnsEverDo = 1f;

		
		public bool unroofedOnly;

		
		public JoyKindDef joyKind;

		
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		
		public bool canDoWhileInBed;

		
		private JoyGiver workerInt;
	}
}
