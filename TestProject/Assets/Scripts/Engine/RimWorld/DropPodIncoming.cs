using System;
using Verse;

namespace RimWorld
{
	
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		
		// (get) Token: 0x06004F02 RID: 20226 RVA: 0x001A993D File Offset: 0x001A7B3D
		// (set) Token: 0x06004F03 RID: 20227 RVA: 0x001A9955 File Offset: 0x001A7B55
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		
		protected override void SpawnThings()
		{
			if (this.Contents.spawnWipeMode == null)
			{
				base.SpawnThings();
				return;
			}
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenSpawn.Spawn(this.innerContainer[i], base.Position, base.Map, this.Contents.spawnWipeMode.Value);
			}
		}

		
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				MoteMaker.ThrowDustPuff(base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f), base.Map, 1.2f);
			}
			MoteMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			GenClamor.DoClamor(this, 15f, ClamorDefOf.Impact);
			base.Impact();
		}
	}
}
