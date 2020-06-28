using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000128 RID: 296
	public struct NamedArgument
	{
		// Token: 0x06000859 RID: 2137 RVA: 0x00029969 File Offset: 0x00027B69
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00029979 File Offset: 0x00027B79
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00029987 File Offset: 0x00027B87
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00029995 File Offset: 0x00027B95
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000299A3 File Offset: 0x00027BA3
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000299B1 File Offset: 0x00027BB1
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000299C8 File Offset: 0x00027BC8
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000299D6 File Offset: 0x00027BD6
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000299E4 File Offset: 0x00027BE4
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x000299F2 File Offset: 0x00027BF2
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00029A00 File Offset: 0x00027C00
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00029A0E File Offset: 0x00027C0E
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00029A1C File Offset: 0x00027C1C
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000299BF File Offset: 0x00027BBF
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00029A2A File Offset: 0x00027C2A
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00029A38 File Offset: 0x00027C38
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x0400074E RID: 1870
		public object arg;

		// Token: 0x0400074F RID: 1871
		public string label;
	}
}
