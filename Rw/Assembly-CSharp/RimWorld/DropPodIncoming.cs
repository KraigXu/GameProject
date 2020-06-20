using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBB RID: 3259
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x17000DFE RID: 3582
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

		// Token: 0x06004F04 RID: 20228 RVA: 0x001A9970 File Offset: 0x001A7B70
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

		// Token: 0x06004F05 RID: 20229 RVA: 0x001A99DC File Offset: 0x001A7BDC
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
