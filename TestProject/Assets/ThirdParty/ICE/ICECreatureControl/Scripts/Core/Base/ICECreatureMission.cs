// ##############################################################################
//
// ICECreatureMission.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures
{
	/// <summary>
	/// ICECreatureMission is the base class for all mission extensions of ICE. 
	/// </summary>
	public class ICECreatureMission : ICEWorldBehaviour 
	{
		[SerializeField]
		private ICECreatureControl m_CreatureControl = null;
		public virtual ICECreatureControl CreatureControl{
			get{ return m_CreatureControl = ( m_CreatureControl == null ? this.gameObject.GetComponent<ICECreatureControl>() : m_CreatureControl ); }
		}

		[SerializeField]
		private TargetObject m_Target = null;
		public virtual TargetObject Target{
			get{ return m_Target = ( m_Target == null ? new TargetObject() : m_Target ); }
			set{ Target.Copy( value ); }
		}

		public virtual bool IsValidAndReady(){
			return ( this.enabled == true && Target.IsValidAndReady ? true : false );
		}

		public virtual void Reset()
		{
			if( Target != null )
				Target.Reset();
		}

		public virtual TargetObject PrepareTarget()
		{
			if( ! this.enabled || CreatureControl == null || Target.PrepareTargetGameObject( CreatureControl ) == null || ! Target.IsValidAndReady )
				return null;

			Target.Behaviour.SetDefault();

			return Target;
		}
	}
}
