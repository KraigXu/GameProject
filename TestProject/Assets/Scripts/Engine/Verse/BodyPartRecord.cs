using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	
	public class BodyPartRecord
	{
		
		
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		
		
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

		
		
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		
		
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		
		
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
