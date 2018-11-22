// ##############################################################################
//
// ice_objects_tools.cs | ICE.World.Objects.DamageImpactDataObject | ICE.World.Objects.DamageImpactObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class ToolObject : ICEOwnerObject{

		public ToolObject(){}
		public ToolObject( ToolObject _object ) : base( _object ){ Copy( _object ); }
		public ToolObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }

		public override void Init( ICEWorldBehaviour _component  )
		{
			base.Init( _component );

			StandByBehaviour.Init( _component );
			OperationBehaviour.Init( _component );
		}

		public override void Reset()
		{
			base.Reset();

			StandByBehaviour.Reset();
			OperationBehaviour.Reset();
		}

		public void Copy( ToolObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			StandByBehaviour = _object.StandByBehaviour;
			OperationBehaviour = _object.OperationBehaviour;

		}

		[SerializeField]
		private ToolBehaviourObject m_StandByBehaviour = null;
		public ToolBehaviourObject StandByBehaviour{
			get{ return m_StandByBehaviour = ( m_StandByBehaviour == null ? new ToolBehaviourObject() : m_StandByBehaviour); }
			set{ StandByBehaviour.Copy( value ); }
		}

		[SerializeField]
		private ToolBehaviourObject m_OperationBehaviour = null;
		public ToolBehaviourObject OperationBehaviour{
			get{ return m_OperationBehaviour = ( m_OperationBehaviour == null ? new ToolBehaviourObject() : m_OperationBehaviour ); }
			set{ OperationBehaviour.Copy( value ); }
		}

		public void Stop()
		{
			StandByBehaviour.Stop();
			OperationBehaviour.Stop();
		}

		public void StandBy()
		{
			StandByBehaviour.Start();
			OperationBehaviour.Stop();
		}

		public void Operate()
		{
			StandByBehaviour.Stop();
			OperationBehaviour.Start();
		}

	}

	[System.Serializable]
	public class ToolBehaviourDataObject : ICEOwnerObject{

		public ToolBehaviourDataObject(){}
		public ToolBehaviourDataObject( ToolBehaviourDataObject _object ) : base( _object ){ Copy( _object ); }
		public ToolBehaviourDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }

		public override void Init( ICEWorldBehaviour _component  )
		{
			base.Init( _component );

			Sound.Init( _component );
			Effect.Init( _component );
		}

		public override void Reset()
		{
			base.Reset();

			Sound.Reset();
			Effect.Reset();
		}

		public void Copy( ToolBehaviourDataObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

		}

		[SerializeField]
		private DirectAudioPlayerObject m_Sound = null;
		public DirectAudioPlayerObject Sound{
			get{ return m_Sound = ( m_Sound == null ? new DirectAudioPlayerObject( OwnerComponent ) : m_Sound); }
			set{ Sound.Copy( value ); }
		}

		[SerializeField]
		private EffectObject m_Effect = null;
		public EffectObject Effect{
			get{ return m_Effect = ( m_Effect == null ? new EffectObject( OwnerComponent ) : m_Effect); }
			set{ Effect.Copy( value ); }
		}
	}

	[System.Serializable]
	public class ToolBehaviourObject : ToolBehaviourDataObject{

		public ToolBehaviourObject(){}
		public ToolBehaviourObject( ToolBehaviourObject _object ) : base( _object ){ Copy( _object ); }
		public ToolBehaviourObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }

		public void Copy( ToolBehaviourObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

		}



		public void Start()
		{
			Sound.Play();
		}

		/// <summary>
		/// Stops this tool instance.
		/// </summary>
		public void Stop()
		{
			Sound.Stop();
		}
	}
}

