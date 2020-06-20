using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200050D RID: 1293
	public class SustainerManager
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000DBAE0 File Offset: 0x000D9CE0
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000DBAE8 File Offset: 0x000D9CE8
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000DBAF6 File Offset: 0x000D9CF6
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000DBB08 File Offset: 0x000D9D08
		public bool SustainerExists(SoundDef def)
		{
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				if (this.allSustainers[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x000DBB44 File Offset: 0x000D9D44
		public void SustainerManagerUpdate()
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000DBB80 File Offset: 0x000D9D80
		public void UpdateAllSustainerScopes()
		{
			SustainerManager.playingPerDef.Clear();
			for (int i = 0; i < this.allSustainers.Count; i++)
			{
				Sustainer sustainer = this.allSustainers[i];
				if (!SustainerManager.playingPerDef.ContainsKey(sustainer.def))
				{
					List<Sustainer> list = SimplePool<List<Sustainer>>.Get();
					list.Add(sustainer);
					SustainerManager.playingPerDef.Add(sustainer.def, list);
				}
				else
				{
					SustainerManager.playingPerDef[sustainer.def].Add(sustainer);
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair in SustainerManager.playingPerDef)
			{
				SoundDef key = keyValuePair.Key;
				List<Sustainer> value = keyValuePair.Value;
				if (value.Count - key.maxVoices < 0)
				{
					for (int j = 0; j < value.Count; j++)
					{
						value[j].scopeFader.inScope = true;
					}
				}
				else
				{
					for (int k = 0; k < value.Count; k++)
					{
						value[k].scopeFader.inScope = false;
					}
					value.Sort(SustainerManager.SortSustainersByCameraDistanceCached);
					int num = 0;
					for (int l = 0; l < value.Count; l++)
					{
						value[l].scopeFader.inScope = true;
						num++;
						if (num >= key.maxVoices)
						{
							break;
						}
					}
					for (int m = 0; m < value.Count; m++)
					{
						if (!value[m].scopeFader.inScope)
						{
							value[m].scopeFader.inScopePercent = 0f;
						}
					}
				}
			}
			foreach (KeyValuePair<SoundDef, List<Sustainer>> keyValuePair2 in SustainerManager.playingPerDef)
			{
				keyValuePair2.Value.Clear();
				SimplePool<List<Sustainer>>.Return(keyValuePair2.Value);
			}
			SustainerManager.playingPerDef.Clear();
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x000DBDC8 File Offset: 0x000D9FC8
		public void EndAllInMap(Map map)
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				if (this.allSustainers[i].info.Maker.Map == map)
				{
					this.allSustainers[i].End();
				}
			}
		}

		// Token: 0x0400167F RID: 5759
		private List<Sustainer> allSustainers = new List<Sustainer>();

		// Token: 0x04001680 RID: 5760
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		// Token: 0x04001681 RID: 5761
		private static readonly Comparison<Sustainer> SortSustainersByCameraDistanceCached = (Sustainer a, Sustainer b) => a.CameraDistanceSquared.CompareTo(b.CameraDistanceSquared);
	}
}
