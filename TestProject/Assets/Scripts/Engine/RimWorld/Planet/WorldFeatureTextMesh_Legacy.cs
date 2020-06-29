using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		
		// (get) Token: 0x0600697B RID: 27003 RVA: 0x0024D223 File Offset: 0x0024B423
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		
		// (get) Token: 0x0600697C RID: 27004 RVA: 0x0024D235 File Offset: 0x0024B435
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		
		// (get) Token: 0x0600697D RID: 27005 RVA: 0x0024D247 File Offset: 0x0024B447
		// (set) Token: 0x0600697E RID: 27006 RVA: 0x0024D254 File Offset: 0x0024B454
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

		
		// (get) Token: 0x0600697F RID: 27007 RVA: 0x0024D262 File Offset: 0x0024B462
		// (set) Token: 0x06006980 RID: 27008 RVA: 0x0024D26F File Offset: 0x0024B46F
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

		
		// (set) Token: 0x06006981 RID: 27009 RVA: 0x0024D27D File Offset: 0x0024B47D
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		
		// (get) Token: 0x06006982 RID: 27010 RVA: 0x0024D296 File Offset: 0x0024B496
		// (set) Token: 0x06006983 RID: 27011 RVA: 0x0024D2A8 File Offset: 0x0024B4A8
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

		
		// (get) Token: 0x06006984 RID: 27012 RVA: 0x0024D2BB File Offset: 0x0024B4BB
		// (set) Token: 0x06006985 RID: 27013 RVA: 0x0024D2CD File Offset: 0x0024B4CD
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
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = WorldMaterials.FeatureNameRenderQueue;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		
		public override void WrapAroundPlanetSurface()
		{
		}

		
		private TextMesh textMesh;

		
		private const float TextScale = 0.23f;

		
		private const int MinFontSize = 13;

		
		private const int MaxFontSize = 40;

		
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
