using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E3 RID: 483
	public abstract class SkyOverlay
	{
		// Token: 0x1700029C RID: 668
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x0004E21B File Offset: 0x0004C41B
		public Color OverlayColor
		{
			set
			{
				if (this.worldOverlayMat != null)
				{
					this.worldOverlayMat.color = value;
				}
				if (this.screenOverlayMat != null)
				{
					this.screenOverlayMat.color = value;
				}
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004E251 File Offset: 0x0004C451
		public SkyOverlay()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.OverlayColor = Color.clear;
			});
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004E26C File Offset: 0x0004C46C
		public virtual void TickOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset("_MainTex", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir1 * -1f * this.worldOverlayPanSpeed1 * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					this.worldOverlayMat.SetTextureOffset("_MainTex2", (float)(Find.TickManager.TicksGame % 3600000) * this.worldPanDir2 * -1f * this.worldOverlayPanSpeed2 * this.worldOverlayMat.GetTextureScale("_MainTex2").x);
				}
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0004E354 File Offset: 0x0004C554
		public void DrawOverlay(Map map)
		{
			if (this.worldOverlayMat != null)
			{
				Vector3 position = map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather);
				Graphics.DrawMesh(MeshPool.wholeMapPlane, position, Quaternion.identity, this.worldOverlayMat, 0);
			}
			if (this.screenOverlayMat != null)
			{
				float num = Find.Camera.orthographicSize * 2f;
				Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
				Vector3 position2 = Find.Camera.transform.position;
				position2.y = AltitudeLayer.Weather.AltitudeFor() + 0.0454545468f;
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(position2, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.screenOverlayMat, 0);
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0004E41E File Offset: 0x0004C61E
		public override string ToString()
		{
			if (this.worldOverlayMat != null)
			{
				return this.worldOverlayMat.name;
			}
			if (this.screenOverlayMat != null)
			{
				return this.screenOverlayMat.name;
			}
			return "NoOverlayOverlay";
		}

		// Token: 0x04000A74 RID: 2676
		public Material worldOverlayMat;

		// Token: 0x04000A75 RID: 2677
		public Material screenOverlayMat;

		// Token: 0x04000A76 RID: 2678
		protected float worldOverlayPanSpeed1;

		// Token: 0x04000A77 RID: 2679
		protected float worldOverlayPanSpeed2;

		// Token: 0x04000A78 RID: 2680
		protected Vector2 worldPanDir1;

		// Token: 0x04000A79 RID: 2681
		protected Vector2 worldPanDir2;
	}
}
