using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x020012BA RID: 4794
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x06007190 RID: 29072 RVA: 0x0027C3E9 File Offset: 0x0027A5E9
		public ZlibException()
		{
		}

		// Token: 0x06007191 RID: 29073 RVA: 0x0027C3F1 File Offset: 0x0027A5F1
		public ZlibException(string s) : base(s)
		{
		}
	}
}
