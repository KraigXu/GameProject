using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class IncidentQueue : IExposable
	{
		
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x0013BBBF File Offset: 0x00139DBF
		public int Count
		{
			get
			{
				return this.queuedIncidents.Count;
			}
		}

		
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x0013BBCC File Offset: 0x00139DCC
		public string DebugQueueReadout
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (QueuedIncident queuedIncident in this.queuedIncidents)
				{
					stringBuilder.AppendLine(queuedIncident.ToString() + " (in " + (queuedIncident.FireTick - Find.TickManager.TicksGame).ToString() + " ticks)");
				}
				return stringBuilder.ToString();
			}
		}

		
		public IEnumerator GetEnumerator()
		{
			foreach (QueuedIncident queuedIncident in this.queuedIncidents)
			{
				yield return queuedIncident;
			}
			List<QueuedIncident>.Enumerator enumerator = default(List<QueuedIncident>.Enumerator);
			yield break;
			yield break;
		}

		
		public void Clear()
		{
			this.queuedIncidents.Clear();
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<QueuedIncident>(ref this.queuedIncidents, "queuedIncidents", LookMode.Deep, Array.Empty<object>());
		}

		
		public bool Add(QueuedIncident qi)
		{
			this.queuedIncidents.Add(qi);
			this.queuedIncidents.Sort((QueuedIncident a, QueuedIncident b) => a.FireTick.CompareTo(b.FireTick));
			return true;
		}

		
		public bool Add(IncidentDef def, int fireTick, IncidentParms parms = null, int retryDurationTicks = 0)
		{
			QueuedIncident qi = new QueuedIncident(new FiringIncident(def, null, parms), fireTick, retryDurationTicks);
			this.Add(qi);
			return true;
		}

		
		public void IncidentQueueTick()
		{
			for (int i = this.queuedIncidents.Count - 1; i >= 0; i--)
			{
				QueuedIncident queuedIncident = this.queuedIncidents[i];
				if (!queuedIncident.TriedToFire)
				{
					if (queuedIncident.FireTick <= Find.TickManager.TicksGame)
					{
						bool flag = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
						queuedIncident.Notify_TriedToFire();
						if (flag || queuedIncident.RetryDurationTicks == 0)
						{
							this.queuedIncidents.Remove(queuedIncident);
						}
					}
				}
				else if (queuedIncident.FireTick + queuedIncident.RetryDurationTicks <= Find.TickManager.TicksGame)
				{
					this.queuedIncidents.Remove(queuedIncident);
				}
				else if (Find.TickManager.TicksGame % 833 == Rand.RangeSeeded(0, 833, queuedIncident.FireTick))
				{
					bool flag2 = Find.Storyteller.TryFire(queuedIncident.FiringIncident);
					queuedIncident.Notify_TriedToFire();
					if (flag2)
					{
						this.queuedIncidents.Remove(queuedIncident);
					}
				}
			}
		}

		
		public void Notify_MapRemoved(Map map)
		{
			this.queuedIncidents.RemoveAll((QueuedIncident x) => x.FiringIncident.parms.target == map);
		}

		
		private List<QueuedIncident> queuedIncidents = new List<QueuedIncident>();
	}
}
