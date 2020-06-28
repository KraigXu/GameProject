using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000443 RID: 1091
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x000C6BA4 File Offset: 0x000C4DA4
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000C6BBC File Offset: 0x000C4DBC
		public static Color CurTargetingColor
		{
			get
			{
				float num = (float)Math.Sin((double)(Time.time * 8f));
				num *= 0.2f;
				num += 0.8f;
				return new Color(1f, num, num);
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000C6BF8 File Offset: 0x000C4DF8
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000C6C01 File Offset: 0x000C4E01
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000C6C0C File Offset: 0x000C4E0C
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000C6CAC File Offset: 0x000C4EAC
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000C6CBA File Offset: 0x000C4EBA
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, GenDraw.LineMatWhite);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000C6CE8 File Offset: 0x000C4EE8
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color));
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000C6CF8 File Offset: 0x000C4EF8
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat)
		{
			if (Mathf.Abs(A.x - B.x) < 0.01f && Mathf.Abs(A.z - B.z) < 0.01f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			if (A == B)
			{
				return;
			}
			A.y = B.y;
			float z = (A - B).MagnitudeHorizontal();
			Quaternion q = Quaternion.LookRotation(A - B);
			Vector3 s = new Vector3(0.2f, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000C6DAD File Offset: 0x000C4FAD
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000C6DBB File Offset: 0x000C4FBB
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000C6DCC File Offset: 0x000C4FCC
		public static void DrawCircleOutline(Vector3 center, float radius, Material material)
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(24f * radius), 12, 48);
			float num2 = 0f;
			float num3 = 6.28318548f / (float)num;
			Vector3 vector = center;
			Vector3 a = center;
			for (int i = 0; i < num + 2; i++)
			{
				if (i >= 2)
				{
					GenDraw.DrawLineBetween(a, vector, material);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000C6E54 File Offset: 0x000C5054
		private static Material GetLineMat(SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return GenDraw.LineMatWhite;
			case SimpleColor.Red:
				return GenDraw.LineMatRed;
			case SimpleColor.Green:
				return GenDraw.LineMatGreen;
			case SimpleColor.Blue:
				return GenDraw.LineMatBlue;
			case SimpleColor.Magenta:
				return GenDraw.LineMatMagenta;
			case SimpleColor.Yellow:
				return GenDraw.LineMatYellow;
			case SimpleColor.Cyan:
				return GenDraw.LineMatCyan;
			default:
				return GenDraw.LineMatWhite;
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000C6EB4 File Offset: 0x000C50B4
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000C6EC8 File Offset: 0x000C50C8
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, Material material, float widthFactor = 1f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.005f && Mathf.Abs(A.y - B.y) < 0.005f && Mathf.Abs(A.z - B.z) < 0.005f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			float magnitude = (A - B).magnitude;
			Quaternion q = Quaternion.LookRotation(A - B, pos.normalized);
			Vector3 s = new Vector3(0.2f * Find.WorldGrid.averageTileSize * widthFactor, 1f, magnitude);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000C6F9C File Offset: 0x000C519C
		public static void DrawWorldRadiusRing(int center, int radius)
		{
			if (radius < 0)
			{
				return;
			}
			if (GenDraw.cachedEdgeTilesForCenter != center || GenDraw.cachedEdgeTilesForRadius != radius || GenDraw.cachedEdgeTilesForWorldSeed != Find.World.info.Seed)
			{
				GenDraw.cachedEdgeTilesForCenter = center;
				GenDraw.cachedEdgeTilesForRadius = radius;
				GenDraw.cachedEdgeTilesForWorldSeed = Find.World.info.Seed;
				GenDraw.cachedEdgeTiles.Clear();
				Find.WorldFloodFiller.FloodFill(center, (int tile) => true, delegate(int tile, int dist)
				{
					if (dist > radius + 1)
					{
						return true;
					}
					if (dist == radius + 1)
					{
						GenDraw.cachedEdgeTiles.Add(tile);
					}
					return false;
				}, int.MaxValue, null);
				WorldGrid worldGrid = Find.WorldGrid;
				Vector3 c = worldGrid.GetTileCenter(center);
				Vector3 n = c.normalized;
				GenDraw.cachedEdgeTiles.Sort(delegate(int a, int b)
				{
					float num = Vector3.Dot(n, Vector3.Cross(worldGrid.GetTileCenter(a) - c, worldGrid.GetTileCenter(b) - c));
					if (Mathf.Abs(num) < 0.0001f)
					{
						return 0;
					}
					if (num < 0f)
					{
						return -1;
					}
					return 1;
				});
			}
			GenDraw.DrawWorldLineStrip(GenDraw.cachedEdgeTiles, GenDraw.OneSidedWorldLineMatWhite, 5f);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000C70B8 File Offset: 0x000C52B8
		public static void DrawWorldLineStrip(List<int> edgeTiles, Material material, float widthFactor)
		{
			if (edgeTiles.Count < 3)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			float d = 0.05f;
			for (int i = 0; i < edgeTiles.Count; i++)
			{
				int index = (i == 0) ? (edgeTiles.Count - 1) : (i - 1);
				int num = edgeTiles[index];
				int num2 = edgeTiles[i];
				if (worldGrid.IsNeighbor(num, num2))
				{
					Vector3 a = worldGrid.GetTileCenter(num);
					Vector3 vector = worldGrid.GetTileCenter(num2);
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector, material, widthFactor);
				}
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000C7169 File Offset: 0x000C5369
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
				return;
			}
			GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000C718D File Offset: 0x000C538D
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			GenDraw.DrawTargetHighlightWithLayer(c, AltitudeLayer.Building);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000C7198 File Offset: 0x000C5398
		public static void DrawTargetHighlightWithLayer(IntVec3 c, AltitudeLayer layer)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(layer);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000C71C4 File Offset: 0x000C53C4
		public static void DrawTargetHighlightWithLayer(Vector3 c, AltitudeLayer layer)
		{
			Vector3 position = new Vector3(c.x, layer.AltitudeFor(), c.z);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000C7200 File Offset: 0x000C5400
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000C723B File Offset: 0x000C543B
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000C7244 File Offset: 0x000C5444
		public static void DrawInteractionCell(ThingDef tDef, IntVec3 center, Rot4 placingRot)
		{
			if (tDef.hasInteractionCell)
			{
				IntVec3 c = ThingUtility.InteractionCellWhenAt(tDef, center, placingRot, Find.CurrentMap);
				Vector3 vector = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				if (c.InBounds(Find.CurrentMap))
				{
					Building edifice = c.GetEdifice(Find.CurrentMap);
					if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
					{
						return;
					}
				}
				if (tDef.interactionCellGraphic == null && tDef.interactionCellIcon != null)
				{
					ThingDef thingDef = tDef.interactionCellIcon;
					if (thingDef.blueprintDef != null)
					{
						thingDef = thingDef.blueprintDef;
					}
					tDef.interactionCellGraphic = thingDef.graphic.GetColoredVersion(ShaderTypeDefOf.EdgeDetect.Shader, GenDraw.InteractionCellIntensity, Color.white);
				}
				if (tDef.interactionCellGraphic != null)
				{
					Rot4 rot = tDef.interactionCellIconReverse ? placingRot.Opposite : placingRot;
					tDef.interactionCellGraphic.DrawFromDef(vector, rot, tDef.interactionCellIcon, 0f);
					return;
				}
				Graphics.DrawMesh(MeshPool.plane10, vector, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000C7348 File Offset: 0x000C5548
		public static void DrawRadiusRing(IntVec3 center, float radius, Color color, Func<IntVec3, bool> predicate = null)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius + ": not enough squares in the precalculated list.", false);
					GenDraw.maxRadiusMessaged = true;
				}
				return;
			}
			GenDraw.ringDrawCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (predicate == null || predicate(intVec))
				{
					GenDraw.ringDrawCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(GenDraw.ringDrawCells, color);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000C73D7 File Offset: 0x000C55D7
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			GenDraw.DrawRadiusRing(center, radius, Color.white, null);
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000C73E6 File Offset: 0x000C55E6
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000C73F4 File Offset: 0x000C55F4
		public static void DrawFieldEdges(List<IntVec3> cells, Color color)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom(new MaterialRequest
			{
				shader = ShaderDatabase.Transparent,
				color = color,
				BaseTexPath = "UI/Overlays/TargetHighlight_Side"
			});
			material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
			if (GenDraw.fieldGrid == null)
			{
				GenDraw.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				IntVec3 intVec = cells[j];
				if (intVec.InBounds(currentMap))
				{
					GenDraw.rotNeeded[0] = (intVec.z < z - 1 && !GenDraw.fieldGrid[intVec.x, intVec.z + 1]);
					GenDraw.rotNeeded[1] = (intVec.x < x - 1 && !GenDraw.fieldGrid[intVec.x + 1, intVec.z]);
					GenDraw.rotNeeded[2] = (intVec.z > 0 && !GenDraw.fieldGrid[intVec.x, intVec.z - 1]);
					GenDraw.rotNeeded[3] = (intVec.x > 0 && !GenDraw.fieldGrid[intVec.x - 1, intVec.z]);
					for (int k = 0; k < 4; k++)
					{
						if (GenDraw.rotNeeded[k])
						{
							Graphics.DrawMesh(MeshPool.plane10, intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), new Rot4(k).AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000C7604 File Offset: 0x000C5804
		public static void DrawAimPie(Thing shooter, LocalTargetInfo target, int degreesWide, float offsetDist)
		{
			float facing = 0f;
			if (target.Cell != shooter.Position)
			{
				if (target.Thing != null)
				{
					facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
				}
				else
				{
					facing = (target.Cell - shooter.Position).AngleFlat;
				}
			}
			GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000C7698 File Offset: 0x000C5898
		public static void DrawAimPieRaw(Vector3 center, float facing, int degreesWide)
		{
			if (degreesWide <= 0)
			{
				return;
			}
			if (degreesWide > 360)
			{
				degreesWide = 360;
			}
			center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.8f;
			Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000C770C File Offset: 0x000C590C
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000C7750 File Offset: 0x000C5950
		public static void DrawFillableBar(GenDraw.FillableBarRequest r)
		{
			Vector2 vector = r.preRotationOffset.RotatedBy(r.rotation.AsAngle);
			r.center += new Vector3(vector.x, 0f, vector.y);
			if (r.rotation == Rot4.South)
			{
				r.rotation = Rot4.North;
			}
			if (r.rotation == Rot4.West)
			{
				r.rotation = Rot4.East;
			}
			Vector3 s = new Vector3(r.size.x + r.margin, 1f, r.size.y + r.margin);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(r.center, r.rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, r.unfilledMat, 0);
			if (r.fillPercent > 0.001f)
			{
				s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
				matrix = default(Matrix4x4);
				Vector3 pos = r.center + Vector3.up * 0.01f;
				if (!r.rotation.IsHorizontal)
				{
					pos.x -= r.size.x * 0.5f;
					pos.x += 0.5f * r.size.x * r.fillPercent;
				}
				else
				{
					pos.z -= r.size.x * 0.5f;
					pos.z += 0.5f * r.size.x * r.fillPercent;
				}
				matrix.SetTRS(pos, r.rotation.AsQuat, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000C7953 File Offset: 0x000C5B53
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, loc, quat);
				return;
			}
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000C7974 File Offset: 0x000C5B74
		public static void DrawArrowPointingAt(Vector3 mapTarget, bool offscreenOnly = false)
		{
			Vector3 vector = UI.UIToMapPosition((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
			if ((vector - mapTarget).MagnitudeHorizontalSquared() < 81f)
			{
				if (!offscreenOnly)
				{
					Vector3 position = mapTarget;
					position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
					position.z -= 1.5f;
					Graphics.DrawMesh(MeshPool.plane20, position, Quaternion.identity, GenDraw.ArrowMatWhite, 0);
					return;
				}
			}
			else
			{
				Vector3 normalized = (mapTarget - vector).Yto0().normalized;
				Vector3 position2 = vector + normalized * 7f;
				position2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Quaternion rotation = Quaternion.LookRotation(normalized);
				Graphics.DrawMesh(MeshPool.plane20, position2, rotation, GenDraw.ArrowMatWhite, 0);
			}
		}

		// Token: 0x040013D8 RID: 5080
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x040013D9 RID: 5081
		private const float TargetPulseFrequency = 8f;

		// Token: 0x040013DA RID: 5082
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x040013DB RID: 5083
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x040013DC RID: 5084
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x040013DD RID: 5085
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x040013DE RID: 5086
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x040013DF RID: 5087
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x040013E0 RID: 5088
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x040013E1 RID: 5089
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x040013E2 RID: 5090
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x040013E3 RID: 5091
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x040013E4 RID: 5092
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040013E5 RID: 5093
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040013E6 RID: 5094
		private const float LineWidth = 0.2f;

		// Token: 0x040013E7 RID: 5095
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x040013E8 RID: 5096
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x040013E9 RID: 5097
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040013EA RID: 5098
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x040013EB RID: 5099
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x040013EC RID: 5100
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x040013ED RID: 5101
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x040013EE RID: 5102
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x040013EF RID: 5103
		private static bool maxRadiusMessaged = false;

		// Token: 0x040013F0 RID: 5104
		private static BoolGrid fieldGrid;

		// Token: 0x040013F1 RID: 5105
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x040013F2 RID: 5106
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x040013F3 RID: 5107
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x02001692 RID: 5778
		public struct FillableBarRequest
		{
			// Token: 0x0400568A RID: 22154
			public Vector3 center;

			// Token: 0x0400568B RID: 22155
			public Vector2 size;

			// Token: 0x0400568C RID: 22156
			public float fillPercent;

			// Token: 0x0400568D RID: 22157
			public Material filledMat;

			// Token: 0x0400568E RID: 22158
			public Material unfilledMat;

			// Token: 0x0400568F RID: 22159
			public float margin;

			// Token: 0x04005690 RID: 22160
			public Rot4 rotation;

			// Token: 0x04005691 RID: 22161
			public Vector2 preRotationOffset;
		}
	}
}
