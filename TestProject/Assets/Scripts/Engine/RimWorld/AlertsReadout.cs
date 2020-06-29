using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class AlertsReadout
	{
		
		public AlertsReadout()
		{
			this.AllAlerts.Clear();
			foreach (Type type in typeof(Alert).AllLeafSubclasses())
			{
				if (!(type == typeof(Alert_Custom)) && !(type == typeof(Alert_CustomCritical)))
				{
					this.AllAlerts.Add((Alert)Activator.CreateInstance(type));
				}
			}
			if (this.PriosInDrawOrder == null)
			{
				this.PriosInDrawOrder = new List<AlertPriority>();
				foreach (object obj in Enum.GetValues(typeof(AlertPriority)))
				{
					AlertPriority item = (AlertPriority)obj;
					this.PriosInDrawOrder.Add(item);
				}
				this.PriosInDrawOrder.Reverse();
			}
		}

		
		public void AlertsReadoutUpdate()
		{
			if (Mathf.Max(Find.TickManager.TicksGame, Find.TutorialState.endTick) < 600)
			{
				return;
			}
			if (Find.Storyteller.def.disableAlerts)
			{
				this.activeAlerts.Clear();
				return;
			}
			this.curAlertIndex++;
			if (this.curAlertIndex >= 24)
			{
				this.curAlertIndex = 0;
			}
			for (int i = this.curAlertIndex; i < this.AllAlerts.Count; i += 24)
			{
				this.CheckAddOrRemoveAlert(this.AllAlerts[i], false);
			}
			if (Time.frameCount % 20 == 0)
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int j = 0; j < questsListForReading.Count; j++)
				{
					List<QuestPart> partsListForReading = questsListForReading[j].PartsListForReading;
					for (int k = 0; k < partsListForReading.Count; k++)
					{
						QuestPartActivable questPartActivable = partsListForReading[k] as QuestPartActivable;
						if (questPartActivable != null)
						{
							Alert cachedAlert = questPartActivable.CachedAlert;
							if (cachedAlert != null)
							{
								bool flag = questsListForReading[j].State != QuestState.Ongoing || questPartActivable.State != QuestPartState.Enabled;
								bool alertDirty = questPartActivable.AlertDirty;
								this.CheckAddOrRemoveAlert(cachedAlert, flag || alertDirty);
								if (alertDirty)
								{
									questPartActivable.ClearCachedAlert();
								}
							}
						}
					}
				}
			}
			for (int l = this.activeAlerts.Count - 1; l >= 0; l--)
			{
				Alert alert = this.activeAlerts[l];
				try
				{
					this.activeAlerts[l].AlertActiveUpdate();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce("Exception updating alert " + alert.ToString() + ": " + ex.ToString(), 743575, false);
					this.activeAlerts.RemoveAt(l);
				}
			}
			if (this.mouseoverAlertIndex >= 0 && this.mouseoverAlertIndex < this.activeAlerts.Count)
			{
				IEnumerable<GlobalTargetInfo> allCulprits = this.activeAlerts[this.mouseoverAlertIndex].GetReport().AllCulprits;
				if (allCulprits != null)
				{
					foreach (GlobalTargetInfo target in allCulprits)
					{
						TargetHighlighter.Highlight(target, true, true, false);
					}
				}
			}
			this.mouseoverAlertIndex = -1;
		}

		
		private void CheckAddOrRemoveAlert(Alert alert, bool forceRemove = false)
		{
			try
			{
				alert.Recalculate();
				if (!forceRemove && alert.Active)
				{
					if (!this.activeAlerts.Contains(alert))
					{
						this.activeAlerts.Add(alert);
						alert.Notify_Started();
					}
				}
				else
				{
					this.activeAlerts.Remove(alert);
				}
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception processing alert " + alert.ToString() + ": " + ex.ToString(), 743575, false);
				this.activeAlerts.Remove(alert);
			}
		}

		
		public void AlertsReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout || Event.current.type == EventType.MouseDrag)
			{
				return;
			}
			if (this.activeAlerts.Count == 0)
			{
				return;
			}
			Alert alert = null;
			AlertPriority alertPriority = AlertPriority.Critical;
			bool flag = false;
			float num = 0f;
			for (int i = 0; i < this.activeAlerts.Count; i++)
			{
				num += this.activeAlerts[i].Height;
			}
			float num2 = Find.LetterStack.LastTopY - num;
			Rect rect = new Rect((float)UI.screenWidth - 154f, num2, 154f, this.lastFinalY - num2);
			float num3 = GenUI.BackgroundDarkAlphaForText();
			if (num3 > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num3);
				Widgets.DrawShadowAround(rect);
				GUI.color = Color.white;
			}
			float num4 = num2;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			for (int j = 0; j < this.PriosInDrawOrder.Count; j++)
			{
				AlertPriority alertPriority2 = this.PriosInDrawOrder[j];
				for (int k = 0; k < this.activeAlerts.Count; k++)
				{
					Alert alert2 = this.activeAlerts[k];
					if (alert2.Priority == alertPriority2)
					{
						if (!flag)
						{
							alertPriority = alertPriority2;
							flag = true;
						}
						Rect rect2 = alert2.DrawAt(num4, alertPriority2 != alertPriority);
						if (Mouse.IsOver(rect2))
						{
							alert = alert2;
							this.mouseoverAlertIndex = k;
						}
						num4 += rect2.height;
					}
				}
			}
			this.lastFinalY = num4;
			UIHighlighter.HighlightOpportunity(rect, "Alerts");
			if (alert != null)
			{
				alert.DrawInfoPane();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Alerts, KnowledgeAmount.FrameDisplayed);
				this.CheckAddOrRemoveAlert(alert, false);
			}
		}

		
		private List<Alert> activeAlerts = new List<Alert>(16);

		
		private int curAlertIndex;

		
		private float lastFinalY;

		
		private int mouseoverAlertIndex = -1;

		
		private readonly List<Alert> AllAlerts = new List<Alert>();

		
		private const int StartTickDelay = 600;

		
		public const float AlertListWidth = 164f;

		
		private const int AlertCycleLength = 24;

		
		private const int UpdateAlertsFromQuestsIntervalFrames = 20;

		
		private readonly List<AlertPriority> PriosInDrawOrder;
	}
}
