using System;

namespace Verse
{
	// Token: 0x02000149 RID: 329
	public static class TranslatorFormattedStringExtensions
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x000331A5 File Offset: 0x000313A5
		public static TaggedString Translate(this string key, NamedArgument arg1)
		{
			return key.Translate().Formatted(arg1);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000331B3 File Offset: 0x000313B3
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2)
		{
			return key.Translate().Formatted(arg1, arg2);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000331C2 File Offset: 0x000313C2
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			return key.Translate().Formatted(arg1, arg2, arg3);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000331D2 File Offset: 0x000313D2
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000331E4 File Offset: 0x000313E4
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000331F8 File Offset: 0x000313F8
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0003320E File Offset: 0x0003140E
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00033228 File Offset: 0x00031428
		public static TaggedString Translate(this string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4, NamedArgument arg5, NamedArgument arg6, NamedArgument arg7, NamedArgument arg8)
		{
			return key.Translate().Formatted(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0003324D File Offset: 0x0003144D
		public static TaggedString Translate(this string key, params NamedArgument[] args)
		{
			return key.Translate().Formatted(args);
		}
	}
}
