// ##############################################################################
//
// ice_CreatureTarget.cs
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
using System.Xml;
using System.Xml.Serialization;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class SelectionTimerObject : ICEDataObject
	{
		public SelectionTimerObject(){}

		public bool UseRandomSpan = false;
		public float Span = 0;
		public float SpanMin = 0;
		public float SpanMax = 0;
		public float SpanMaximum = 60;

		protected float m_StartTime = 0;
		public float StartTime{
			get{ return m_StartTime = ( m_StartTime == 0 ? Time.time : m_StartTime ); }
		}

		protected float m_Timer = 0;
		public float Timer{
			get{ return m_Timer; }
		}

		public virtual void Copy( SelectionTimerObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			m_StartTime = _object.StartTime;
			m_Timer = _object.Timer;
			UseRandomSpan = _object.UseRandomSpan;
			Span = _object.Span;
			SpanMin = _object.SpanMin;
			SpanMax = _object.SpanMax;
			SpanMaximum = _object.SpanMaximum;
		}

		public override void Reset(){

			if( UseRandomSpan && ( Span == 0 || m_Timer != 0 ) )
				Span = UnityEngine.Random.Range( SpanMin, SpanMax );
			
			m_StartTime = 0;
			m_Timer = 0;
		}

		public virtual bool Update( bool _reset )
		{
			if( ! Enabled )
				return true;

			if( UseRandomSpan && Span == 0 )
				Span = UnityEngine.Random.Range( SpanMin, SpanMax );

			if( Span == 0 )
				return true;

			if( _reset )
			{
				Reset();
				return true;
			}
			else
			{
				m_Timer = Time.time - StartTime;

				return ( m_Timer >= Span ? true : false );
			}
		}
	}

	[System.Serializable]
	public class DelayTimerObject : SelectionTimerObject{
		public DelayTimerObject(){}
	}

	[System.Serializable]
	public class BreakTimerObject : SelectionTimerObject{
		public BreakTimerObject(){}
	}

	[System.Serializable]
	public class RetainingTimeObject : SelectionTimerObject
	{
		public RetainingTimeObject(){}

		public bool Update()
		{
			if( ! Enabled || Span == 0 )
				return false;

			m_Timer = Time.time - m_StartTime;
			return( m_Timer > Span ? false : true );
		}
	}


	[System.Serializable]
	public class SelectionConditionObject : ICEObject
	{
		public SelectionConditionObject(){}
		public SelectionConditionObject( SelectionConditionObject _condition ) { Copy( _condition ); }
		public SelectionConditionObject( ConditionalOperatorType _condition_type ){
			ConditionType = _condition_type;
		}

		public SelectionConditionObject( ConditionalOperatorType _condition_type, SelectionExpressionType _expression_type ){

			ConditionType = _condition_type;
			ExpressionType = _expression_type;
		}

		public void Copy( SelectionConditionObject _condition )
		{
			if( _condition == null )
				return;

			IsValid = false;
			Enabled = _condition.Enabled;
	
			UseDynamicValue = _condition.UseDynamicValue;

			ConditionType = _condition.ConditionType;
			Operator = _condition.Operator;

 	   		ExpressionType = _condition.ExpressionType;
			m_ExpressionTypeKey = _condition.ExpressionTypeKey;
			ExpressionValue = _condition.ExpressionValue;
			m_ExpressionValueKey = _condition.ExpressionValueKey;

			FloatValue = _condition.FloatValue;
			StringValue = _condition.StringValue;
			IntegerValue = _condition.IntegerValue;
			BooleanValue = _condition.BooleanValue;

			KeyCodeValue = _condition.KeyCodeValue;
			AxisValue = _condition.AxisValue;
			ToggleValue = _condition.ToggleValue;
			ButtonValue = _condition.ButtonValue;

			PositionType = _condition.PositionType;
			PositionVector = _condition.PositionVector;

			UseUpdateLastPosition = _condition.UseUpdateLastPosition;

			ShowOwner = _condition.ShowOwner;
			ShowActiveTarget = _condition.ShowActiveTarget;
			ShowLastTarget = _condition.ShowLastTarget;
			ShowTarget = _condition.ShowTarget;
			ShowEnvironment = _condition.ShowEnvironment;
			ShowSystem = _condition.ShowSystem;
			ShowAll = _condition.ShowAll;
			PositionVector = _condition.PositionVector;
		}

		public bool Enabled = true;

		public SelectionStatus Status = SelectionStatus.UNCHECKED;
		public void ResetStatus()
		{
			Status = SelectionStatus.UNCHECKED;
			m_IsValid = false;
		}

		private bool m_IsValid = false;
		public bool IsValid{
			set{
				m_IsValid = value;

				if( m_IsValid )
					Status = SelectionStatus.VALID;
				else
					Status = SelectionStatus.INVALID; 
			}
			get{ return m_IsValid; }
		}

		public bool ShowOwner = false;
		public bool ShowActiveTarget = false;
		public bool ShowLastTarget = false;
		public bool ShowTarget = false;
		public bool ShowEnvironment = false;
		public bool ShowSystem = false;
		public bool ShowAll = false;

		public bool UseUpdateLastPosition = false;

		[SerializeField]
		private string m_ExpressionTypeKey = "None";
		public string ExpressionTypeKey{
			get{ return m_ExpressionTypeKey = ( m_ExpressionTypeKey == "None" ? SelectionTools.TypeToStr( m_ExpressionType ) : m_ExpressionTypeKey ); }
		}

		[SerializeField]
		private SelectionExpressionType m_ExpressionType = SelectionExpressionType.None;
		public SelectionExpressionType ExpressionType{
			set{ 
				m_ExpressionType = value; 
				m_ExpressionTypeKey = SelectionTools.TypeToStr( m_ExpressionType );
			}
			get{ 
				// just required to avoid serialization problems in cases SelectionExpressionType was updated
				if( ! Application.isPlaying )
					SelectionTools.Type( ref m_ExpressionType, m_ExpressionTypeKey ); 
				
				return m_ExpressionType; 
			}
		}

		[SerializeField]
		private string m_ExpressionValueKey = "None";
		public string ExpressionValueKey{
			get{ return m_ExpressionValueKey = ( m_ExpressionValueKey == "None" ? SelectionTools.TypeToStr( m_ExpressionValue ) : m_ExpressionValueKey ); }
		}

		[SerializeField]
		private SelectionExpressionType m_ExpressionValue = SelectionExpressionType.None;
		public SelectionExpressionType ExpressionValue{
			set{ 
				m_ExpressionValue = value;
				m_ExpressionValueKey = SelectionTools.TypeToStr( m_ExpressionValue ); 
			}
			get{ 
				// just required to avoid serialization problems in cases SelectionExpressionType was updated
				if( ! Application.isPlaying )
					SelectionTools.Type( ref m_ExpressionValue, m_ExpressionValueKey ); 
				
				return m_ExpressionValue;
			}
		}

		public List<string> GetValueSelectionExpressionsByType( SelectionExpressionType _expression )
		{
			List<string> _list = new List<string>();

			SelectionExpressionDataType _type = SelectionTools.DataType( _expression );
		
			foreach( SelectionExpressionType _list_expression in System.Enum.GetValues( typeof(SelectionExpressionType) ) )
				if( _list_expression != _expression && _type == SelectionTools.DataType( _list_expression ) )
					_list.Add( _list_expression.ToString() );

			if( _list.Count == 0 )
			{
				foreach( SelectionExpressionType _list_expression in System.Enum.GetValues( typeof(SelectionExpressionType) ) )
					_list.Add( _list_expression.ToString() );
			}

			return _list;
		}

		public List<string> GetValueSelectionExpressions( SelectionExpressionType _type )
		{
			List<string> _list = new List<string>();

			string _compare_str = _type.ToString();

			_compare_str = _compare_str.Replace( "Own", "" );
			_compare_str = _compare_str.Replace( "ActiveTarget", "" );
			_compare_str = _compare_str.Replace( "LastTarget", "" );
			_compare_str = _compare_str.Replace( "Target", "" );

			foreach( SelectionExpressionType _expression in System.Enum.GetValues( typeof(SelectionExpressionType) ) )
			{
				string _expression_str = _expression.ToString();
				if( _expression_str.Contains( _compare_str ) && _expression_str != _type.ToString() )
					_list.Add( _expression_str );
			}

			if( _list.Count == 0 )
			{
				foreach( SelectionExpressionType _expression in System.Enum.GetValues( typeof(SelectionExpressionType) ) )
					_list.Add( _expression.ToString() );
			}

			return _list;
		}



		public List<string> GetSelectionExpressions( TargetObject _target, bool _show_owner, bool _show_target, bool _show_active, bool _show_last, bool _show_evnironment, bool _show_system )
		{
			if( _target == null )
				return new List<string>();
			
			List<string> _list = new List<string>();

			if( ! Application.isPlaying )
			{
				if( _show_owner )
				{
					_list.Add( SelectionExpressionType.OwnAge.ToString() );
					_list.Add( SelectionExpressionType.OwnGameObject.ToString() );
					_list.Add( SelectionExpressionType.OwnBehaviour.ToString() );
					_list.Add( SelectionExpressionType.OwnReceivedCommand.ToString() );
					_list.Add( SelectionExpressionType.OwnAge.ToString() );
					_list.Add( SelectionExpressionType.OwnGenderType.ToString() );
					_list.Add( SelectionExpressionType.OwnTrophicLevel.ToString() );
					_list.Add( SelectionExpressionType.OwnOdour.ToString() );
					_list.Add( SelectionExpressionType.OwnOdourIntensity.ToString() );
					_list.Add( SelectionExpressionType.OwnOdourRange.ToString() );
					_list.Add( SelectionExpressionType.OwnEnvTemperatureDeviation.ToString() );
					_list.Add( SelectionExpressionType.OwnZoneName.ToString() );
					_list.Add( SelectionExpressionType.OwnHomeDistance.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnFitness.ToString() );
					_list.Add( SelectionExpressionType.OwnHealth.ToString() );
					_list.Add( SelectionExpressionType.OwnStamina.ToString() );
					_list.Add( SelectionExpressionType.OwnPower.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnDamage.ToString() );
					_list.Add( SelectionExpressionType.OwnStress.ToString() );
					_list.Add( SelectionExpressionType.OwnDebility.ToString() );
					_list.Add( SelectionExpressionType.OwnHunger.ToString() );
					_list.Add( SelectionExpressionType.OwnThirst.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnAggressivity.ToString() );
					_list.Add( SelectionExpressionType.OwnExperience.ToString() );
					_list.Add( SelectionExpressionType.OwnAnxiety.ToString() );
					_list.Add( SelectionExpressionType.OwnNosiness.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnVisualSense.ToString() );
					_list.Add( SelectionExpressionType.OwnAuditorySense.ToString() );
					_list.Add( SelectionExpressionType.OwnOlfactorySense.ToString() );
					_list.Add( SelectionExpressionType.OwnGustatorySense.ToString() );
					_list.Add( SelectionExpressionType.OwnTactileSense.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnSlot0Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot1Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot2Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot3Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot4Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot5Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot6Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot7Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot8Amount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot9Amount.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnSlot0MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot1MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot2MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot3MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot4MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot5MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot6MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot7MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot8MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.OwnSlot9MaxAmount.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.OwnerIsWithinHomeArea.ToString() );
					_list.Add( SelectionExpressionType.OwnerPosition.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsDead.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsInjured.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsSheltered.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsIndoor.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsGrounded.ToString() );
					_list.Add( SelectionExpressionType.OwnerAltitude.ToString() );
					_list.Add( SelectionExpressionType.OwnerIsSelectedByTarget.ToString() );
					_list.Add( " " );


				}

				if( _show_active )
				{
					_list.Add( SelectionExpressionType.ActiveTargetName.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetEntityType.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetGameObject.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetTime.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetTimeTotal.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetAge.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetDurability.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetDurabilityInPercent.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetActiveCounterpartsLimit.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetHasOwnerActiveSelected.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetIsDestroyed.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetIsInFieldOfView.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetIsVisible.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetIsAudible.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetIsSmellable.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetVisibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetAudibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSmellabilityByDistance.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetHasParent.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetParentName.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetOdour.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetOdourIntensity.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetOdourIntensityNet.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetOdourIntensityByDistance.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetOdourRange.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetSlot0Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot1Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot2Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot3Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot4Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot5Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot6Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot7Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot8Amount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot9Amount.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.ActiveTargetSlot0MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot1MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot2MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot3MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot4MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot5MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot6MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot7MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot8MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.ActiveTargetSlot9MaxAmount.ToString() );
					_list.Add( " " );
				}

				if( _show_last )
				{
					_list.Add( SelectionExpressionType.LastTargetName.ToString() );
					_list.Add( SelectionExpressionType.LastTargetEntityType.ToString() );
					_list.Add( SelectionExpressionType.LastTargetGameObject.ToString() );
					_list.Add( SelectionExpressionType.LastTargetTime.ToString() );
					_list.Add( SelectionExpressionType.LastTargetTimeTotal.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetAge.ToString() );
					_list.Add( SelectionExpressionType.LastTargetDurability.ToString() );
					_list.Add( SelectionExpressionType.LastTargetDurabilityInPercent.ToString() );
					_list.Add( SelectionExpressionType.LastTargetActiveCounterpartsLimit.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetHasOwnerActiveSelected.ToString() );
					_list.Add( SelectionExpressionType.LastTargetIsDestroyed.ToString() );
					_list.Add( SelectionExpressionType.LastTargetIsInFieldOfView.ToString() );
					_list.Add( SelectionExpressionType.LastTargetIsVisible.ToString() );
					_list.Add( SelectionExpressionType.LastTargetIsAudible.ToString() );
					_list.Add( SelectionExpressionType.LastTargetIsSmellable.ToString() );
					_list.Add( SelectionExpressionType.LastTargetVisibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.LastTargetAudibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSmellabilityByDistance.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetHasParent.ToString() );
					_list.Add( SelectionExpressionType.LastTargetParentName.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetOdour.ToString() );
					_list.Add( SelectionExpressionType.LastTargetOdourIntensity.ToString() );
					_list.Add( SelectionExpressionType.LastTargetOdourIntensityNet.ToString() );
					_list.Add( SelectionExpressionType.LastTargetOdourIntensityByDistance.ToString() );
					_list.Add( SelectionExpressionType.LastTargetOdourRange.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetSlot0Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot1Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot2Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot3Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot4Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot5Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot6Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot7Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot8Amount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot9Amount.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.LastTargetSlot0MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot1MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot2MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot3MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot4MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot5MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot6MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot7MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot8MaxAmount.ToString() );
					_list.Add( SelectionExpressionType.LastTargetSlot9MaxAmount.ToString() );
					_list.Add( " " );
				}

				if( _show_target )
				{
					_list.Add( SelectionExpressionType.TargetIsActive.ToString() );
					_list.Add( SelectionExpressionType.TargetIsLastTarget.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.TargetName.ToString() );
					_list.Add( SelectionExpressionType.TargetEntityType.ToString() );
					_list.Add( SelectionExpressionType.TargetGameObject.ToString() );
					_list.Add( SelectionExpressionType.TargetTime.ToString() );
					_list.Add( SelectionExpressionType.TargetTimeTotal.ToString() );
					_list.Add( " " );

					if( _target.EntityComponent  != null )
					{
						_list.Add( SelectionExpressionType.TargetAge.ToString() );
						_list.Add( SelectionExpressionType.TargetDurability.ToString() );
						_list.Add( SelectionExpressionType.TargetDurabilityInPercent.ToString() );
						_list.Add( SelectionExpressionType.TargetActiveCounterpartsLimit.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.TargetHasOwnerActiveSelected.ToString() );
						_list.Add( SelectionExpressionType.TargetIsDestroyed.ToString() );
						_list.Add( " " );
					}
						
					_list.Add( SelectionExpressionType.TargetHasParent.ToString() );
					_list.Add( SelectionExpressionType.TargetParentName.ToString() );
					_list.Add( SelectionExpressionType.TargetZoneName.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.TargetDistance.ToString() );
					_list.Add( SelectionExpressionType.TargetOffsetPositionDistance.ToString() );
					_list.Add( SelectionExpressionType.TargetMovePositionDistance.ToString() );
					_list.Add( SelectionExpressionType.TargetLastKnownPositionDistance.ToString() );
					_list.Add( " " );

					_list.Add( SelectionExpressionType.TargetIsInFieldOfView.ToString() );
					_list.Add( SelectionExpressionType.TargetIsVisible.ToString() );
					_list.Add( SelectionExpressionType.TargetIsAudible.ToString() );
					_list.Add( SelectionExpressionType.TargetIsSmellable.ToString() );
					_list.Add( SelectionExpressionType.TargetVisibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.TargetAudibilityByDistance.ToString() );
					_list.Add( SelectionExpressionType.TargetSmellabilityByDistance.ToString() );
					_list.Add( " " );



					if( _target.EntityMarker  != null ||
						_target.EntityCreature != null )
					{
						_list.Add( SelectionExpressionType.TargetOdour.ToString() );
						_list.Add( SelectionExpressionType.TargetOdourIntensity.ToString() );
						_list.Add( SelectionExpressionType.TargetOdourIntensityNet.ToString() );
						_list.Add( SelectionExpressionType.TargetOdourIntensityByDistance.ToString() );
						_list.Add( SelectionExpressionType.TargetOdourRange.ToString() );
						_list.Add( " " );
					}

					if( _target.EntityPlant != null || 
						_target.EntityPlayer != null || 
						_target.EntityItem != null ||
						_target.EntityCreature != null )
					{
						_list.Add( SelectionExpressionType.TargetSlot0Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot1Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot2Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot3Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot4Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot5Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot6Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot7Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot8Amount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot9Amount.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.TargetSlot0MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot1MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot2MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot3MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot4MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot5MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot6MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot7MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot8MaxAmount.ToString() );
						_list.Add( SelectionExpressionType.TargetSlot9MaxAmount.ToString() );
						_list.Add( " " );
					}

					if( _target.EntityCreature != null )
					{
						_list.Add( SelectionExpressionType.CreatureActiveTargetGameObject.ToString() );
						_list.Add( SelectionExpressionType.CreatureActiveTargetTime.ToString() );
						_list.Add( SelectionExpressionType.CreatureActiveTargetTimeTotal.ToString() );
						_list.Add( SelectionExpressionType.CreatureBehaviour.ToString() );
						_list.Add( SelectionExpressionType.CreatureCommand.ToString() );
						_list.Add( SelectionExpressionType.CreatureAltitude.ToString() );

						_list.Add( SelectionExpressionType.CreatureEnvTemperatureDeviation.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.CreatureFitness.ToString() );
						_list.Add( SelectionExpressionType.CreatureHealth.ToString() );
						_list.Add( SelectionExpressionType.CreatureStamina.ToString() );
						_list.Add( SelectionExpressionType.CreaturePower.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.CreatureDamage.ToString() );
						_list.Add( SelectionExpressionType.CreatureStress.ToString() );
						_list.Add( SelectionExpressionType.CreatureDebility.ToString() );
						_list.Add( SelectionExpressionType.CreatureHunger.ToString() );
						_list.Add( SelectionExpressionType.CreatureThirst.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.CreatureAggressivity.ToString() );
						_list.Add( SelectionExpressionType.CreatureExperience.ToString() );
						_list.Add( SelectionExpressionType.CreatureAnxiety.ToString() );
						_list.Add( SelectionExpressionType.CreatureNosiness.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.CreatureVisualSense.ToString() );
						_list.Add( SelectionExpressionType.CreatureAuditorySense.ToString() );
						_list.Add( SelectionExpressionType.CreatureOlfactorySense.ToString() );
						_list.Add( SelectionExpressionType.CreatureGustatorySense.ToString() );
						_list.Add( SelectionExpressionType.CreatureTactileSense.ToString() );
						_list.Add( " " );

						_list.Add( SelectionExpressionType.CreaturePosition.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsDead.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsInjured.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsInjuredOrDead.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsSheltered.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsIndoor.ToString() );
						_list.Add( SelectionExpressionType.CreatureIsGrounded.ToString() );

						_list.Add( SelectionExpressionType.CreatureGenderType.ToString() );
						_list.Add( SelectionExpressionType.CreatureTrophicLevel.ToString() );
					}
				}

				if( _show_evnironment )
				{
					_list.Add( SelectionExpressionType.EnvironmentTimeHour.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentTimeMinute.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentTimeSecond.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentDateYear.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentDateMonth.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentDateDay.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentTemperature.ToString() );
					_list.Add( SelectionExpressionType.EnvironmentWeather.ToString() );
					_list.Add( " " );
				}

				if( _show_system )
				{
					_list.Add( SelectionExpressionType.SystemInputKey.ToString() );
					_list.Add( SelectionExpressionType.SystemInputAxis.ToString() );
					_list.Add( SelectionExpressionType.SystemUIToggle.ToString() );
					_list.Add( SelectionExpressionType.SystemUIButton.ToString() );
					_list.Add( " " );
				}
				/*
				string[] _array = _list.ToArray();

				for( int i = 0 ; i < _array.Length ; i++ )
				{
					int _out;
					if( int.TryParse( _array[i], out _out ) )
						_array[i] = " ";
				}*/
			}

			if( _list.Count == 0 )
			{
				_list = SystemTools.EnumToList<SelectionExpressionType>();
				//foreach( SelectionExpressionType _expression in System.Enum.GetValues( typeof(SelectionExpressionType) ) )
				//	_list.Add( _expression.ToString() );
			}

			return _list;
		}



		public ConditionalOperatorType ConditionType = ConditionalOperatorType.AND;	
		public LogicalOperatorType Operator = LogicalOperatorType.EQUAL;

		public float FloatValue = 0;
		public int IntegerValue = 0;
		public string StringValue = "";
		public KeyCode KeyCodeValue = KeyCode.Escape;
		public AxisInputData AxisValue;
		public string ToggleValue = "";
		public string ButtonValue = "";


		public bool BooleanValue = false;
		public TargetSelectorPositionType PositionType = TargetSelectorPositionType.TargetMovePosition;
		public Vector3 PositionVector = Vector3.zero;

		public bool UseDynamicValue = false;

		[System.NonSerialized]
		private UnityEngine.UI.Toggle _toggle_buffer = null;
		public UnityEngine.UI.Toggle GetUIToggle()
		{
			if( string.IsNullOrEmpty( ToggleValue ) )
				return null;
			
			if( _toggle_buffer == null || _toggle_buffer.name != ToggleValue )
			{
				UnityEngine.UI.Toggle[] _toggles = GameObject.FindObjectsOfType<UnityEngine.UI.Toggle>();

				for( int _i = 0 ; _i < _toggles.Length ; _i++ )
				{
					if( _toggles[_i] != null && _toggles[_i].name == ToggleValue )
					{
						_toggle_buffer = _toggles[_i];
						return _toggle_buffer;
					}				
				}
			}

			return _toggle_buffer;
		}
			
		private bool m_ButtonIsPressed = false;
		public bool ButtonIsPressed{
			get{ 
				bool _pressed = m_ButtonIsPressed;
				m_ButtonIsPressed = false;
				return _pressed; 
			
			}
		}
			
		private void UIButtonClick(){
			m_ButtonIsPressed = true;
		}


		[System.NonSerialized]
		private UnityEngine.UI.Button _button_buffer = null;
		public UnityEngine.UI.Button GetUIButton()
		{
			if( string.IsNullOrEmpty( ButtonValue ) )
				return null;
			
			if( _button_buffer == null || _button_buffer.name != ButtonValue )
			{
				UnityEngine.UI.Button[] _buttons = GameObject.FindObjectsOfType<UnityEngine.UI.Button>();

				for( int _i = 0 ; _i < _buttons.Length ; _i++ )
				{
					if( _buttons[_i] != null && _buttons[_i].name == ButtonValue )
					{
						_button_buffer = _buttons[_i];
						_button_buffer.onClick.AddListener(UIButtonClick);
						return _button_buffer;
					}				
				}
			}

			return _button_buffer;
		}

		public string ConditionToString()
		{
			string _condition = "";
			switch( ConditionType )
			{
			case ConditionalOperatorType.OR:
				_condition = "OR";
				break;
			default:
				_condition = "AND";
				break;
			}

			return _condition;
		}

		public string OperatorToString()
		{
			string _operator = "";

			if( SelectionTools.NeedLogicalOperator( ExpressionType ) )
			{
				switch( Operator )
				{
				case LogicalOperatorType.EQUAL:
					_operator = "==";
					break;
				case LogicalOperatorType.NOT:
					_operator = "!=";
					break;
				case LogicalOperatorType.LESS:
					_operator = "<";
					break;
				case LogicalOperatorType.LESS_OR_EQUAL:
					_operator = "<=";
					break;				
				case LogicalOperatorType.GREATER:
					_operator = ">";
					break;
				case LogicalOperatorType.GREATER_OR_EQUAL:
					_operator = ">=";
					break;
				}
			}
			else
			{
				switch( Operator )
				{
				case LogicalOperatorType.NOT:
					_operator = "IS NOT";
					break;
				default:
					_operator = "IS";
					break;
				}
			}

			return _operator;
		}
	}

	[System.Serializable]
	public class SelectionConditionGroupObject : ICEDataObject
	{
		public void ResetStatus()
		{
			Status = SelectionStatus.UNCHECKED;
			m_IsValid = false;
		}

		public void ResetFullStatus()
		{
			Status = SelectionStatus.UNCHECKED;
			m_IsValid = false;

			for( int _i = 0 ; _i < Conditions.Count ; _i++ )
			{
				if( Conditions[_i] != null )
					Conditions[_i].ResetStatus();
			}
		}

		public SelectionStatus Status = SelectionStatus.UNCHECKED;
		private bool m_IsValid = false;
		public bool IsValid{
			set{
				m_IsValid = value;

				if( m_IsValid )
					Status = SelectionStatus.VALID;
				else
					Status = SelectionStatus.INVALID; 
			}
			get{ return m_IsValid; }
		}
		public SelectionConditionGroupObject(){}
		public SelectionConditionGroupObject( SelectionConditionGroupObject _group ) { Copy( _group ); }
		public SelectionConditionGroupObject( ConditionalOperatorType _condition_type ){
			Enabled = true;
			Foldout = true;
			Conditions.Add( new SelectionConditionObject( _condition_type, SelectionExpressionType.OwnFitness ) );
		}
			
		public void Copy( SelectionConditionGroupObject _group )
		{
			if( _group == null )
				return;

			Enabled = _group.Enabled;
			UseUpdateLastPosition = _group.UseUpdateLastPosition;

			Conditions.Clear();
			foreach( SelectionConditionObject _condition in _group.Conditions )
				Conditions.Add( new SelectionConditionObject( _condition ) );
		}

		public ConditionalOperatorType InitialOperatorType{
			get{ return ( Conditions.Count > 0 ? Conditions[0].ConditionType : ConditionalOperatorType.AND ); }
			set{ if( Conditions.Count > 0 ) Conditions[0].ConditionType = value; }
		}

		[SerializeField]
		private List<SelectionConditionObject> m_Conditions = null;
		public List<SelectionConditionObject> Conditions{
			get{ return m_Conditions = ( m_Conditions == null ? new List<SelectionConditionObject>() : m_Conditions ); }
			set{ 
				Conditions.Clear();
				if( value == null ) return;	
				foreach( SelectionConditionObject _condition in value )
					Conditions.Add( new SelectionConditionObject( _condition ) ); 			
			}
		}

		public bool UseUpdateLastPosition = false;
	}

	[System.Serializable]
	public class PreselectionCriteriaObject : ICEDataObject
	{
		public PreselectionCriteriaObject(){}
		public PreselectionCriteriaObject( PreselectionCriteriaObject _criteria ) : base( _criteria ) {
			Copy( _criteria );
		}

		public void Copy( PreselectionCriteriaObject _criteria )
		{
			if( _criteria == null )
				return;

			base.Copy( _criteria );

			UseTargetRefreshInterval = _criteria.UseTargetRefreshInterval;
			UseTargetRecheck = _criteria.UseTargetRecheck;
			RefreshInvalidTargetsOnly = _criteria.RefreshInvalidTargetsOnly;

			TargetUpdateTimeMin = _criteria.TargetUpdateTimeMin;
			TargetUpdateTimeMax = _criteria.TargetUpdateTimeMax;
			TargetUpdateTimeMaximum = _criteria.TargetUpdateTimeMaximum;

			UseActiveCounterpartsLimit = _criteria.UseActiveCounterpartsLimit;
			ActiveCounterpartsLimit = _criteria.ActiveCounterpartsLimit;

			PreferActiveCounterparts = _criteria.PreferActiveCounterparts;

			UseChildObjects = _criteria.UseChildObjects;
			UseAllAvailableObjects = _criteria.UseAllAvailableObjects;



		}

		[SerializeField]
		private bool m_UseTargetRefreshInterval = false;
		public bool UseTargetRefreshInterval{
			get{ return ( Enabled ? m_UseTargetRefreshInterval : false ); }
			set{ m_UseTargetRefreshInterval = ( Enabled ? value : m_UseTargetRefreshInterval ); }
		}

		[SerializeField]
		private bool m_RefreshInvalidTargetsOnly = false;
		public bool RefreshInvalidTargetsOnly{
			get{ return ( UseTargetRefreshInterval ? m_RefreshInvalidTargetsOnly : false ); }
			set{ m_RefreshInvalidTargetsOnly = ( UseTargetRefreshInterval ? value : m_RefreshInvalidTargetsOnly ); }
		}

		[SerializeField]
		private bool m_UseTargetRecheck = false;
		public bool UseTargetRecheck{
			get{ return ( UseTargetRefreshInterval ? m_UseTargetRecheck : false ); }
			set{ m_UseTargetRecheck = ( Enabled ? value : m_UseTargetRecheck ); }
		}

		public float TargetUpdateTimeMin = 1;
		public float TargetUpdateTimeMax = 3;
		public float TargetUpdateTimeMaximum = 30;

		private float m_TargetUpdateTimer = 0;
		public float TargetUpdateTimer{
			get{ return m_TargetUpdateTimer; }
		}

		private float m_TargetUpdateTime = 0;
		public float TargetUpdateTime{
			get{ return m_TargetUpdateTime; }
		}

		[SerializeField]
		private bool m_UseActiveCounterpartsLimit = false;
		public bool UseActiveCounterpartsLimit{
			get{ return ( Enabled ? m_UseActiveCounterpartsLimit : false ); }
			set{ m_UseActiveCounterpartsLimit = ( Enabled ? value : m_UseActiveCounterpartsLimit ); }
		}

		[SerializeField]
		private int m_ActiveCounterpartsLimit = 0;
		public int ActiveCounterpartsLimit{
			get{ return ( UseActiveCounterpartsLimit ? m_ActiveCounterpartsLimit : -1 ); }
			set{ m_ActiveCounterpartsLimit = ( UseActiveCounterpartsLimit ? value : m_ActiveCounterpartsLimit ); }
		}

		[SerializeField]
		private bool m_PreferActiveCounterparts = false;
		public bool PreferActiveCounterparts{
			get{ return ( Enabled ? m_PreferActiveCounterparts : false ); }
			set{ m_PreferActiveCounterparts = ( Enabled ? value : m_PreferActiveCounterparts ); }
		}

		[SerializeField]
		private bool m_UseChildObjects = false;
		public bool UseChildObjects{
			get{ return ( Enabled ? m_UseChildObjects : false ); }
			set{ m_UseChildObjects = ( Enabled ? value : m_UseChildObjects ); }
		}

		[SerializeField]
		private bool m_UseAllAvailableObjects = false;
		public bool UseAllAvailableObjects{
			get{ return ( Enabled ? m_UseAllAvailableObjects : false ); }
			set{ m_UseAllAvailableObjects = ( Enabled ? value : m_UseAllAvailableObjects ); }
		}
			
		public bool RefreshRequired()
		{
			if( ! Enabled || ! UseTargetRefreshInterval )
				return true;

			if( RefreshInvalidTargetsOnly )
				return false;

			if(  m_TargetUpdateTime == 0 || m_TargetUpdateTimer >= m_TargetUpdateTime )
			{
				m_TargetUpdateTime = UnityEngine.Random.Range( TargetUpdateTimeMin, TargetUpdateTimeMax );
				m_TargetUpdateTimer = 0;

				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Late update.
		/// </summary>
		public void LateUpdate()
		{
			if( RefreshInvalidTargetsOnly || ! m_UseTargetRefreshInterval )
				m_TargetUpdateTimer = 0;
			else
				m_TargetUpdateTimer += Time.deltaTime;
		}
	}

	[System.Serializable]
	public class SelectionCriteriaObject : ICEDataObject
	{
		public SelectionCriteriaObject(){}
		public SelectionCriteriaObject( SelectionCriteriaObject _selectors ) : base( _selectors ) {
			Copy( _selectors );
		}
			
		public bool UsePreselection{
			get{ return Preselection.Enabled; }
			set{ Preselection.Enabled = value; }
		}
			
		public bool UseSelectionRange = false;
		public bool UseSelectionAngle = false;
		public bool UseVisibilityCheck = false;
		public bool UseAudibleCheck = false;
		public bool UseOdourCheck = false;
		public bool UseTactileCheck = false;
		public bool UseFlavourCheck = false;
		public float VisibilityCheckVerticalOffset = 0;

		public int Priority = 0;
		public int DefaultPriority = 0;

		public float SelectionRange = 0;
		public float SelectionRangeMax = 0;

		public bool UseFieldOfView = false;
		public float SelectionAngle = 0;
		public bool CanUseDefaultPriority = false;
		public bool UseDefaultPriority = false;
		public bool UseAdvanced = false;

		public bool TotalCheckIsValid = false;
		public bool AdvancedCheckIsValid = false;
		public ConditionalOperatorType AdvancedOperatorType = ConditionalOperatorType.AND;

		private int m_AlternateObjectsCount = 0;
		public int AlternateObjectsCount{
			get{ return m_AlternateObjectsCount; }
		}
		public void SetAlternateObjectsCount( int _count ){
			m_AlternateObjectsCount = _count;
		}

		public float FixedSelectionAngle{
			get{
				if( ! UseSelectionAngle || SelectionAngle <= 0 || SelectionAngle >= 180 )
					return 0;
				else
					return SelectionAngle;
			}
		}

		public float FixedSelectionRange{
			get{
				if( ! UseSelectionRange || SelectionRange == 0 )
					return Mathf.Infinity;
				else
					return SelectionRange;
			}
		}
			
		public void ResetStatus()
		{
			m_Status = SelectionStatus.UNCHECKED;
			m_IsValid = false;

			for( int _i = 0 ; _i < ConditionGroups.Count ; _i++ )
			{
				SelectionConditionGroupObject _group = ConditionGroups[_i];
				if( _group != null )
					_group.ResetFullStatus();
			}
		}

		public void TryResetStatus()
		{
			if( m_Status == SelectionStatus.UNCHECKED )
				return;
			
			m_Status = SelectionStatus.UNCHECKED;
			m_IsValid = false;

			for( int _i = 0 ; _i < ConditionGroups.Count ; _i++ )
			{
				SelectionConditionGroupObject _group = ConditionGroups[_i];
				if( _group != null )
					_group.ResetFullStatus();
			}
		}

		private SelectionStatus m_Status = SelectionStatus.UNCHECKED;
		public SelectionStatus Status{
			get{ return m_Status; }
		}

		private bool m_IsValid = false;
		public bool IsValid{
			set{
				m_IsValid = value;

				m_Status = ( m_IsValid ? SelectionStatus.VALID : SelectionStatus.INVALID ); 
			}
			get{ return m_IsValid; }
		}

		private float m_DynamicPriority = 0;
		public float DynamicPriority{
			get{ return UpdateRelevance( 0 ); }
		}

		private float m_RelevanceMultiplier = 0;
		public float RelevanceMultiplier{
			get{ 
				if( m_RelevanceMultiplier > 1 )
					m_RelevanceMultiplier = 1;				
				else if( m_RelevanceMultiplier < -1 )
					m_RelevanceMultiplier = -1;

				return m_RelevanceMultiplier; 			
			}
		}

		public void ResetRelevanceMultiplier(){
			m_RelevanceMultiplier = 0;
		}

		public ConditionalOperatorType InitialOperatorType{
			get{
				if( ConditionGroups.Count > 0 )
					return ConditionGroups[0].InitialOperatorType;
				else
					return ConditionalOperatorType.AND;
			}
		}

		public void SetRelevanceMultiplier( float _relevance_multiplier ){
			m_RelevanceMultiplier = _relevance_multiplier;
		}

		[SerializeField]
		private RetainingTimeObject m_RetainingTimer = null;
		public RetainingTimeObject RetainingTimer{
			get{ return m_RetainingTimer = ( m_RetainingTimer == null ? new RetainingTimeObject() : m_RetainingTimer ); }
			set{ RetainingTimer.Copy( value ); }
		}

		[SerializeField]
		private DelayTimerObject m_DelayTimer = null;
		public DelayTimerObject DelayTimer{
			get{ return m_DelayTimer = ( m_DelayTimer == null ? new DelayTimerObject() : m_DelayTimer ); }
			set{ DelayTimer.Copy( value ); }
		}

		[SerializeField]
		private List<SelectionConditionGroupObject> m_SelectorGroups = null; // TODO: obsolete m_SelectorGroups

		[SerializeField]
		private PreselectionCriteriaObject m_Preselection = null;
		public PreselectionCriteriaObject Preselection{
			get{ return m_Preselection = ( m_Preselection == null ? new PreselectionCriteriaObject() : m_Preselection ); }
			set{ Preselection.Copy( value ); }
		}


		[SerializeField]
		private List<SelectionConditionGroupObject> m_ConditionGroups = null;
		public List<SelectionConditionGroupObject> ConditionGroups{
			get{

				m_ConditionGroups = ( m_ConditionGroups == null ? new List<SelectionConditionGroupObject>() : m_ConditionGroups ); 

				// TODO: handles obsolete m_SelectorGroups
				if( m_SelectorGroups != null && m_SelectorGroups.Count > 0 )
				{
					m_ConditionGroups.Clear();
					foreach( SelectionConditionGroupObject _group in m_SelectorGroups )
						m_ConditionGroups.Add( new SelectionConditionGroupObject( _group ) ); 

					m_SelectorGroups.Clear();
					m_SelectorGroups = null;

					ICEDebug.LogInfo( "Copy obsolete SelectorGroups to ConditionGroups" );
				}
					
				return m_ConditionGroups;
			}
			set{ 
				ConditionGroups.Clear();
				if( value == null ) return;	
				foreach( SelectionConditionGroupObject _group in value )
					ConditionGroups.Add( new SelectionConditionGroupObject( _group ) ); 
			}
		}

		public float UpdateRelevance( float _relevance_multiplier )
		{
			m_RelevanceMultiplier += _relevance_multiplier;

			m_DynamicPriority = Priority + ( Priority * RelevanceMultiplier );

			if( m_DynamicPriority > 100 )
				m_DynamicPriority = 100;

			if( m_DynamicPriority < 0 )
				m_DynamicPriority = 0;

			return m_DynamicPriority;
		}
			
		public int GetPriority( TargetType _type )
		{
			if( _type == TargetType.HOME && Enabled == false )
				return GetDefaultPriorityByType( _type );
			else if( UseAdvanced )
				return (int)DynamicPriority;
			else
				return Priority;
		}

		public int GetDefaultPriorityByType( TargetType _type )
		{
			int _priority = 0;
			if( _type == TargetType.HOME )
				_priority = 0;
			else if( _type == TargetType.INTERACTOR )
				_priority = 60;
			else if( _type == TargetType.PATROL )
				_priority = 50;
			else if( _type == TargetType.WAYPOINT )
				_priority = 50;
			else if( _type == TargetType.OUTPOST )
				_priority = 50;
			else if( _type == TargetType.ESCORT )
				_priority = 55;

			return _priority;
		}

		public float GetDefaultRangeByType( TargetType _type )
		{
			float _range = 0;
			if( _type == TargetType.HOME )
				_range = 0;	
			else if( _type == TargetType.INTERACTOR )
				_range = 20;
			else if( _type == TargetType.PATROL )
				_range = 0;
			else if( _type == TargetType.WAYPOINT )
				_range = 0;
			else if( _type == TargetType.OUTPOST )
				_range = 0;
			else if( _type == TargetType.ESCORT )
				_range = 0;

			return _range;
		}

		public float GetDefaultAngleByType( TargetType _type )
		{
			float _angle = 0;
			if( _type == TargetType.HOME )
				_angle = 0;	
			else if( _type == TargetType.INTERACTOR )
				_angle = 50;
			else if( _type == TargetType.PATROL )
				_angle = 0;
			else if( _type == TargetType.WAYPOINT )
				_angle = 0;
			else if( _type == TargetType.OUTPOST )
				_angle = 0;
			else if( _type == TargetType.ESCORT )
				_angle = 0;

			return _angle;
		}

		public void Copy( SelectionCriteriaObject _criteria )
		{
			if( _criteria == null )
				return;

			base.Copy( _criteria );

			Preselection.Copy( _criteria.Preselection );

			Priority = _criteria.Priority;

			UseFieldOfView = _criteria.UseFieldOfView;
			UseSelectionRange = _criteria.UseSelectionRange;
			UseSelectionAngle = _criteria.UseSelectionAngle;
			UseVisibilityCheck = _criteria.UseVisibilityCheck;
			UseAudibleCheck = _criteria.UseAudibleCheck;
			UseOdourCheck = _criteria.UseOdourCheck;
			UseTactileCheck = _criteria.UseTactileCheck;
			UseFlavourCheck = _criteria.UseFlavourCheck;

			DelayTimer = _criteria.DelayTimer;
			RetainingTimer = _criteria.RetainingTimer;

			SelectionRange = _criteria.SelectionRange;
			SelectionAngle = _criteria.SelectionAngle;
			DefaultPriority = _criteria.DefaultPriority;
			UseDefaultPriority = _criteria.UseDefaultPriority;
			UseAdvanced = _criteria.UseAdvanced;

			SetRelevanceMultiplier( _criteria.RelevanceMultiplier );

			ConditionGroups.Clear();
			foreach( SelectionConditionGroupObject _group in _criteria.ConditionGroups )
				ConditionGroups.Add( new SelectionConditionGroupObject( _group ) );
		}


		public void LateUpdate()
		{
			Preselection.LateUpdate();
		}

	}

}