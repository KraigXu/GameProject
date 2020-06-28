using System;

namespace Verse
{
	// Token: 0x02000181 RID: 385
	public abstract class MapComponent : IExposable
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x0003C0E3 File Offset: 0x0003A2E3
		public MapComponent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MapComponentUpdate()
		{
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MapComponentTick()
		{
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MapComponentOnGUI()
		{
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MapGenerated()
		{
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void MapRemoved()
		{
		}

		// Token: 0x04000902 RID: 2306
		public Map map;
	}
}
