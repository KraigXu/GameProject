using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x0200031A RID: 794
	public class CompAttachBase : ThingComp
	{
		// Token: 0x06001737 RID: 5943 RVA: 0x00085368 File Offset: 0x00083568
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000853B0 File Offset: 0x000835B0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x000853F8 File Offset: 0x000835F8
		public override string CompInspectStringExtra()
		{
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return null;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00085450 File Offset: 0x00083650
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x0008549D File Offset: 0x0008369D
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x000854A9 File Offset: 0x000836A9
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000854CA File Offset: 0x000836CA
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x04000EA0 RID: 3744
		public List<AttachableThing> attachments;
	}
}
