using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F24 RID: 3876
	public abstract class Lesson : IExposable
	{
		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x0020C5B4 File Offset: 0x0020A7B4
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ExposeData()
		{
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x0020C5DA File Offset: 0x0020A7DA
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06005EE7 RID: 24295
		public abstract void LessonOnGUI();

		// Token: 0x06005EE8 RID: 24296 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x0020C5E7 File Offset: 0x0020A7E7
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06005EEC RID: 24300 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04003373 RID: 13171
		public float startRealTime = -999f;

		// Token: 0x04003374 RID: 13172
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
