using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200003E RID: 62
	public static class ShaderUtility
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0001251B File Offset: 0x0001071B
		public static bool SupportsMaskTex(this Shader shader)
		{
			return shader == ShaderDatabase.CutoutComplex;
		}
	}
}
