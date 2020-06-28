using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200005C RID: 92
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000144F3 File Offset: 0x000126F3
		private Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00014515 File Offset: 0x00012715
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001452C File Offset: 0x0001272C
		public CameraZoomRange CurrentZoom
		{
			get
			{
				if (this.rootSize < this.config.minSize + 1f)
				{
					return CameraZoomRange.Closest;
				}
				if (this.rootSize < 13.8f)
				{
					return CameraZoomRange.Close;
				}
				if (this.rootSize < 42f)
				{
					return CameraZoomRange.Middle;
				}
				if (this.rootSize < 57f)
				{
					return CameraZoomRange.Far;
				}
				return CameraZoomRange.Furthest;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00014582 File Offset: 0x00012782
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00014594 File Offset: 0x00012794
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000145AC File Offset: 0x000127AC
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000145D0 File Offset: 0x000127D0
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					Vector3 currentRealPosition = this.CurrentRealPosition;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(currentRealPosition.x - this.rootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(currentRealPosition.x + this.rootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(currentRealPosition.z - this.rootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(currentRealPosition.z + this.rootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001469C File Offset: 0x0001289C
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000146C8 File Offset: 0x000128C8
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.rootSize * 2f);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000146DD File Offset: 0x000128DD
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.farClipPlane = 71.5f;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001470B File Offset: 0x0001290B
		public void OnPreRender()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			Map currentMap = Find.CurrentMap;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001471B File Offset: 0x0001291B
		public void OnPreCull()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				Find.CurrentMap.weatherManager.DrawAllWeather();
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00014744 File Offset: 0x00012944
		public void CameraDriverOnGUI()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			this.mouseCoveredByUI = false;
			if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
			{
				this.mouseCoveredByUI = true;
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					Event.current.Use();
					if (currentEventDelta != Vector2.zero)
					{
						currentEventDelta.x *= -1f;
						this.desiredDollyRaw += currentEventDelta / UI.CurUICellSize() * Prefs.MapDragSensitivity;
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * 0.35f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				this.desiredSize -= num * this.config.zoomSpeed * this.rootSize / 35f;
				this.desiredSize = Mathf.Clamp(this.desiredSize, this.config.minSize, 60f);
				this.desiredDolly = Vector3.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredDolly.x = -this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredDolly.x = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredDolly.y = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredDolly.y = -this.config.dollyRateKeys;
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00014954 File Offset: 0x00012B54
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.rootSize - this.config.minSize) / (60f - this.config.minSize) * 0.7f + 0.3f;
				this.velocity = new Vector3(vector.x, 0f, vector.y) * d;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
			}
			if (!Input.GetMouseButton(2) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				Vector2 extraVelocityFromReleasingDragButton = CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 0.75f);
				this.velocity += new Vector3(extraVelocityFromReleasingDragButton.x, 0f, extraVelocityFromReleasingDragButton.y);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float d2 = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.rootPos += this.velocity * d2 * this.config.moveSpeedScale;
				this.rootPos += new Vector3(this.desiredDollyRaw.x, 0f, this.desiredDollyRaw.y);
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredDollyRaw,
					time = Time.time
				});
				this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
				this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
			}
			this.desiredDollyRaw = Vector2.zero;
			int num = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num; i++)
			{
				if (this.velocity != Vector3.zero)
				{
					this.velocity *= this.config.camSpeedDecayFactor;
					if (this.velocity.magnitude < 0.1f)
					{
						this.velocity = Vector3.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num2 = Mathf.Lerp(this.rootSize, this.desiredSize, 0.05f);
					this.desiredSize += (num2 - this.rootSize) * this.config.zoomPreserveFactor;
					this.rootSize = num2;
				}
				else
				{
					float num3 = (this.desiredSize - this.rootSize) * 0.4f;
					this.desiredSize += this.config.zoomPreserveFactor * num3;
					this.rootSize += num3;
				}
				this.config.ConfigFixedUpdate_60(ref this.velocity);
			}
			this.shaker.Update();
			this.ApplyPositionToGameObject();
			Current.SubcameraDriver.UpdatePositions(this.MyCamera);
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				rememberedCameraPos.rootPos = this.rootPos;
				rememberedCameraPos.rootSize = this.rootSize;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00014CD0 File Offset: 0x00012ED0
		private void ApplyPositionToGameObject()
		{
			this.rootPos.y = 15f + (this.rootSize - this.config.minSize) / (60f - this.config.minSize) * 50f;
			this.MyCamera.orthographicSize = this.rootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			Vector3 position = base.transform.position;
			position.y = 65f;
			this.reverbDummy.transform.position = position;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00014D78 File Offset: 0x00012F78
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 vector2 = mousePositionOnUI;
				vector2.y = (float)UI.screenHeight - vector2.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(vector2) && !rect3.Contains(vector2) && !rect2.Contains(vector2) && !rect4.Contains(vector2))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00015014 File Offset: 0x00013214
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.rootSize = this.desiredSize;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00015084 File Offset: 0x00013284
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.rootSize = this.desiredSize;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001509D File Offset: 0x0001329D
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x000150AC File Offset: 0x000132AC
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000150D0 File Offset: 0x000132D0
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredDollyRaw = Vector2.zero;
			this.desiredSize = rootSize;
			this.dragTimeStamps.Clear();
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015124 File Offset: 0x00013324
		public static Vector2 GetExtraVelocityFromReleasingDragButton(List<CameraDriver.DragTimeStamp> dragTimeStamps, float velocityFromMouseDragInitialFactor)
		{
			float num = 0f;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < dragTimeStamps.Count; i++)
			{
				if (dragTimeStamps[i].time < Time.time - 0.05f)
				{
					num = 0.05f;
				}
				else
				{
					num = Mathf.Max(num, Time.time - dragTimeStamps[i].time);
					vector += dragTimeStamps[i].posDelta;
				}
			}
			if (vector != Vector2.zero && num > 0f)
			{
				return vector / num * velocityFromMouseDragInitialFactor;
			}
			return Vector2.zero;
		}

		// Token: 0x0400012B RID: 299
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x0400012C RID: 300
		private Camera cachedCamera;

		// Token: 0x0400012D RID: 301
		private GameObject reverbDummy;

		// Token: 0x0400012E RID: 302
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x0400012F RID: 303
		private Vector3 velocity;

		// Token: 0x04000130 RID: 304
		private Vector3 rootPos;

		// Token: 0x04000131 RID: 305
		private float rootSize;

		// Token: 0x04000132 RID: 306
		private float desiredSize;

		// Token: 0x04000133 RID: 307
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x04000134 RID: 308
		private Vector2 desiredDollyRaw = Vector2.zero;

		// Token: 0x04000135 RID: 309
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x04000136 RID: 310
		private bool mouseCoveredByUI;

		// Token: 0x04000137 RID: 311
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04000138 RID: 312
		private float fixedTimeStepBuffer;

		// Token: 0x04000139 RID: 313
		private static int lastViewRectGetFrame = -1;

		// Token: 0x0400013A RID: 314
		private static CellRect lastViewRect;

		// Token: 0x0400013B RID: 315
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x0400013C RID: 316
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x0400013D RID: 317
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x0400013E RID: 318
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x0400013F RID: 319
		private const float DragTimeStampExpireSeconds = 0.05f;

		// Token: 0x04000140 RID: 320
		private const float VelocityFromMouseDragInitialFactor = 0.75f;

		// Token: 0x04000141 RID: 321
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x04000142 RID: 322
		public const float StartingSize = 24f;

		// Token: 0x04000143 RID: 323
		private const float MaxSize = 60f;

		// Token: 0x04000144 RID: 324
		private const float ZoomTightness = 0.4f;

		// Token: 0x04000145 RID: 325
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x04000146 RID: 326
		private const float PageKeyZoomRate = 4f;

		// Token: 0x04000147 RID: 327
		private const float ScrollWheelZoomRate = 0.35f;

		// Token: 0x04000148 RID: 328
		public const float MinAltitude = 15f;

		// Token: 0x04000149 RID: 329
		private const float MaxAltitude = 65f;

		// Token: 0x0400014A RID: 330
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x02001320 RID: 4896
		public struct DragTimeStamp
		{
			// Token: 0x0400484D RID: 18509
			public Vector2 posDelta;

			// Token: 0x0400484E RID: 18510
			public float time;
		}
	}
}
