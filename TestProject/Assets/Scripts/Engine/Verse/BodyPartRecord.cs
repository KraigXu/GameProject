using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	
	public class BodyPartRecord
	{
		
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00016E65 File Offset: 0x00015065
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		
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

		
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00016ED0 File Offset: 0x000150D0
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00016EDD File Offset: 0x000150DD
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00016EEA File Offset: 0x000150EA
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		
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

		
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		
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

		
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		
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

		
		public BodyDef body;

		
		[TranslationHandle]
		public BodyPartDef def;

		
		[MustTranslate]
		public string customLabel;

		
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		
		public BodyPartHeight height;

		
		public BodyPartDepth depth;

		
		public float coverage = 1f;

		
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		
		[Unsaved(false)]
		public BodyPartRecord parent;

		
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		
		[Unsaved(false)]
		public float coverageAbs;

		
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}
