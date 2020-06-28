using System;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public static class AttachmentUtility
	{
		// Token: 0x0600173F RID: 5951 RVA: 0x000854DC File Offset: 0x000836DC
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x000854FC File Offset: 0x000836FC
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
