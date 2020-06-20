using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004AC RID: 1196
	public class Curve : ModuleBase
	{
		// Token: 0x0600235A RID: 9050 RVA: 0x000D538B File Offset: 0x000D358B
		public Curve() : base(1)
		{
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000D539F File Offset: 0x000D359F
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000D53BC File Offset: 0x000D35BC
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x000D53C9 File Offset: 0x000D35C9
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x000D53D4 File Offset: 0x000D35D4
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x000D542E File Offset: 0x000D362E
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x000D543C File Offset: 0x000D363C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num].Key)
			{
				num++;
			}
			int index = Mathf.Clamp(num - 2, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(num + 1, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num2].Value;
			}
			double key = this.m_data[num2].Key;
			double key2 = this.m_data[num3].Key;
			double position = (value - key) / (key2 - key);
			return Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num2].Value, this.m_data[num3].Value, this.m_data[index2].Value, position);
		}

		// Token: 0x04001576 RID: 5494
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
