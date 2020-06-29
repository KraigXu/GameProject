using System;
using UnityEngine;

namespace RimWorld.Planet
{
	
	public abstract class WorldFeatureTextMesh
	{
		
		// (get) Token: 0x0600696A RID: 26986
		public abstract bool Active { get; }

		
		// (get) Token: 0x0600696B RID: 26987
		public abstract Vector3 Position { get; }

		
		// (get) Token: 0x0600696C RID: 26988
		// (set) Token: 0x0600696D RID: 26989
		public abstract Color Color { get; set; }

		
		// (get) Token: 0x0600696E RID: 26990
		// (set) Token: 0x0600696F RID: 26991
		public abstract string Text { get; set; }

		
		// (set) Token: 0x06006970 RID: 26992
		public abstract float Size { set; }

		
		// (get) Token: 0x06006971 RID: 26993
		// (set) Token: 0x06006972 RID: 26994
		public abstract Quaternion Rotation { get; set; }

		
		// (get) Token: 0x06006973 RID: 26995
		// (set) Token: 0x06006974 RID: 26996
		public abstract Vector3 LocalPosition { get; set; }

		
		public abstract void SetActive(bool active);

		
		public abstract void Destroy();

		
		public abstract void Init();

		
		public abstract void WrapAroundPlanetSurface();
	}
}
