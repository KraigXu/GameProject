using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	
	public struct SoundInfo
	{
		
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000DA76A File Offset: 0x000D896A
		// (set) Token: 0x060024CC RID: 9420 RVA: 0x000DA772 File Offset: 0x000D8972
		public bool IsOnCamera { get; private set; }

		
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000DA77B File Offset: 0x000D897B
		// (set) Token: 0x060024CE RID: 9422 RVA: 0x000DA783 File Offset: 0x000D8983
		public TargetInfo Maker { get; private set; }

		
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000DA78C File Offset: 0x000D898C
		// (set) Token: 0x060024D0 RID: 9424 RVA: 0x000DA794 File Offset: 0x000D8994
		public MaintenanceType Maintenance { get; private set; }

		
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x000DA79D File Offset: 0x000D899D
		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters == null)
				{
					yield break;
				}
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					yield return keyValuePair;
				}
				Dictionary<string, float>.Enumerator enumerator = default(Dictionary<string, float>.Enumerator);
				yield break;
				yield break;
			}
		}

		
		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		
		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		
		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					text = string.Concat(new string[]
					{
						text,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text2 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text2 = this.Maker.ToString();
			}
			string text3 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text3 = ", Maint=" + this.Maintenance;
			}
			return string.Concat(new string[]
			{
				"(",
				this.IsOnCamera ? "Camera" : "World from ",
				text2,
				text,
				text3,
				")"
			});
		}

		
		private Dictionary<string, float> parameters;

		
		public float volumeFactor;

		
		public float pitchFactor;

		
		public bool testPlay;
	}
}
