using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	
	public class BodyDef : Def
	{
		
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00017012 File Offset: 0x00015212
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x0001701A File Offset: 0x0001521A
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		
		public IEnumerable<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			int num;
			for (int i = 0; i < this.AllParts.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def.tags.Contains(tag))
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		
		public IEnumerable<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			int num;
			for (int i = 0; i < this.AllParts.Count; i = num + 1)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def == def)
				{
					yield return bodyPartRecord;
				}
				num = i;
			}
			yield break;
		}

		
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				if (this.AllParts[i].def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty<BodyPartRecord>())
			{
				yield return "no parts vulnerable to frostbite";
			}
			foreach (BodyPartRecord bodyPartRecord in this.AllParts)
			{
				if (bodyPartRecord.def.conceptual && bodyPartRecord.coverageAbs != 0f)
				{
					yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", bodyPartRecord);
				}
				else if (Prefs.DevMode && !bodyPartRecord.def.conceptual)
				{
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.parts)
					{
						num += bodyPartRecord2.coverage;
					}
					if (num >= 1f)
					{
						Log.Warning(string.Concat(new string[]
						{
							"BodyDef ",
							this.defName,
							" has BodyPartRecord of ",
							bodyPartRecord.def.defName,
							" whose children have more or equal coverage than 100% (",
							(num * 100f).ToString("0.00"),
							"%)"
						}), false);
					}
				}
			}
			List<BodyPartRecord>.Enumerator enumerator2 = default(List<BodyPartRecord>.Enumerator);
			yield break;
			yield break;
		}

		
		public override void ResolveReferences()
		{
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + this, false);
				return;
			}
			node.body = this;
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (Mathf.Abs(num) < 1E-05f)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				num = 0f;
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}

		
		public BodyPartRecord corePart;

		
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;
	}
}
