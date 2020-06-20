using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C56 RID: 3158
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x06004B60 RID: 19296 RVA: 0x00196B80 File Offset: 0x00194D80
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (this.overlaysToDraw.ContainsKey(t))
			{
				Dictionary<Thing, OverlayTypes> dictionary = this.overlaysToDraw;
				dictionary[t] |= overlayType;
				return;
			}
			this.overlaysToDraw.Add(t, overlayType);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x00196BC4 File Offset: 0x00194DC4
		public void DrawAllOverlays()
		{
			foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair in this.overlaysToDraw)
			{
				this.curOffset = Vector3.zero;
				Thing key = keyValuePair.Key;
				OverlayTypes value = keyValuePair.Value;
				if ((value & OverlayTypes.BurningWick) != (OverlayTypes)0)
				{
					this.RenderBurningWick(key);
				}
				else
				{
					OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
					int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
					float num = this.StackOffsetFor(keyValuePair.Key);
					switch (bitCountOf)
					{
					case 1:
						this.curOffset = Vector3.zero;
						break;
					case 2:
						this.curOffset = new Vector3(-0.5f * num, 0f, 0f);
						break;
					case 3:
						this.curOffset = new Vector3(-1.5f * num, 0f, 0f);
						break;
					}
					if ((value & OverlayTypes.NeedsPower) != (OverlayTypes)0)
					{
						this.RenderNeedsPowerOverlay(key);
					}
					if ((value & OverlayTypes.PowerOff) != (OverlayTypes)0)
					{
						this.RenderPowerOffOverlay(key);
					}
					if ((value & OverlayTypes.BrokenDown) != (OverlayTypes)0)
					{
						this.RenderBrokenDownOverlay(key);
					}
					if ((value & OverlayTypes.OutOfFuel) != (OverlayTypes)0)
					{
						this.RenderOutOfFuelOverlay(key);
					}
				}
				if ((value & OverlayTypes.ForbiddenBig) != (OverlayTypes)0)
				{
					this.RenderForbiddenBigOverlay(key);
				}
				if ((value & OverlayTypes.Forbidden) != (OverlayTypes)0)
				{
					this.RenderForbiddenOverlay(key);
				}
				if ((value & OverlayTypes.ForbiddenRefuel) != (OverlayTypes)0)
				{
					this.RenderForbiddenRefuelOverlay(key);
				}
				if ((value & OverlayTypes.QuestionMark) != (OverlayTypes)0)
				{
					this.RenderQuestionMarkOverlay(key);
				}
			}
			this.overlaysToDraw.Clear();
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00196D44 File Offset: 0x00194F44
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x00196D58 File Offset: 0x00194F58
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00196D68 File Offset: 0x00194F68
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00196D78 File Offset: 0x00194F78
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00196D88 File Offset: 0x00194F88
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x00196DE0 File Offset: 0x00194FE0
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x00196E00 File Offset: 0x00195000
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.0454545468f * (float)altInd;
			vector += this.curOffset;
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x00196E5C File Offset: 0x0019505C
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
			num = 0.3f + num * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num);
			Graphics.DrawMesh(mesh, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x00196EC4 File Offset: 0x001950C4
		private void RenderForbiddenRefuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material material = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 vector = t.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.227272734f;
			Vector3 position = new Vector3(vector.x, vector.y + 0.0454545468f, vector.z);
			Graphics.DrawMesh(MeshPool.plane08, vector, Quaternion.identity, material, 0);
			Graphics.DrawMesh(MeshPool.plane08, position, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x00196F64 File Offset: 0x00195164
		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			if (t.RotatedSize.z == 1)
			{
				drawPos.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				drawPos.z -= (float)t.RotatedSize.z * 0.3f;
			}
			drawPos.y = OverlayDrawer.BaseAlt + 0.181818187f;
			Graphics.DrawMesh(MeshPool.plane05, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x00196FE0 File Offset: 0x001951E0
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.181818187f;
			Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, OverlayDrawer.ForbiddenMat, 0);
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x0019701C File Offset: 0x0019521C
		private void RenderBurningWick(Thing parent)
		{
			Material material;
			if ((parent.thingIDNumber + Find.TickManager.TicksGame) % 6 < 3)
			{
				material = OverlayDrawer.WickMaterialA;
			}
			else
			{
				material = OverlayDrawer.WickMaterialB;
			}
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.227272734f;
			Graphics.DrawMesh(MeshPool.plane20, drawPos, Quaternion.identity, material, 0);
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x00197078 File Offset: 0x00195278
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.272727281f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x04002A9A RID: 10906
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x04002A9B RID: 10907
		private Vector3 curOffset;

		// Token: 0x04002A9C RID: 10908
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x04002A9D RID: 10909
		private static readonly Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x04002A9E RID: 10910
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x04002A9F RID: 10911
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x04002AA0 RID: 10912
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x04002AA1 RID: 10913
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x04002AA2 RID: 10914
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x04002AA3 RID: 10915
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x04002AA4 RID: 10916
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x04002AA5 RID: 10917
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x04002AA6 RID: 10918
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x04002AA7 RID: 10919
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x04002AA8 RID: 10920
		private const float PulseFrequency = 4f;

		// Token: 0x04002AA9 RID: 10921
		private const float PulseAmplitude = 0.7f;

		// Token: 0x04002AAA RID: 10922
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x04002AAB RID: 10923
		private const float StackOffsetMultipiler = 0.25f;
	}
}
