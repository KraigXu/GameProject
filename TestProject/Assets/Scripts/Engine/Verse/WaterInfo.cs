using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000184 RID: 388
	public class WaterInfo : MapComponent
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0003C322 File Offset: 0x0003A522
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003C336 File Offset: 0x0003A536
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				UnityEngine.Object.Destroy(this.riverOffsetTexture);
			});
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0003C34C File Offset: 0x0003A54C
		public void SetTextures()
		{
			Camera subcamera = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.WaterDepth);
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOutputTex, subcamera.targetTexture);
			if (this.riverOffsetTexture == null && this.riverOffsetMap != null && this.riverOffsetMap.Length != 0)
			{
				this.riverOffsetTexture = new Texture2D(this.map.Size.x + 4, this.map.Size.z + 4, TextureFormat.RGFloat, false);
				this.riverOffsetTexture.LoadRawTextureData(this.riverOffsetMap);
				this.riverOffsetTexture.wrapMode = TextureWrapMode.Clamp;
				this.riverOffsetTexture.Apply();
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOffsetTex, this.riverOffsetTexture);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003C404 File Offset: 0x0003A604
		public Vector3 GetWaterMovement(Vector3 position)
		{
			if (this.riverOffsetMap == null)
			{
				return Vector3.zero;
			}
			if (this.riverFlowMap == null)
			{
				this.GenerateRiverFlowMap();
			}
			IntVec3 intVec = new IntVec3(Mathf.FloorToInt(position.x), 0, Mathf.FloorToInt(position.z));
			IntVec3 c = new IntVec3(Mathf.FloorToInt(position.x) + 1, 0, Mathf.FloorToInt(position.z) + 1);
			if (!this.riverFlowMapBounds.Contains(intVec) || !this.riverFlowMapBounds.Contains(c))
			{
				return Vector3.zero;
			}
			int num = this.riverFlowMapBounds.IndexOf(intVec);
			int num2 = num + 1;
			int num3 = num + this.riverFlowMapBounds.Width;
			int num4 = num3 + 1;
			Vector3 a = Vector3.Lerp(new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]), new Vector3(this.riverFlowMap[num2 * 2], 0f, this.riverFlowMap[num2 * 2 + 1]), position.x - Mathf.Floor(position.x));
			Vector3 b = Vector3.Lerp(new Vector3(this.riverFlowMap[num3 * 2], 0f, this.riverFlowMap[num3 * 2 + 1]), new Vector3(this.riverFlowMap[num4 * 2], 0f, this.riverFlowMap[num4 * 2 + 1]), position.x - Mathf.Floor(position.x));
			return Vector3.Lerp(a, b, position.z - (float)Mathf.FloorToInt(position.z));
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0003C584 File Offset: 0x0003A784
		public void GenerateRiverFlowMap()
		{
			if (this.riverOffsetMap == null)
			{
				return;
			}
			this.riverFlowMapBounds = new CellRect(-2, -2, this.map.Size.x + 4, this.map.Size.z + 4);
			this.riverFlowMap = new float[this.riverFlowMapBounds.Area * 2];
			float[] array = new float[this.riverFlowMapBounds.Area * 2];
			Buffer.BlockCopy(this.riverOffsetMap, 0, array, 0, array.Length * 4);
			for (int i = this.riverFlowMapBounds.minZ; i <= this.riverFlowMapBounds.maxZ; i++)
			{
				int newZ = (i == this.riverFlowMapBounds.minZ) ? i : (i - 1);
				int newZ2 = (i == this.riverFlowMapBounds.maxZ) ? i : (i + 1);
				float num = (float)((i == this.riverFlowMapBounds.minZ || i == this.riverFlowMapBounds.maxZ) ? 1 : 2);
				for (int j = this.riverFlowMapBounds.minX; j <= this.riverFlowMapBounds.maxX; j++)
				{
					int newX = (j == this.riverFlowMapBounds.minX) ? j : (j - 1);
					int newX2 = (j == this.riverFlowMapBounds.maxX) ? j : (j + 1);
					float num2 = (float)((j == this.riverFlowMapBounds.minX || j == this.riverFlowMapBounds.maxX) ? 1 : 2);
					float x = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX2, 0, i)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(newX, 0, i)) * 2 + 1]) / num2;
					float z = (array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ2)) * 2 + 1] - array[this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, newZ)) * 2 + 1]) / num;
					Vector3 vector = new Vector3(x, 0f, z);
					if (vector.magnitude > 0.0001f)
					{
						vector = vector.normalized / vector.magnitude;
						int num3 = this.riverFlowMapBounds.IndexOf(new IntVec3(j, 0, i)) * 2;
						this.riverFlowMap[num3] = vector.x;
						this.riverFlowMap[num3 + 1] = vector.z;
					}
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0003C7E0 File Offset: 0x0003A9E0
		public override void ExposeData()
		{
			base.ExposeData();
			DataExposeUtility.ByteArray(ref this.riverOffsetMap, "riverOffsetMap");
			this.GenerateRiverFlowMap();
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0003C800 File Offset: 0x0003AA00
		public void DebugDrawRiver()
		{
			for (int i = 0; i < this.riverDebugData.Count; i += 2)
			{
				GenDraw.DrawLineBetween(this.riverDebugData[i], this.riverDebugData[i + 1], SimpleColor.Magenta);
			}
		}

		// Token: 0x04000904 RID: 2308
		public byte[] riverOffsetMap;

		// Token: 0x04000905 RID: 2309
		public Texture2D riverOffsetTexture;

		// Token: 0x04000906 RID: 2310
		public List<Vector3> riverDebugData = new List<Vector3>();

		// Token: 0x04000907 RID: 2311
		public float[] riverFlowMap;

		// Token: 0x04000908 RID: 2312
		public CellRect riverFlowMapBounds;

		// Token: 0x04000909 RID: 2313
		public const int RiverOffsetMapBorder = 2;
	}
}
