using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020011CC RID: 4556
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x0600696A RID: 26986
		public abstract bool Active { get; }

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600696B RID: 26987
		public abstract Vector3 Position { get; }

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x0600696C RID: 26988
		// (set) Token: 0x0600696D RID: 26989
		public abstract Color Color { get; set; }

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x0600696E RID: 26990
		// (set) Token: 0x0600696F RID: 26991
		public abstract string Text { get; set; }

		// Token: 0x1700118C RID: 4492
		// (set) Token: 0x06006970 RID: 26992
		public abstract float Size { set; }

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06006971 RID: 26993
		// (set) Token: 0x06006972 RID: 26994
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06006973 RID: 26995
		// (set) Token: 0x06006974 RID: 26996
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x06006975 RID: 26997
		public abstract void SetActive(bool active);

		// Token: 0x06006976 RID: 26998
		public abstract void Destroy();

		// Token: 0x06006977 RID: 26999
		public abstract void Init();

		// Token: 0x06006978 RID: 27000
		public abstract void WrapAroundPlanetSurface();
	}
}
