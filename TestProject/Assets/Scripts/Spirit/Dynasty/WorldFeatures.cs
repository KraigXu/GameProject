using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011CB RID: 4555
	public class WorldFeatures : IExposable
	{
		// Token: 0x0600695D RID: 26973 RVA: 0x0024CC9B File Offset: 0x0024AE9B
		private static void TextWrapThreshold_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x0024CC9B File Offset: 0x0024AE9B
		protected static void ForceLegacyText_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x0024CCA8 File Offset: 0x0024AEA8
		public void ExposeData()
		{
			Scribe_Collections.Look<WorldFeature>(ref this.features, "features", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				WorldGrid grid = Find.WorldGrid;
				if (grid.tileFeature != null && grid.tileFeature.Length != 0)
				{
					DataSerializeUtility.LoadUshort(grid.tileFeature, grid.TilesCount, delegate(int i, ushort data)
					{
						grid[i].feature = ((data == ushort.MaxValue) ? null : this.GetFeatureWithID((int)data));
					});
				}
				this.textsCreated = false;
			}
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x0024CD34 File Offset: 0x0024AF34
		public void UpdateFeatures()
		{
			if (!this.textsCreated)
			{
				this.textsCreated = true;
				this.CreateTextsAndSetPosition();
			}
			bool showWorldFeatures = Find.PlaySettings.showWorldFeatures;
			for (int i = 0; i < this.features.Count; i++)
			{
				Vector3 position = WorldFeatures.texts[i].Position;
				bool flag = showWorldFeatures && !WorldRendererUtility.HiddenBehindTerrainNow(position);
				if (flag != WorldFeatures.texts[i].Active)
				{
					WorldFeatures.texts[i].SetActive(flag);
					WorldFeatures.texts[i].WrapAroundPlanetSurface();
				}
				if (flag)
				{
					this.UpdateAlpha(WorldFeatures.texts[i], this.features[i]);
				}
			}
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x0024CDF0 File Offset: 0x0024AFF0
		public WorldFeature GetFeatureWithID(int uniqueID)
		{
			for (int i = 0; i < this.features.Count; i++)
			{
				if (this.features[i].uniqueID == uniqueID)
				{
					return this.features[i];
				}
			}
			return null;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x0024CE38 File Offset: 0x0024B038
		private void UpdateAlpha(WorldFeatureTextMesh text, WorldFeature feature)
		{
			float num = 0.3f * feature.alpha;
			if (text.Color.a != num)
			{
				text.Color = new Color(1f, 1f, 1f, num);
				text.WrapAroundPlanetSurface();
			}
			float num2 = Time.deltaTime * 5f;
			if (this.GoodCameraAltitudeFor(feature))
			{
				feature.alpha += num2;
			}
			else
			{
				feature.alpha -= num2;
			}
			feature.alpha = Mathf.Clamp01(feature.alpha);
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x0024CEC8 File Offset: 0x0024B0C8
		private bool GoodCameraAltitudeFor(WorldFeature feature)
		{
			float num = feature.EffectiveDrawSize;
			float altitude = Find.WorldCameraDriver.altitude;
			float num2 = 1f / (altitude / WorldFeatures.AlphaScale * (altitude / WorldFeatures.AlphaScale));
			num *= num2;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose && num >= 0.56f)
			{
				return false;
			}
			if (num < WorldFeatures.VisibleMinimumSize)
			{
				return Find.WorldCameraDriver.AltitudePercent <= 0.07f;
			}
			return num <= WorldFeatures.VisibleMaximumSize || Find.WorldCameraDriver.AltitudePercent >= 0.35f;
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x0024CF54 File Offset: 0x0024B154
		private void CreateTextsAndSetPosition()
		{
			this.CreateOrDestroyTexts();
			float averageTileSize = Find.WorldGrid.averageTileSize;
			for (int i = 0; i < this.features.Count; i++)
			{
				WorldFeatures.texts[i].Text = this.features[i].name.WordWrapAt(WorldFeatures.TextWrapThreshold);
				WorldFeatures.texts[i].Size = this.features[i].EffectiveDrawSize * averageTileSize;
				Vector3 normalized = this.features[i].drawCenter.normalized;
				Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(normalized, Vector3.up), normalized);
				quaternion *= Quaternion.Euler(Vector3.right * 90f);
				quaternion *= Quaternion.Euler(Vector3.forward * (90f - this.features[i].drawAngle));
				WorldFeatures.texts[i].Rotation = quaternion;
				WorldFeatures.texts[i].LocalPosition = this.features[i].drawCenter;
				WorldFeatures.texts[i].WrapAroundPlanetSurface();
				WorldFeatures.texts[i].SetActive(false);
			}
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x0024D0A0 File Offset: 0x0024B2A0
		private void CreateOrDestroyTexts()
		{
			for (int i = 0; i < WorldFeatures.texts.Count; i++)
			{
				WorldFeatures.texts[i].Destroy();
			}
			WorldFeatures.texts.Clear();
			bool flag = LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage;
			for (int j = 0; j < this.features.Count; j++)
			{
				WorldFeatureTextMesh worldFeatureTextMesh;
				if (WorldFeatures.ForceLegacyText || (!flag && this.HasCharactersUnsupportedByTextMeshPro(this.features[j].name)))
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_Legacy();
				}
				else
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_TextMeshPro();
				}
				worldFeatureTextMesh.Init();
				WorldFeatures.texts.Add(worldFeatureTextMesh);
			}
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x0024D144 File Offset: 0x0024B344
		private bool HasCharactersUnsupportedByTextMeshPro(string str)
		{
			TMP_FontAsset font = WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab.GetComponent<TextMeshPro>().font;
			for (int i = 0; i < str.Length; i++)
			{
				if (!this.HasCharacter(font, str[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x0024D188 File Offset: 0x0024B388
		private bool HasCharacter(TMP_FontAsset font, char character)
		{
			if (TMP_FontAsset.GetCharacters(font).IndexOf(character) >= 0)
			{
				return true;
			}
			List<TMP_FontAsset> fallbackFontAssetTable = font.fallbackFontAssetTable;
			for (int i = 0; i < fallbackFontAssetTable.Count; i++)
			{
				if (TMP_FontAsset.GetCharacters(fallbackFontAssetTable[i]).IndexOf(character) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400417F RID: 16767
		public List<WorldFeature> features = new List<WorldFeature>();

		// Token: 0x04004180 RID: 16768
		public bool textsCreated;

		// Token: 0x04004181 RID: 16769
		private static List<WorldFeatureTextMesh> texts = new List<WorldFeatureTextMesh>();

		// Token: 0x04004182 RID: 16770
		private const float BaseAlpha = 0.3f;

		// Token: 0x04004183 RID: 16771
		private const float AlphaChangeSpeed = 5f;

		// Token: 0x04004184 RID: 16772
		[TweakValue("Interface", 0f, 300f)]
		private static float TextWrapThreshold = 150f;

		// Token: 0x04004185 RID: 16773
		[TweakValue("Interface.World", 0f, 100f)]
		protected static bool ForceLegacyText = false;

		// Token: 0x04004186 RID: 16774
		[TweakValue("Interface.World", 1f, 150f)]
		protected static float AlphaScale = 30f;

		// Token: 0x04004187 RID: 16775
		[TweakValue("Interface.World", 0f, 1f)]
		protected static float VisibleMinimumSize = 0.04f;

		// Token: 0x04004188 RID: 16776
		[TweakValue("Interface.World", 0f, 5f)]
		protected static float VisibleMaximumSize = 1f;
	}
}
