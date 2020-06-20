using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000072 RID: 114
	public class BodyPartRecord
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00016E65 File Offset: 0x00015065
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00016E70 File Offset: 0x00015070
		public string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				return this.def.label;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00016E91 File Offset: 0x00015091
		public string LabelCap
		{
			get
			{
				if (this.customLabel.NullOrEmpty())
				{
					return this.def.LabelCap;
				}
				if (this.cachedCustomLabelCap == null)
				{
					this.cachedCustomLabelCap = this.customLabel.CapitalizeFirst();
				}
				return this.cachedCustomLabelCap;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00016ED0 File Offset: 0x000150D0
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00016EDD File Offset: 0x000150DD
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00016EEA File Offset: 0x000150EA
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00016EF8 File Offset: 0x000150F8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"BodyPartRecord(",
				(this.def != null) ? this.def.defName : "NULL_DEF",
				" parts.Count=",
				this.parts.Count,
				")"
			});
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00016F58 File Offset: 0x00015158
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00016F68 File Offset: 0x00015168
		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00016F9D File Offset: 0x0001519D
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetChildParts(tag))
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00016FB4 File Offset: 0x000151B4
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				yield return this.parts[i];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00016FC4 File Offset: 0x000151C4
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00016FD2 File Offset: 0x000151D2
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord bodyPartRecord = this;
			while (bodyPartRecord.parent != null && bodyPartRecord.parent.def.tags.Contains(tag))
			{
				bodyPartRecord = bodyPartRecord.parent;
			}
			foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.GetChildParts(tag))
			{
				yield return bodyPartRecord2;
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400017C RID: 380
		public BodyDef body;

		// Token: 0x0400017D RID: 381
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x0400017E RID: 382
		[MustTranslate]
		public string customLabel;

		// Token: 0x0400017F RID: 383
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x04000180 RID: 384
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04000181 RID: 385
		public BodyPartHeight height;

		// Token: 0x04000182 RID: 386
		public BodyPartDepth depth;

		// Token: 0x04000183 RID: 387
		public float coverage = 1f;

		// Token: 0x04000184 RID: 388
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x04000185 RID: 389
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x04000186 RID: 390
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x04000187 RID: 391
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x04000188 RID: 392
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}
