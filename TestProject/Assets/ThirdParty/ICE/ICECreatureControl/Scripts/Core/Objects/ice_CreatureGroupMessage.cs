// ##############################################################################
//
// ice_CreatureInventory.cs
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

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class BroadcastMessageDataObject : ICEDataObject
	{
		public BroadcastMessageDataObject(){}
		public BroadcastMessageDataObject( BroadcastMessageDataObject _object ) : base( _object ){ Copy( _object ); }
		public BroadcastMessageDataObject( BroadcastMessageType _type, GameObject _target, string _command ){
			Type = _type;
			TargetGameObject = _target;
			Command = _command;
		}


		public BroadcastMessageType Type;
		[XmlIgnore]
		public GameObject TargetGameObject;
		public string BahaviourKey;
		public string Command;

		public void Copy( BroadcastMessageDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Type = _object.Type;
			Command = _object.Command;
			TargetGameObject = _object.TargetGameObject;
			BahaviourKey = _object.BahaviourKey;
		}

	}

	[System.Serializable]
	public class BroadcastMessageObject : BroadcastMessageDataObject
	{
	}

	[System.Serializable]
	public class MessageDataObject : ICEOwnerObject
	{
		public MessageDataObject(){}
		public MessageDataObject( MessageDataObject _object ) : base( _object ){ Copy( _object ); }
		public MessageDataObject( ICEWorldBehaviour _component ) : base( _component ){}

		protected ReferenceGroupObject m_ReferenceGroup = null;
		public ReferenceGroupObject ReferenceGroup{
			get{ return m_ReferenceGroup; }
		}

		public void SetReferenceGroup( ReferenceGroupObject _group ){
			m_ReferenceGroup = _group;
		}

		public void Copy( MessageDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}
	}

	[System.Serializable]
	public class MessageObject : MessageDataObject
	{

		public void SendGroupMessage( BroadcastMessageDataObject _msg )
		{
			if( _msg == null || Owner == null )
				return;

			PrintDebugLog( this, "send from ID : " + Owner.GetInstanceID() + " - " + _msg.Type.ToString() );

			if( ReferenceGroup != null )
				ReferenceGroup.Message( Owner, _msg );
		}

		public void SendRegisterMessage( BroadcastMessageDataObject _msg )
		{
			if( _msg == null )
				return;

			//CreatureRegister.SendGroupMessage( ReferenceGroup, Owner, _msg );
			//Debug.Log ( "send from ID : " + Owner.GetInstanceID() + " - " + _msg.Type.ToString() );
		}

		public string LastReceivedCommand = "";

		public void ReceiveGroupMessage( ReferenceGroupObject _group, GameObject _sender, BroadcastMessageDataObject _msg )
		{
			if( _group == null || _sender == null ||_msg == null )
				return;
			
			PrintDebugLog( this, "receive from ID : " + _sender.GetInstanceID() + " - " + _msg.Type.ToString() );

			switch( _msg.Type )
			{
			case BroadcastMessageType.COMMAND:
				LastReceivedCommand = _msg.Command;
				break;
			default:
				break;
			}
		}
	}
}
