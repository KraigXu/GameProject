using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class Lesson : IExposable
	{
		
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

		
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06005EE2 RID: 24290 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x06005EE3 RID: 24291 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		
		public virtual void ExposeData()
		{
		}

		
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		
		public virtual void PostDeactivated()
		{
		}

		
		public abstract void LessonOnGUI();

		
		public virtual void LessonUpdate()
		{
		}

		
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		
		public virtual void Notify_Event(EventPack ep)
		{
		}

		
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		
		// (get) Token: 0x06005EEC RID: 24300 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		
		public float startRealTime = -999f;

		
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
