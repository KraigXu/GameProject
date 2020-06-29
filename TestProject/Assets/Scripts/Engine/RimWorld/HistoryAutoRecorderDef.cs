using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class HistoryAutoRecorderDef : Def
	{
		
		
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		
		public Type workerClass;

		
		public int recordTicksFrequency = 60000;

		
		public Color graphColor = Color.green;

		
		[MustTranslate]
		public string graphLabelY;

		
		public string valueFormat;

		
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
