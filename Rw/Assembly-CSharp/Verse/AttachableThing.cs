using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DD RID: 733
	public abstract class AttachableThing : Thing
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0007940D File Offset: 0x0007760D
		public override Vector3 DrawPos
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.DrawPos + Vector3.up * 0.0454545468f * 0.9f;
				}
				return base.DrawPos;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001488 RID: 5256
		public abstract string InspectStringAddon { get; }

		// Token: 0x06001489 RID: 5257 RVA: 0x00079447 File Offset: 0x00077647
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0007947C File Offset: 0x0007767C
		public virtual void AttachTo(Thing parent)
		{
			this.parent = parent;
			CompAttachBase compAttachBase = parent.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot attach ",
					this,
					" to ",
					parent,
					": parent has no CompAttachBase."
				}), false);
				return;
			}
			compAttachBase.AddAttachment(this);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000794D3 File Offset: 0x000776D3
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		// Token: 0x04000DBA RID: 3514
		public Thing parent;
	}
}
