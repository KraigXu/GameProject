using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AC RID: 172
	public class DesignationDef : Def
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x0001B211 File Offset: 0x00019411
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x04000379 RID: 889
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400037A RID: 890
		public TargetType targetType;

		// Token: 0x0400037B RID: 891
		public bool removeIfBuildingDespawned;

		// Token: 0x0400037C RID: 892
		public bool designateCancelable = true;

		// Token: 0x0400037D RID: 893
		[Unsaved(false)]
		public Material iconMat;
	}
}
