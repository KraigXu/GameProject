using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200092B RID: 2347
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x060037C8 RID: 14280 RVA: 0x0012B4E8 File Offset: 0x001296E8
		public void Tick()
		{
			if (Find.TickManager.TicksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0012B538 File Offset: 0x00129738
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] recordsFromBytes = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				recordsFromBytes = this.RecordsToBytes();
			}
			DataExposeUtility.ByteArray(ref recordsFromBytes, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetRecordsFromBytes(recordsFromBytes);
			}
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x0012B584 File Offset: 0x00129784
		private byte[] RecordsToBytes()
		{
			byte[] array = new byte[this.records.Count * 4];
			for (int i = 0; i < this.records.Count; i++)
			{
				byte[] bytes = BitConverter.GetBytes(this.records[i]);
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = bytes[j];
				}
			}
			return array;
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0012B5E4 File Offset: 0x001297E4
		private void SetRecordsFromBytes(byte[] bytes)
		{
			int num = bytes.Length / 4;
			this.records.Clear();
			for (int i = 0; i < num; i++)
			{
				float item = BitConverter.ToSingle(bytes, i * 4);
				this.records.Add(item);
			}
		}

		// Token: 0x04002109 RID: 8457
		public HistoryAutoRecorderDef def;

		// Token: 0x0400210A RID: 8458
		public List<float> records = new List<float>();
	}
}
