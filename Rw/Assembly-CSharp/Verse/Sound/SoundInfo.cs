using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020004FE RID: 1278
	public struct SoundInfo
	{
		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x000DA76A File Offset: 0x000D896A
		// (set) Token: 0x060024CC RID: 9420 RVA: 0x000DA772 File Offset: 0x000D8972
		public bool IsOnCamera { get; private set; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000DA77B File Offset: 0x000D897B
		// (set) Token: 0x060024CE RID: 9422 RVA: 0x000DA783 File Offset: 0x000D8983
		public TargetInfo Maker { get; private set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000DA78C File Offset: 0x000D898C
		// (set) Token: 0x060024D0 RID: 9424 RVA: 0x000DA794 File Offset: 0x000D8994
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x1700075C RID: 1884
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

		// Token: 0x060024D2 RID: 9426 RVA: 0x000DA7B4 File Offset: 0x000D89B4
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

		// Token: 0x060024D3 RID: 9427 RVA: 0x000DA804 File Offset: 0x000D8A04
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

		// Token: 0x060024D4 RID: 9428 RVA: 0x000DA850 File Offset: 0x000D8A50
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x000DA872 File Offset: 0x000D8A72
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x000DA87B File Offset: 0x000D8A7B
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x000DA88C File Offset: 0x000D8A8C
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

		// Token: 0x04001655 RID: 5717
		private Dictionary<string, float> parameters;

		// Token: 0x04001656 RID: 5718
		public float volumeFactor;

		// Token: 0x04001657 RID: 5719
		public float pitchFactor;

		// Token: 0x04001658 RID: 5720
		public bool testPlay;
	}
}
