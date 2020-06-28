using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000420 RID: 1056
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000C1A14 File Offset: 0x000BFC14
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000C1A1C File Offset: 0x000BFC1C
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000C1A24 File Offset: 0x000BFC24
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x000C1A34 File Offset: 0x000BFC34
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			T1 t = this.First;
			array[num] = t.ToString();
			array[2] = ", ";
			int num2 = 3;
			T2 t2 = this.Second;
			array[num2] = t2.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x000C1A92 File Offset: 0x000BFC92
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000C1AAB File Offset: 0x000BFCAB
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000C1AC3 File Offset: 0x000BFCC3
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000C1AF5 File Offset: 0x000BFCF5
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x000C1AFF File Offset: 0x000BFCFF
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001326 RID: 4902
		private T1 first;

		// Token: 0x04001327 RID: 4903
		private T2 second;
	}
}
