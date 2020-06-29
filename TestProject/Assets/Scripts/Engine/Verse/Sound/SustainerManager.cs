using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	
	public class SustainerManager
	{
		
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x000DBAE0 File Offset: 0x000D9CE0
		public List<Sustainer> AllSustainers
		{
			get
			{
				return this.allSustainers;
			}
		}

		
		public void RegisterSustainer(Sustainer newSustainer)
		{
			this.allSustainers.Add(newSustainer);
		}

		
		public void DeregisterSustainer(Sustainer oldSustainer)
		{
			this.allSustainers.Remove(oldSustainer);
		}

		
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

		
		public void SustainerManagerUpdate()
		{
			for (int i = this.allSustainers.Count - 1; i >= 0; i--)
			{
				this.allSustainers[i].SustainerUpdate();
			}
			this.UpdateAllSustainerScopes();
		}

		
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

		
		private List<Sustainer> allSustainers = new List<Sustainer>();

		
		private static Dictionary<SoundDef, List<Sustainer>> playingPerDef = new Dictionary<SoundDef, List<Sustainer>>();

		
		private static readonly Comparison<Sustainer> SortSustainersByCameraDistanceCached = (Sustainer a, Sustainer b) => a.CameraDistanceSquared.CompareTo(b.CameraDistanceSquared);
	}
}
