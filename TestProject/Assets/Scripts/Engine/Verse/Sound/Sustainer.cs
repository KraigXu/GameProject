using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200050B RID: 1291
	public class Sustainer
	{
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x000DB411 File Offset: 0x000D9611
		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0f;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060024FC RID: 9468 RVA: 0x000DB423 File Offset: 0x000D9623
		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x000DB434 File Offset: 0x000D9634
		public float CameraDistanceSquared
		{
			get
			{
				if (this.info.IsOnCamera)
				{
					return 0f;
				}
				if (this.worldRootObject == null)
				{
					if (Prefs.DevMode)
					{
						Log.Error(string.Concat(new object[]
						{
							"Sustainer ",
							this.def,
							" info is ",
							this.info,
							" but its worldRootObject is null"
						}), false);
					}
					return 0f;
				}
				return (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000DB4DC File Offset: 0x000D96DC
		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
				{
					this.externalParams[keyValuePair.Key] = keyValuePair.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (info.IsOnCamera)
					{
						Log.Error("Playing sound " + def.ToString() + " on camera, but it has sub-sounds in the world.", false);
					}
					this.worldRootObject = new GameObject("SustainerRootObject_" + def.defName);
					this.UpdateRootObjectPosition();
				}
				else if (!info.IsOnCamera)
				{
					info = SoundInfo.OnCamera(info.Maintenance);
				}
				Find.SoundRoot.sustainerManager.RegisterSustainer(this);
				if (!info.IsOnCamera)
				{
					Find.SoundRoot.sustainerManager.UpdateAllSustainerScopes();
				}
				for (int i = 0; i < def.subSounds.Count; i++)
				{
					this.subSustainers.Add(new SubSustainer(this, def.subSounds[i]));
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				this.lastMaintainFrame = Time.frameCount;
			});
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000DB660 File Offset: 0x000D9860
		public void SustainerUpdate()
		{
			if (!this.Ended)
			{
				if (this.info.Maintenance == MaintenanceType.PerTick)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 1)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerFrame && Time.frameCount > this.lastMaintainFrame + 1)
				{
					this.End();
					return;
				}
			}
			else if (this.TimeSinceEnd > this.def.sustainFadeoutTime)
			{
				this.Cleanup();
			}
			if (this.def.subSounds.Count > 0)
			{
				if (!this.info.IsOnCamera && this.info.Maker.HasThing)
				{
					this.UpdateRootObjectPosition();
				}
				this.scopeFader.SustainerScopeUpdate();
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].SubSustainerUpdate();
				}
			}
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000DB748 File Offset: 0x000D9948
		private void UpdateRootObjectPosition()
		{
			if (this.worldRootObject != null)
			{
				this.worldRootObject.transform.position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000DB794 File Offset: 0x000D9994
		public void Maintain()
		{
			if (this.Ended)
			{
				Log.Error("Tried to maintain ended sustainer: " + this.def, false);
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerTick)
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerFrame)
			{
				this.lastMaintainFrame = Time.frameCount;
			}
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000DB7F8 File Offset: 0x000D99F8
		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutTime < 0.001f)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000DB820 File Offset: 0x000D9A20
		private void Cleanup()
		{
			if (this.def.subSounds.Count > 0)
			{
				Find.SoundRoot.sustainerManager.DeregisterSustainer(this);
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].Cleanup();
				}
			}
			if (this.def.sustainStopSound != null)
			{
				if (this.worldRootObject != null)
				{
					Map map = this.info.Maker.Map;
					if (map != null)
					{
						SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
						this.def.sustainStopSound.PlayOneShot(soundInfo);
					}
				}
				else
				{
					this.def.sustainStopSound.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
				}
			}
			if (this.worldRootObject != null)
			{
				UnityEngine.Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000DB91C File Offset: 0x000D9B1C
		public string DebugString()
		{
			string text = this.def.defName;
			text = text + "\n  inScopePercent=" + this.scopeFader.inScopePercent;
			text = text + "\n  CameraDistanceSquared=" + this.CameraDistanceSquared;
			foreach (SubSustainer arg in this.subSustainers)
			{
				text = text + "\n  sub: " + arg;
			}
			return text;
		}

		// Token: 0x04001675 RID: 5749
		public SoundDef def;

		// Token: 0x04001676 RID: 5750
		public SoundInfo info;

		// Token: 0x04001677 RID: 5751
		internal GameObject worldRootObject;

		// Token: 0x04001678 RID: 5752
		private int lastMaintainTick;

		// Token: 0x04001679 RID: 5753
		private int lastMaintainFrame;

		// Token: 0x0400167A RID: 5754
		private float endRealTime = -1f;

		// Token: 0x0400167B RID: 5755
		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		// Token: 0x0400167C RID: 5756
		public SoundParams externalParams = new SoundParams();

		// Token: 0x0400167D RID: 5757
		public SustainerScopeFader scopeFader = new SustainerScopeFader();
	}
}
