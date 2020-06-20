using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000043 RID: 67
	public class SimpleLinearPool<T> where T : new()
	{
		// Token: 0x0600037E RID: 894 RVA: 0x00012778 File Offset: 0x00010978
		public T Get()
		{
			if (this.readIndex >= this.items.Count)
			{
				this.items.Add(Activator.CreateInstance<T>());
			}
			List<T> list = this.items;
			int num = this.readIndex;
			this.readIndex = num + 1;
			return list[num];
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000127C4 File Offset: 0x000109C4
		public void Clear()
		{
			this.readIndex = 0;
		}

		// Token: 0x040000F6 RID: 246
		private List<T> items = new List<T>();

		// Token: 0x040000F7 RID: 247
		private int readIndex;
	}
}
