using System;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x0024D3EA File Offset: 0x0024B5EA
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		
		// (get) Token: 0x0600698E RID: 27022 RVA: 0x0024D3FC File Offset: 0x0024B5FC
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x0024D40E File Offset: 0x0024B60E
		// (set) Token: 0x06006990 RID: 27024 RVA: 0x0024D41B File Offset: 0x0024B61B
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		
		// (get) Token: 0x06006991 RID: 27025 RVA: 0x0024D429 File Offset: 0x0024B629
		// (set) Token: 0x06006992 RID: 27026 RVA: 0x0024D436 File Offset: 0x0024B636
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		
		// (set) Token: 0x06006993 RID: 27027 RVA: 0x0024D444 File Offset: 0x0024B644
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		
		// (get) Token: 0x06006994 RID: 27028 RVA: 0x0024D458 File Offset: 0x0024B658
		// (set) Token: 0x06006995 RID: 27029 RVA: 0x0024D46A File Offset: 0x0024B66A
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		
		// (get) Token: 0x06006996 RID: 27030 RVA: 0x0024D47D File Offset: 0x0024B67D
		// (set) Token: 0x06006997 RID: 27031 RVA: 0x0024D48F File Offset: 0x0024B68F
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		
		public override void Init()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = WorldMaterials.FeatureNameRenderQueue;
			}
		}

		
		public override void WrapAroundPlanetSurface()
		{
			this.textMesh.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				return;
			}
			float num = this.textMesh.bounds.extents.x * 2f;
			float num2 = Find.WorldGrid.DistOnSurfaceToAngle(num);
			Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
			for (int i = 0; i < characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = this.textMesh.textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3 vector = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3];
					vector /= 4f;
					float num3 = vector.x / (num / 2f);
					bool flag = num3 >= 0f;
					num3 = Mathf.Abs(num3);
					float num4 = num2 / 2f * num3;
					float num5 = (180f - num4) / 2f;
					float num6 = 200f * Mathf.Tan(num4 / 2f * 0.0174532924f);
					Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.0174532924f) * num6 * (flag ? 1f : -1f), vector.y, Mathf.Cos(num5 * 0.0174532924f) * num6);
					Vector3 b = vector2 - vector;
					Vector3 vector3 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + b;
					Vector3 vector4 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + b;
					Vector3 vector5 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + b;
					Vector3 vector6 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] + b;
					Quaternion rotation = Quaternion.Euler(0f, num4 * (flag ? -1f : 1f), 0f);
					vector3 = rotation * (vector3 - vector2) + vector2;
					vector4 = rotation * (vector4 - vector2) + vector2;
					vector5 = rotation * (vector5 - vector2) + vector2;
					vector6 = rotation * (vector6 - vector2) + vector2;
					vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * (100f + WorldAltitudeOffsets.WorldText));
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] = vector3;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] = vector4;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] = vector5;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] = vector6;
				}
			}
			this.textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		
		private TextMeshPro textMesh;

		
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
