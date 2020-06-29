using System;
using Verse;

namespace RimWorld.Planet
{
	
	public abstract class WorldComponent : IExposable
	{
		
		public WorldComponent(World world)
		{
			this.world = world;
		}

		
		public virtual void WorldComponentUpdate()
		{
		}

		
		public virtual void WorldComponentTick()
		{
		}

		
		public virtual void ExposeData()
		{
		}

		
		public virtual void FinalizeInit()
		{
		}

		
		public World world;
	}
}
