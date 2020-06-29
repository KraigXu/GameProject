using System;
using System.Collections.Generic;

namespace Verse
{
	
	public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
	{
		
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000C1A14 File Offset: 0x000BFC14
		public T1 First
		{
			get
			{
				return this.first;
			}
		}

		
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000C1A1C File Offset: 0x000BFC1C
		public T2 Second
		{
			get
			{
				return this.second;
			}
		}

		
		public Pair(T1 first, T2 second)
		{
			this.first = first;
			this.second = second;
		}

		
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

		
		public override int GetHashCode()
		{
			return Gen.HashCombine<T2>(Gen.HashCombine<T1>(0, this.first), this.second);
		}

		
		public override bool Equals(object other)
		{
			return other is Pair<T1, T2> && this.Equals((Pair<T1, T2>)other);
		}

		
		public bool Equals(Pair<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.first, other.first) && EqualityComparer<T2>.Default.Equals(this.second, other.second);
		}

		
		public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return lhs.Equals(rhs);
		}

		
		public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
		{
			return !(lhs == rhs);
		}

		
		private T1 first;

		
		private T2 second;
	}
}
