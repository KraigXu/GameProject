using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020011DE RID: 4574
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060069D0 RID: 27088 RVA: 0x0024EBE6 File Offset: 0x0024CDE6
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

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x060069D1 RID: 27089 RVA: 0x0024EC08 File Offset: 0x0024CE08
		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				if (altitudePercent < 0.025f)
				{
					return WorldCameraZoomRange.VeryClose;
				}
				if (altitudePercent < 0.042f)
				{
					return WorldCameraZoomRange.Close;
				}
				if (altitudePercent < 0.125f)
				{
					return WorldCameraZoomRange.Far;
				}
				return WorldCameraZoomRange.VeryFar;
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x060069D2 RID: 27090 RVA: 0x00014515 File Offset: 0x00012715
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

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060069D3 RID: 27091 RVA: 0x0024EC3B File Offset: 0x0024CE3B
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x060069D4 RID: 27092 RVA: 0x0024EC4D File Offset: 0x0024CE4D
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(125f, 1100f, this.altitude);
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x060069D5 RID: 27093 RVA: 0x0024EC64 File Offset: 0x0024CE64
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060069D6 RID: 27094 RVA: 0x0024EC80 File Offset: 0x0024CE80
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x0024EC98 File Offset: 0x0024CE98
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x0024ECA8 File Offset: 0x0024CEA8
		public void WorldCameraDriverOnGUI()
		{
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
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
						currentEventDelta.x *= -1f;
						this.desiredRotationRaw += currentEventDelta / GenWorldUI.CurUITileSize() * 0.273f * Prefs.MapDragSensitivity;
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * 0.1f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.desiredAltitude -= num * this.config.zoomSpeed * this.altitude / 12f;
				this.desiredAltitude = Mathf.Clamp(this.desiredAltitude, 125f, 1100f);
				this.desiredRotation = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredRotation.x = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredRotation.x = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredRotation.y = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredRotation.y = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x0024EEE4 File Offset: 0x0024D0E4
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.World == null)
			{
				this.MyCamera.gameObject.SetActive(false);
				return;
			}
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.altitude - 125f) / 975f * 0.85f + 0.15f;
				this.rotationVelocity = new Vector2(vector.x, vector.y) * d;
			}
			if (!Input.GetMouseButton(2) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				this.rotationVelocity += CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 5f);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.sphereRotation *= Quaternion.AngleAxis(this.rotationVelocity.x * num * this.config.rotationSpeedScale, this.MyCamera.transform.up);
				this.sphereRotation *= Quaternion.AngleAxis(-this.rotationVelocity.y * num * this.config.rotationSpeedScale, this.MyCamera.transform.right);
				if (this.desiredRotationRaw != Vector2.zero)
				{
					this.sphereRotation *= Quaternion.AngleAxis(this.desiredRotationRaw.x, this.MyCamera.transform.up);
					this.sphereRotation *= Quaternion.AngleAxis(-this.desiredRotationRaw.y, this.MyCamera.transform.right);
				}
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredRotationRaw,
					time = Time.time
				});
			}
			this.desiredRotationRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.rotationVelocity != Vector2.zero)
				{
					this.rotationVelocity *= this.config.camRotationDecayFactor;
					if (this.rotationVelocity.magnitude < 0.05f)
					{
						this.rotationVelocity = Vector2.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.altitude, this.desiredAltitude, 0.05f);
					this.desiredAltitude += (num3 - this.altitude) * this.config.zoomPreserveFactor;
					this.altitude = num3;
				}
				else
				{
					float num4 = (this.desiredAltitude - this.altitude) * 0.4f;
					this.desiredAltitude += this.config.zoomPreserveFactor * num4;
					this.altitude += num4;
				}
			}
			this.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
			if (Find.PlaySettings.lockNorthUp)
			{
				this.RotateSoNorthIsUp(false);
				this.ClampXRotation(ref this.sphereRotation);
			}
			for (int j = 0; j < num2; j++)
			{
				this.config.ConfigFixedUpdate_60(ref this.rotationVelocity);
			}
			this.ApplyPositionToGameObject();
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x0024F260 File Offset: 0x0024D460
		private void ApplyPositionToGameObject()
		{
			Quaternion rotation;
			if (this.rotationAnimation_lerpFactor < 1f)
			{
				rotation = Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			}
			else
			{
				rotation = this.sphereRotation;
			}
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			this.MyCamera.transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = this.MyCamera.transform.rotation * Vector3.forward;
			this.MyCamera.transform.position = -a * this.altitude;
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x0024F304 File Offset: 0x0024D504
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						zero.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						zero.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						zero.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							zero.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += zero;
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

		// Token: 0x060069DC RID: 27100 RVA: 0x0024F557 File Offset: 0x0024D757
		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x0024F585 File Offset: 0x0024D785
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x0024F5B9 File Offset: 0x0024D7B9
		public void JumpTo(int tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x0024F5CC File Offset: 0x0024D7CC
		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x0024F61C File Offset: 0x0024D81C
		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90f)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360f - num)
			{
				eulerAngles.x = 360f - num;
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}

		// Token: 0x040041D6 RID: 16854
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x040041D7 RID: 16855
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x040041D8 RID: 16856
		private Vector2 rotationVelocity;

		// Token: 0x040041D9 RID: 16857
		private Vector2 desiredRotation;

		// Token: 0x040041DA RID: 16858
		private Vector2 desiredRotationRaw;

		// Token: 0x040041DB RID: 16859
		private float desiredAltitude;

		// Token: 0x040041DC RID: 16860
		public float altitude;

		// Token: 0x040041DD RID: 16861
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x040041DE RID: 16862
		private Camera cachedCamera;

		// Token: 0x040041DF RID: 16863
		private bool mouseCoveredByUI;

		// Token: 0x040041E0 RID: 16864
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x040041E1 RID: 16865
		private float fixedTimeStepBuffer;

		// Token: 0x040041E2 RID: 16866
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x040041E3 RID: 16867
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x040041E4 RID: 16868
		private const float SphereRadius = 100f;

		// Token: 0x040041E5 RID: 16869
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x040041E6 RID: 16870
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x040041E7 RID: 16871
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x040041E8 RID: 16872
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x040041E9 RID: 16873
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x040041EA RID: 16874
		private const float TileSizeToRotationSpeed = 0.273f;

		// Token: 0x040041EB RID: 16875
		private const float VelocityFromMouseDragInitialFactor = 5f;

		// Token: 0x040041EC RID: 16876
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x040041ED RID: 16877
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x040041EE RID: 16878
		public const float MinAltitude = 125f;

		// Token: 0x040041EF RID: 16879
		private const float MaxAltitude = 1100f;

		// Token: 0x040041F0 RID: 16880
		private const float ZoomTightness = 0.4f;

		// Token: 0x040041F1 RID: 16881
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x040041F2 RID: 16882
		private const float PageKeyZoomRate = 2f;

		// Token: 0x040041F3 RID: 16883
		private const float ScrollWheelZoomRate = 0.1f;
	}
}
