using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class AttachableThing : Thing
	{
		
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

		
		// (get) Token: 0x06001488 RID: 5256
		public abstract string InspectStringAddon { get; }

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent != null)
			{
				this.AttachTo(this.parent);
			}
		}

		
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

		
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.parent != null)
			{
				this.parent.TryGetComp<CompAttachBase>().RemoveAttachment(this);
			}
		}

		
		public Thing parent;
	}
}
