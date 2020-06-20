using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x020004BD RID: 1213
	public class Terrace : ModuleBase
	{
		// Token: 0x060023B8 RID: 9144 RVA: 0x000D5FE4 File Offset: 0x000D41E4
		public Terrace() : base(1)
		{
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000D5FF8 File Offset: 0x000D41F8
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000D6015 File Offset: 0x000D4215
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x000D6039 File Offset: 0x000D4239
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000D6046 File Offset: 0x000D4246
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x000D604E File Offset: 0x000D424E
		// (set) Token: 0x060023BE RID: 9150 RVA: 0x000D6056 File Offset: 0x000D4256
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000D6060 File Offset: 0x000D4260
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000D60B1 File Offset: 0x000D42B1
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000D60C0 File Offset: 0x000D42C0
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000D611C File Offset: 0x000D431C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num])
			{
				num++;
			}
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num3];
			}
			double num4 = this.m_data[num2];
			double num5 = this.m_data[num3];
			double num6 = (value - num4) / (num5 - num4);
			if (this.m_inverted)
			{
				num6 = 1.0 - num6;
				double num7 = num4;
				num4 = num5;
				num5 = num7;
			}
			num6 *= num6;
			return Utils.InterpolateLinear(num4, num5, num6);
		}

		// Token: 0x04001591 RID: 5521
		private List<double> m_data = new List<double>();

		// Token: 0x04001592 RID: 5522
		private bool m_inverted;
	}
}
