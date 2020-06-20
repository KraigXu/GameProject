using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005DE RID: 1502
	public class Transition
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000F54B8 File Offset: 0x000F36B8
		public Map Map
		{
			get
			{
				return this.target.Map;
			}
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000F54C8 File Offset: 0x000F36C8
		public Transition(LordToil firstSource, LordToil target, bool canMoveToSameState = false, bool updateDutiesIfMovedToSameState = true)
		{
			this.canMoveToSameState = canMoveToSameState;
			this.updateDutiesIfMovedToSameState = updateDutiesIfMovedToSameState;
			this.target = target;
			this.sources = new List<LordToil>();
			this.AddSource(firstSource);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000F552C File Offset: 0x000F372C
		public void AddSource(LordToil source)
		{
			if (this.sources.Contains(source))
			{
				Log.Error("Double-added source to Transition: " + source, false);
				return;
			}
			if (!this.canMoveToSameState && this.target == source)
			{
				Log.Error("Transition !canMoveToSameState and target is source: " + source, false);
			}
			this.sources.Add(source);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000F5588 File Offset: 0x000F3788
		public void AddSources(IEnumerable<LordToil> sources)
		{
			foreach (LordToil source in sources)
			{
				this.AddSource(source);
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000F55D0 File Offset: 0x000F37D0
		public void AddSources(params LordToil[] sources)
		{
			for (int i = 0; i < sources.Length; i++)
			{
				this.AddSource(sources[i]);
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000F55F4 File Offset: 0x000F37F4
		public void AddTrigger(Trigger trigger)
		{
			this.triggers.Add(trigger);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000F5602 File Offset: 0x000F3802
		public void AddPreAction(TransitionAction action)
		{
			this.preActions.Add(action);
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000F5610 File Offset: 0x000F3810
		public void AddPostAction(TransitionAction action)
		{
			this.postActions.Add(action);
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000F5620 File Offset: 0x000F3820
		public void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				this.triggers[i].SourceToilBecameActive(transition, previousToil);
			}
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000F5658 File Offset: 0x000F3858
		public bool CheckSignal(Lord lord, TriggerSignal signal)
		{
			for (int i = 0; i < this.triggers.Count; i++)
			{
				if (this.triggers[i].ActivateOn(lord, signal))
				{
					if (this.triggers[i].filters != null)
					{
						bool flag = true;
						for (int j = 0; j < this.triggers[i].filters.Count; j++)
						{
							if (!this.triggers[i].filters[j].AllowActivation(lord, signal))
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							goto IL_E7;
						}
					}
					if (DebugViewSettings.logLordToilTransitions)
					{
						Log.Message(string.Concat(new object[]
						{
							"Transitioning ",
							this.sources,
							" to ",
							this.target,
							" by trigger ",
							this.triggers[i],
							" on signal ",
							signal
						}), false);
					}
					this.Execute(lord);
					return true;
				}
				IL_E7:;
			}
			return false;
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000F5764 File Offset: 0x000F3964
		public void Execute(Lord lord)
		{
			if (!this.canMoveToSameState && this.target == lord.CurLordToil)
			{
				return;
			}
			for (int i = 0; i < this.preActions.Count; i++)
			{
				this.preActions[i].DoAction(this);
			}
			if (this.target != lord.CurLordToil || this.updateDutiesIfMovedToSameState)
			{
				lord.GotoToil(this.target);
			}
			for (int j = 0; j < this.postActions.Count; j++)
			{
				this.postActions[j].DoAction(this);
			}
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000F57FC File Offset: 0x000F39FC
		public override string ToString()
		{
			string text = this.sources.NullOrEmpty<LordToil>() ? "null" : this.sources[0].ToString();
			int num = (this.sources == null) ? 0 : this.sources.Count;
			string text2 = (this.target == null) ? "null" : this.target.ToString();
			return string.Concat(new object[]
			{
				text,
				"(",
				num,
				")->",
				text2
			});
		}

		// Token: 0x04001907 RID: 6407
		public List<LordToil> sources;

		// Token: 0x04001908 RID: 6408
		public LordToil target;

		// Token: 0x04001909 RID: 6409
		public List<Trigger> triggers = new List<Trigger>();

		// Token: 0x0400190A RID: 6410
		public List<TransitionAction> preActions = new List<TransitionAction>();

		// Token: 0x0400190B RID: 6411
		public List<TransitionAction> postActions = new List<TransitionAction>();

		// Token: 0x0400190C RID: 6412
		public bool canMoveToSameState;

		// Token: 0x0400190D RID: 6413
		public bool updateDutiesIfMovedToSameState = true;
	}
}
