// ##############################################################################
//
// ice_editor_world_objects.cs | WorldObjectEditor
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
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public class WorldObjectEditor : ObjectEditor
	{	

		/// <summary>
		/// Draws the audio object.
		/// </summary>
		/// <param name="_audio">Audio.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEntityStatusDisplayObject( ICEWorldBehaviour _control, EntityStatusDisplayObject _display, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _display == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Display";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			DrawObjectHeader( _display, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _display ) )
				return;
		
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _display.ShowName == false );
					_display.Name = ICEEditorLayout.Text( "Display Name", "", _display.Name );
					if( ICEEditorLayout.ResetButtonSmall() )
						_display.Name = _control.name;
					EditorGUI.EndDisabledGroup();
					_display.ShowName = ICEEditorLayout.CheckButton( "SHOW", "", _display.ShowName, ICEEditorStyle.ButtonMiddle );
				ICEEditorLayout.EndHorizontal();

				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _display.ShowCommand == false );
					_display.Command = ICEEditorLayout.Text( "Input Command", "", _display.Command );
					if( ICEEditorLayout.ResetButtonSmall() )
						_display.Command = "Press F to Pickup\n";
					EditorGUI.EndDisabledGroup();
					_display.ShowCommand = ICEEditorLayout.CheckButton( "SHOW", "", _display.ShowCommand, ICEEditorStyle.ButtonMiddle );
				ICEEditorLayout.EndHorizontal();
		
			ICEEditorLayout.BeginHorizontal();
				_display.Offset = ICEEditorLayout.OffsetGroup( "Offset", _display.Offset );
			ICEEditorLayout.EndHorizontal();

			ICEEditorLayout.BeginHorizontal();
			_display.Offset = ICEEditorLayout.OffsetGroup( "Offset", _display.Offset );
			ICEEditorLayout.EndHorizontal();

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawToolObject( ICEWorldBehaviour _control, ToolObject _tool, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _tool == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Tool";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.TOOL;


			DrawObjectHeader( _tool, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _tool ) )
				return;

			_tool.StandByBehaviour.Enabled = true;
			_tool.OperationBehaviour.Enabled = true;

			DrawToolBehaviourObject( _control, _tool.StandByBehaviour , EditorHeaderType.FOLDOUT, "", "StandBy" );
			DrawToolBehaviourObject( _control, _tool.OperationBehaviour , EditorHeaderType.FOLDOUT, "", "Operation" );

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawToolBehaviourObject( ICEWorldBehaviour _control, ToolBehaviourObject _behaviour, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _behaviour == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Behaviour";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.TOOL_BEHAVIOUR;


			DrawObjectHeader( _behaviour, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _behaviour ) )
				return;

			DrawDirectAudioPlayerObject( _behaviour.Sound, EditorHeaderType.FOLDOUT_ENABLED, Info.TOOL_BEHAVIOUR_AUDIO, "Sound" );
			DrawEffectObject( _control, _behaviour.Effect, EditorHeaderType.FOLDOUT_ENABLED, Info.TOOL_BEHAVIOUR_EFFECT, "Effect" );

			EndObjectContent();
			// CONTENT END
		}



		public static void DrawExplosiveObject( ICEWorldBehaviour _control, ICE.World.Objects.ExplosiveObject _explosive, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _explosive == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Explosive";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EXPLOSIVE;


			DrawObjectHeader( _explosive, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _explosive ) )
				return;

				_explosive.DetonateOnContact = ICEEditorLayout.Toggle( "Detonate On Contact", "", _explosive.DetonateOnContact, Info.EXPLOSIVE_DETONATE_ON_CONTACT );
				_explosive.DetonateOnDestroyed = ICEEditorLayout.Toggle( "Detonate On Destroyed", "", _explosive.DetonateOnDestroyed, Info.EXPLOSIVE_DETONATE_ON_DESTROYED );
				_explosive.DetonateOnCountdown = ICEEditorLayout.Toggle( "Detonate On Countdown", "", _explosive.DetonateOnCountdown, Info.EXPLOSIVE_DETONATE_ON_COUNTDOWN );

			EndObjectContent();
			// CONTENT END
		}



		public static void DrawImpactObject( ICEWorldBehaviour _control, BasicImpactObject _impact, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _impact == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Impact";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.IMPACT;


			DrawObjectHeader( _impact, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _impact ) )
				return;

			string _help_type = Info.IMPACT_TYPE;

			if( _impact.ImpactType == DamageTransferType.Direct )
				_help_type += "\n\n" + Info.IMPACT_TYPE_DIRECT;
			else if( _impact.ImpactType == DamageTransferType.Message )
				_help_type += "\n\n" + Info.IMPACT_TYPE_MESSAGE;
			else if( _impact.ImpactType == DamageTransferType.DirectOrMessage )
				_help_type += "\n\n" + Info.IMPACT_TYPE_DIRECT_OR_MESSAGE;
			else if( _impact.ImpactType == DamageTransferType.DirectAndMessage )
				_help_type += "\n\n" + Info.IMPACT_TYPE_DIRECT_AND_MESSAGE;

			_impact.ImpactType = (DamageTransferType)ICEEditorLayout.EnumPopup( "Damage Transfer", "", _impact.ImpactType, _help_type );
			EditorGUI.indentLevel++;
				if( _impact.ImpactType != DamageTransferType.Direct )
					_impact.DamageMethodName = ICEEditorLayout.Text( "Damage Method", "", _impact.DamageMethodName, Info.IMPACT_DAMAGE_METHOD );					
				_impact.DamageMethodValue = ICEEditorLayout.MaxDefaultSlider( "Damage Value", "", _impact.DamageMethodValue, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _impact.DamageMethodValueMaximum, 10, Info.IMPACT_DAMAGE_VALUE );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();

			_impact.ForceType = (DamageForceType)ICEEditorLayout.EnumPopup( "Force Type", "", _impact.ForceType, Info.IMPACT_FORCE_TYPE );
			EditorGUI.indentLevel++;
				if( _impact.ForceType != DamageForceType.None )
				{
					ICEEditorLayout.MinMaxRandomDefaultSlider( "Energy (min/max)", "", ref _impact.ForceMin, ref _impact.ForceMax, 0, ref _impact.ForceMaximum, 100, 100, Init.DECIMAL_PRECISION_VELOCITY, 40, Info.IMPACT_FORCE_ENERGY );
					if( _impact.ForceType == DamageForceType.Explosion )
						_impact.ExplosionRadius = ICEEditorLayout.MaxDefaultSlider( "Explosion Radius", "", _impact.ExplosionRadius, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _impact.ExplosionRadiusMaximum, 5,  Info.IMPACT_EXPLOSION_RADIUS ); 
				}

			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();

			DrawDirectAudioPlayerObject( _impact.ImpactSound, EditorHeaderType.FOLDOUT_ENABLED, Info.IMPACT_AUDIO, "Sound" );
			DrawDirectEffectObject( _control, _impact.ImpactEffect, EditorHeaderType.FOLDOUT_ENABLED, Info.IMPACT_EFFECT, "Effect" );

			DrawImpactBehaviourObject( _control, _impact.ImpactBehaviour, ( _impact as ImpactObject == null ), EditorHeaderType.FOLDOUT_ENABLED );


			EndObjectContent();
			// CONTENT END
		}


		/// <summary>
		/// Draws the audio object.
		/// </summary>
		/// <param name="_audio">Audio.</param>
		/// <param name="_help">Help.</param>
		public static void DrawDirectAudioPlayerObject( DirectAudioPlayerObject _audio, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _audio == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Audio";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			DrawObjectHeader( _audio, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _audio ) )
				return;

			for( int i = 0 ; i < _audio.Clips.Count ; i++ )
			{
				ICEEditorLayout.BeginHorizontal();

				_audio.Clips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip #" + (int)(i+1), _audio.Clips[i], typeof(AudioClip), false);


				if( ICEEditorLayout.ListDeleteButtonMini<AudioClip>( _audio.Clips, _audio.Clips[i], "Removes this audio clip." ) )
					return;

				ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );
			}


			ICEEditorLayout.BeginHorizontal();
			_audio.AddClip( (AudioClip)EditorGUILayout.ObjectField("New Audio Clip", null, typeof(AudioClip), false) );
			if( ICEEditorLayout.AddButton( "Adds a new audio clip." ) )
				_audio.AddClip();
			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP_ADD );



			EditorGUILayout.Separator();

			_audio.Volume = ICEEditorLayout.DefaultSlider( "Volume", "The volume of the sound at the MinDistance",_audio.Volume, 0.05f, 0, 1, 0.5f, Info.AUDIO_VOLUME );
			ICEEditorLayout.MinMaxSlider( "Pitch", "", ref _audio.MinPitch, ref _audio.MaxPitch, 0, ref _audio.PitchMaximum, Init.DECIMAL_PRECISION, 40, Info.AUDIO_PITCH );
			ICEEditorLayout.MinMaxSlider( "Distance", "", ref _audio.MinDistance, ref _audio.MaxDistance, 0, ref _audio.DistanceMaximum, Init.DECIMAL_PRECISION_DISTANCES, 40, Info.AUDIO_DISTANCE );

			EditorGUILayout.Separator();
			_audio.Break = ICEEditorLayout.Toggle("Break","Breaks a playing audio clip?", _audio.Break, Info.AUDIO_BREAK );
			_audio.Loop = ICEEditorLayout.Toggle("Loop","Is the audio clip looping?", _audio.Loop, Info.AUDIO_LOOP );
			_audio.RolloffMode = (AudioRolloffMode)ICEEditorLayout.EnumPopup("RolloffMode", "Rolloff modes that a 3D sound can have in an audio source.", _audio.RolloffMode, Info.AUDIO_ROLLOFF );

			ICEEditorLayout.BeginHorizontal();
			_audio.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Audio Mixer Group", _audio.MixerGroup, typeof(AudioMixerGroup), false );
			ICEEditorLayout.EndHorizontal( Info.AUDIO_MIXER_GROUP );


			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the footstep audio object.
		/// </summary>
		/// <param name="_footstep">Footstep.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		public static void DrawFootstepAudioObject( FootstepAudioObject _footstep, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _footstep == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Footsteps";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			DrawObjectHeader( _footstep, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _footstep ) )
				return;


			Keyframe[] _keys = new Keyframe[3];
			_keys[0] = new Keyframe( 0, 0f );
			_keys[1] = new Keyframe( 3, 0.6f);
			_keys[2] = new Keyframe( 6, 0.3f);

			_footstep.Interval = ICEEditorLayout.DefaultCurve( "Footstep Interval", "", _footstep.Interval, new AnimationCurve(_keys) );
			EditorGUILayout.Separator();

			for( int i = 0 ; i < _footstep.Clips.Count ; i++ )
			{
				ICEEditorLayout.BeginHorizontal();

				_footstep.Clips[i] = (AudioClip)EditorGUILayout.ObjectField("Footstep Clip #" + (int)(i+1), _footstep.Clips[i], typeof(AudioClip), false);

				if( ICEEditorLayout.ListDeleteButtonMini<AudioClip>( _footstep.Clips, _footstep.Clips[i], "Removes this audio clip." ) )
					return;

				ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );
			}


			ICEEditorLayout.BeginHorizontal();
			_footstep.AddClip( (AudioClip)EditorGUILayout.ObjectField("New Footstep Clip", null, typeof(AudioClip), false) );
			if( ICEEditorLayout.AddButton( "Adds new audio clip" ) )
				_footstep.AddClip();
			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );



			EditorGUILayout.Separator();

			_footstep.Volume = ICEEditorLayout.DefaultSlider( "Volume", "The volume of the sound at the MinDistance",_footstep.Volume, 0.05f, 0, 1, 0.5f, Info.AUDIO_VOLUME );

			ICEEditorLayout.MinMaxSlider( "Pitch", "", ref _footstep.MinPitch, ref _footstep.MaxPitch, 0, ref _footstep.PitchMaximum, Init.DECIMAL_PRECISION, 40, Info.AUDIO_PITCH );
			ICEEditorLayout.MinMaxSlider( "Distance", "", ref _footstep.MinDistance, ref _footstep.MaxDistance, 0, ref _footstep.DistanceMaximum, Init.DECIMAL_PRECISION, 40, Info.AUDIO_DISTANCE );
			_footstep.RolloffMode = (AudioRolloffMode)ICEEditorLayout.EnumPopup("RolloffMode", "Rolloff modes that a 3D sound can have in an audio source.", _footstep.RolloffMode, Info.AUDIO_ROLLOFF );

			ICEEditorLayout.BeginHorizontal();
			_footstep.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Audio Mixer Group", _footstep.MixerGroup, typeof(AudioMixerGroup), false );
			ICEEditorLayout.EndHorizontal( Info.AUDIO_MIXER_GROUP );

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the audio object.
		/// </summary>
		/// <param name="_audio">Audio.</param>
		/// <param name="_help">Help.</param>
		public static void DrawAudioObject( AudioObject _audio, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _audio == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Audio";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			DrawObjectHeader( _audio, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _audio ) )
				return;

			DrawImpulsTimerObject( _audio );
			EditorGUILayout.Separator();

			for( int i = 0 ; i < _audio.Clips.Count ; i++ )
			{
				ICEEditorLayout.BeginHorizontal();

				_audio.Clips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip #" + (int)(i+1), _audio.Clips[i], typeof(AudioClip), false);

				if( ICEEditorLayout.ListDeleteButtonMini<AudioClip>( _audio.Clips, _audio.Clips[i], "Removes this audio clip." ) )
					return;

				ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );
			}


			ICEEditorLayout.BeginHorizontal();
			_audio.AddClip( (AudioClip)EditorGUILayout.ObjectField("New Audio Clip", null, typeof(AudioClip), false) );
			if( ICEEditorLayout.AddButton( "Adds new audio clip" ) )
				_audio.AddClip();
			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );



			EditorGUILayout.Separator();

			_audio.Volume = ICEEditorLayout.DefaultSlider( "Volume", "The volume of the sound at the MinDistance",_audio.Volume, 0.05f, 0, 1, 0.5f, Info.AUDIO_VOLUME );

			ICEEditorLayout.MinMaxSlider( "Pitch", "", ref _audio.MinPitch, ref _audio.MaxPitch, 0, ref _audio.PitchMaximum, Init.DECIMAL_PRECISION, 40, Info.AUDIO_PITCH );
			ICEEditorLayout.MinMaxSlider( "Distance", "", ref _audio.MinDistance, ref _audio.MaxDistance, 0, ref _audio.DistanceMaximum, Init.DECIMAL_PRECISION, 40, Info.AUDIO_DISTANCE );

			EditorGUILayout.Separator();
			_audio.StopAtEnd = ICEEditorLayout.Toggle("Stop","Stops a playing audio clip ath the end of the current action", _audio.StopAtEnd, Info.AUDIO_STOP );
			_audio.Break = ICEEditorLayout.Toggle("Break","Breaks a playing audio clip", _audio.Break, Info.AUDIO_BREAK );
			_audio.Loop = ICEEditorLayout.Toggle("Loop","Is the audio clip looping?", _audio.Loop, Info.AUDIO_LOOP );
			_audio.RolloffMode = (AudioRolloffMode)ICEEditorLayout.EnumPopup("RolloffMode", "Rolloff modes that a 3D sound can have in an audio source.", _audio.RolloffMode, Info.AUDIO_ROLLOFF );

			ICEEditorLayout.BeginHorizontal();
			_audio.MixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField("Audio Mixer Group", _audio.MixerGroup, typeof(AudioMixerGroup), false );
			ICEEditorLayout.EndHorizontal( Info.AUDIO_MIXER_GROUP );

			EndObjectContent();
			// CONTENT END
		}


		public static DurabilityInfluenceObject InfluencePopup( string _title, string _hint, DurabilityInfluenceObject _influence, List<DurabilityInfluenceObject> _list, string _help = "", params GUILayoutOption[] _gui )
		{
			ICEEditorLayout.BeginHorizontal();
	
				GUIContent[] _options = new GUIContent[_list.Count];
				int _selected = 0;
				for( int _i = 0 ; _i < _list.Count ; _i++ )
				{
					_options[ _i ] = new GUIContent( _list[_i].Key );

					if( _list[_i].Key == _influence.Key )
						_selected = _i;
				}

				_selected = EditorGUILayout.Popup( new GUIContent( _title, _hint ) , _selected, _options, _gui );

				_influence = _list[_selected];

			ICEEditorLayout.EndHorizontal( _help );
			return _influence;
		}
		/*
		public static bool DrawDurabilityInfluenceObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, int _index )
		{
			if( _composition == null || _index < 0 || _index >= _composition.Influences.Count )
				return false;

			DurabilityInfluenceObject _influence = _composition.Influences[_index];


			ICEEditorLayout.BeginHorizontal();

			_influence.Key = ICEEditorLayout.Text( "Key", "", _influence.Key , "" );


			if( ICEEditorLayout.ListUpDownButtons<DurabilityInfluenceObject>( _composition.Influences, _index ) )
				return true;

			if( ICEEditorLayout.ListDeleteButton<DurabilityInfluenceObject>( _composition.Influences, _influence ) )
				return true;

			ICEEditorLayout.EndHorizontal( "TODO" );

			return false;
		}

		public static bool DrawDurabilityAttributeObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, int _index )
		{
			if( _composition == null || _index < 0 || _index >= _composition.Attributes.Count )
				return false;

			DurabilityAttributeObject _attribute = _composition.Attributes[_index];


			ICEEditorLayout.BeginHorizontal();

					_attribute.Key = ICEEditorLayout.Text( "Key", "", _attribute.Key , "" );

				if( ICEEditorLayout.ListUpDownButtons<DurabilityAttributeObject>( _composition.Attributes, _index ) )
					return true;

				if( ICEEditorLayout.ListDeleteButton<DurabilityAttributeObject>( _composition.Attributes, _attribute ) )
					return true;

				if( ICEEditorLayout.AddButton( "" ) )
					_attribute.Multiplier.Add( new DurabilityInfluenceMultiplierObject() );

			ICEEditorLayout.EndHorizontal( "TODO" );

			for( int i = 0 ; i < _attribute.Multiplier.Count ; i++ )
			{
				DurabilityInfluenceMultiplierObject _multiplier = _attribute.Multiplier[i];

				if( _multiplier == null )
					continue;
				
				ICEEditorLayout.BeginHorizontal();

				//_multiplier.Influence = InfluencePopup( "", "", _multiplier.Influence, _composition.Influences, "", GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 250 ) );
				_multiplier.Multiplier = ICEEditorLayout.DefaultSlider( _multiplier.Influence.Key + " Multiplier", "", _multiplier.Multiplier, 0.001f, -1, 1, 0 );

				if( ICEEditorLayout.ListUpDownButtons<DurabilityInfluenceMultiplierObject>( _attribute.Multiplier, _attribute.Multiplier.IndexOf( _multiplier ) ) )
					return true;
				
				if( ICEEditorLayout.ListDeleteButton<DurabilityInfluenceMultiplierObject>( _attribute.Multiplier, _multiplier ) )
					return true;

				ICEEditorLayout.EndHorizontal( "TODO" );
			}

			return false;
		}


		public static void DrawDurabilityCompositionObject( ICEWorldBehaviour _component, DurabilityCompositionObject _composition, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _composition == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Durability Composition";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = "TODO";

			ICEEditorLayout.BeginHorizontal();

			if( IsEnabledFoldoutType( _type ) )
				EditorGUI.BeginDisabledGroup( _composition.Enabled == false );

				DrawObjectHeaderLine( _composition, GetSimpleFoldout( _type ), _title, _hint );	


				if( ICEEditorLayout.SaveButton() )
					ICEWorldIO.SaveDurabilityCompositionToFile( _composition, _component.name );
				if( ICEEditorLayout.LoadButton() )
					_composition.Copy( ICEWorldIO.LoadDurabilityCompositionFromFile( new DurabilityCompositionObject() ) );
				if( ICEEditorLayout.ResetButton() )
					_composition.Reset();

				if( IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.EndDisabledGroup();
					_composition.Enabled = ICEEditorLayout.EnableButton( _composition.Enabled );
				}

			ICEEditorLayout.EndHorizontal( _help );

	
			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _composition ) )
				return;


			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.Label( "Entity Influences", true );

			if( ICEEditorLayout.AddButton( "" ) )
			{
				_composition.AddInfluenceByKey( "NEW" );
			}


			ICEEditorLayout.EndHorizontal();

			for( int i = 0 ; i < _composition.Influences.Count ; i++ )
				if( DrawDurabilityInfluenceObject( _component, _composition, i ) )
					return;


			EditorGUILayout.Separator();

			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.Label( "Entity Attributes", true );

			if( ICEEditorLayout.AddButton( "" ) )
			{
				_composition.AddAttributeByKey( "NEW" );
			}

			ICEEditorLayout.EndHorizontal();

			for( int i = 0 ; i < _composition.Attributes.Count ; i++ )
				if( DrawDurabilityAttributeObject( _component, _composition, i ) )
					return;

			EndObjectContent();
			// CONTENT END
		}
*/

		public static void DrawLayerObject( LayerObject _layer, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _layer == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Layer";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			_title += ( _layer.Layers.Count == 0 ? " (0)" : " (" + _layer.Layers.Count + ")" );

			ICEEditorLayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup( _layer.Enabled == false );
			DrawObjectHeaderLine( _layer, GetSimpleFoldout( _type ), _title, _hint );
			EditorGUI.EndDisabledGroup();

			if( ICEEditorLayout.AddButtonSmall( "" ) )
				_layer.Layers.Add( "Default" );

			if( ICEEditorLayout.ClearButtonSmall( "" ) )
				_layer.Layers.Clear();

			if( IsEnabledType( _type ) )
				_layer.Enabled = ICEEditorLayout.EnableButton( _layer.Enabled );
			ICEEditorLayout.EndHorizontal( _help );



			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _layer ) )
				return;

				ICEEditorLayout.DrawLayersList( _layer.Layers );

			EndObjectContent();
			// CONTENT END
		}

		/*
		public static void DrawLayerObject( LayerObject _layer, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _layer == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Layers";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LAYERS;

			_title += ( _layer.Layers.Count == 0 ? " (0)" : " (" + _layer.Layers.Count + ")" );

			ICEEditorLayout.BeginHorizontal();

				DrawObjectHeaderLine( _layer, _type, _title, _hint );	

				if( ICEEditorLayout.Button( "Add Layer" ) )
					_layer.Layers.Add( "Default" );

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _layer ) )
				return;

			ICEEditorLayout.DrawLayersList( _layer.Layers );


			EndObjectContent();
			// CONTENT END
		}
		*/

		public static void DrawEntityRuntimeBehaviourObject( ICEWorldBehaviour _component, EntityRuntimeBehaviourObject _behaviour, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _behaviour == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Runtime Behaviour";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RUNTIME_BEHAVIOURS;

			if( IsEnabledFoldoutType( _type ) )
				_type = GetSimpleFoldout( _type );

			DrawObjectHeader( _behaviour, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _behaviour ) )
				return;
			
			_behaviour.Enabled = true;
			_behaviour.UseCoroutine = ICEEditorLayout.Toggle("Use Coroutine","", _behaviour.UseCoroutine, Info.RUNTIME_COROUTINE );
			_behaviour.UseDontDestroyOnLoad = ICEEditorLayout.Toggle("Dont Destroy On Load","", _behaviour.UseDontDestroyOnLoad, Info.RUNTIME_DONTDESTROYONLOAD );
			_behaviour.UseHierarchyManagement = ICEEditorLayout.Toggle("Use Hierarchy Management", "Allows the Hierarchy Management to reassigns the parent if required.", _behaviour.UseHierarchyManagement, Info.USE_HIERARCHY_MANAGEMENT );
			_behaviour.UseRemoveOnLost = ICEEditorLayout.Toggle("Use Remove On Lost","", _behaviour.UseRemoveOnLost, Info.RUNTIME_COROUTINE );
			if( _behaviour.UseRemoveOnLost )
			{
				EditorGUI.indentLevel++;
				_behaviour.LostRemovingLevel = ICEEditorLayout.MaxDefaultSlider( "Removing Level", "", _behaviour.LostRemovingLevel, Init.DECIMAL_PRECISION_DISTANCES, - _behaviour.LostRemovingLevelMaximum, ref _behaviour.LostRemovingLevelMaximum, -200, Info.RUNTIME_NAME );
				EditorGUI.indentLevel--;
			}
				
			WorldObjectEditor.DrawCullingOptionsObject( _component, _behaviour.CullingOptions, EditorHeaderType.TOGGLE );

			_behaviour.UseRuntimeName = ICEEditorLayout.Toggle("Runtime Name", "Adapts the name of the GameObject during the runtime", _behaviour.UseRuntimeName, Info.RUNTIME_NAME );
			if( _behaviour.UseRuntimeName )
			{
				EditorGUI.indentLevel++;
				_behaviour.RuntimeName = ICEEditorLayout.Text( "Runtime Name", "", ( ! string.IsNullOrEmpty( _behaviour.RuntimeName ) ? _behaviour.RuntimeName : _component.gameObject.name ), Info.RUNTIME_NAME );
				EditorGUI.indentLevel--;
			}

			ICEWorldEntity _entity = _component as ICEWorldEntity;
			if( _entity != null )
				_entity.BaseOffset = ICEEditorLayout.MaxDefaultSlider( "Base Offset", "", _entity.BaseOffset, Init.DECIMAL_PRECISION_DISTANCES, - _entity.BaseOffsetMaximum, ref _entity.BaseOffsetMaximum, 0, Info.RUNTIME_BASE_OFFSET );

			EditorGUILayout.Separator();


			EndObjectContent();
			// CONTENT END
		}

		public static void DrawCullingOptionsObject( ICEWorldBehaviour _component, CullingOptionsObject _options, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _options == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Culling Options";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RUNTIME_CULLING_CONDITIONS;

			DrawObjectHeader( _options, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _options ) )
				return;

				ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.MinMaxSlider( "Main Camera Distance (min/max)", "Defines the minimum and maximum distances in relation of the direction of the main camera.", ref _options.MainCameraDistanceMin, ref _options.MainCameraDistanceMax, - _options.MainCameraDistanceMaximum, ref _options.MainCameraDistanceMaximum, Init.DECIMAL_PRECISION_DISTANCES, 40 );
				ICEEditorLayout.EndHorizontal( Info.RUNTIME_CULLING_CONDITIONS_MAIN_CAMERA_DISTANCE );//Info.REGISTER_REFERENCE_OBJECT_POOL_SPAWN_CONDITIONS_MAIN_CAMERA_DISTANCE  );	

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the odour object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_odour">Odour.</param>
		/// <param name="_help">Help.</param>
		public static void DrawOdourObject( OdourObject _odour, string _help = "", string _title = "", string _hint = "" )
		{
			if( _odour == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Odour";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ODOUR;

			_odour.Type = (OdourType)ICEEditorLayout.EnumPopup( _title, _hint, _odour.Type, _help );			
			if( _odour.Type == OdourType.NONE )
				return;

			EditorGUI.indentLevel++;
			_odour.Intensity = ICEEditorLayout.MaxDefaultSlider( "Intensity", "", _odour.Intensity , 1, 0, ref _odour.IntensityMax, 0, Info.ODOUR_INTENSITY );
			_odour.Range = ICEEditorLayout.MaxDefaultSlider( "Range", "", _odour.Range , 1, 0, ref _odour.RangeMax, 0, Info.ODOUR_RANGE );

			_odour.UseMarker = ICEEditorLayout.Toggle( "Use Odour Marker", "" , _odour.UseMarker , Info.ODOUR_MARKER );
			if( _odour.UseMarker )
			{
				EditorGUI.indentLevel++;
				ICEEditorLayout.MinMaxRandomDefaultSlider( "Interval", "", ref _odour.MarkerMinInterval, ref _odour.MarkerMaxInterval, 0, ref _odour.MarkerIntervalMax, 2, 5, 0.25f, 30, Info.ODOUR_MARKER_INTERVAL );
				_odour.MarkerPrefab = (GameObject)EditorGUILayout.ObjectField( "Marker Prefab", _odour.MarkerPrefab, typeof(GameObject), false );
				EditorGUI.indentLevel--;
			}

			_odour.UseEffect = ICEEditorLayout.Toggle( "Use Odour Effect", "" , _odour.UseEffect , Info.ODOUR_EFFECT );
			if( _odour.UseEffect )
			{
				EditorGUI.indentLevel++;
				_odour.EffectPrefab = (GameObject)EditorGUILayout.ObjectField( "Effect Prefab", _odour.EffectPrefab, typeof(GameObject), false );
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
		}


		public static void DrawCorpseObject( ICEWorldBehaviour _component, CorpseObject _corpse, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _corpse == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Corpse";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.CORPSE;

			DrawObjectHeader( _corpse, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _corpse ) )
				return;

				ICEEditorLayout.BeginHorizontal();
					_corpse.CorpseReferencePrefab = (GameObject)EditorGUILayout.ObjectField( "Body Prefab", _corpse.CorpseReferencePrefab, typeof(GameObject), false );
					_corpse.UseCorpseScaling = ICEEditorLayout.CheckButtonMiddle( "SCALE", "", _corpse.UseCorpseScaling );
				ICEEditorLayout.EndHorizontal( Info.CORPSE_REFERENCE );
				ICEEditorLayout.BeginHorizontal();
					if( _corpse.UseRandomDelay )
						ICEEditorLayout.MinMaxRandomDefaultSlider( "Removing Delay (secs.)","Defines how long the corpse will be visible after dying.", ref _corpse.CorpseRemovingDelayMin, ref _corpse.CorpseRemovingDelayMax, 0, ref _corpse.CorpseRemovingDelayMaximum, 0, 0, Init.DECIMAL_PRECISION_TIMER, 30 );
					else
						_corpse.CorpseRemovingDelay = ICEEditorLayout.MaxDefaultSlider("Removing Delay (secs.)","Defines how long the corpse will be visible after dying.", _corpse.CorpseRemovingDelay, 0.5f, 0, ref _corpse.CorpseRemovingDelayMaximum, 0 );
					_corpse.UseRandomDelay = ICEEditorLayout.CheckButtonMiddle( "RANDOM", "", _corpse.UseRandomDelay );
				ICEEditorLayout.EndHorizontal( Info.CORPSE_REMOVING_DELAY );

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawEntityStatusObject( ICEWorldBehaviour _component, EntityStatusObject _status, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _status == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Status";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LIFESPAN;



			//LIFESPAN BEGIN
			DrawObjectHeader( _status, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _status ) )
				return;

				DrawStatusMass( _status );
				DrawStatusLifespan( _status );				
				DrawInitialDurability( _status );
				DrawDurabilitySlider( _status );
				DrawDamageTransfer( _status );
				

				EditorGUILayout.Separator();
				WorldObjectEditor.DrawCorpseObject( _component, _status.Corpse, EditorHeaderType.TOGGLE );
				WorldObjectEditor.DrawOdourObject( _status.Odour );
		
	
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawStatusLifespan( EntityStatusObject _status )
		{
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _status.UseLifespan == false );
					ICEEditorLayout.MinMaxSlider( "Lifespan", "", ref _status.LifespanMin, ref _status.LifespanMax, 0, ref _status.LifespanMaximum, 0.25f, 40, "" );

					if( ICEEditorLayout.RandomButton( "" ) )
					{
						_status.LifespanMax = Random.Range( _status.LifespanMin, _status.LifespanMaximum );
						_status.LifespanMin = Random.Range( 0, _status.LifespanMax );
					}

					ICEEditorLayout.ButtonMinMaxDefault( ref _status.LifespanMin, ref _status.LifespanMax, 0, 0 );
				EditorGUI.EndDisabledGroup();
				_status.UseLifespan = ICEEditorLayout.EnableButton( _status.UseLifespan );
			ICEEditorLayout.EndHorizontal( Info.LIFESPAN );
		}

		public static void DrawStatusAging( EntityStatusObject _status )
		{
			if( _status == null )
				return;

			_status.UseAging = ICEEditorLayout.Toggle("Use Aging","", _status.UseAging, Info.AGING );			
			if( _status.UseAging )
			{			
				EditorGUI.indentLevel++;
				ICEEditorLayout.BeginHorizontal();
					ICEEditorLayout.MinMaxSlider( "Lifespan", "", ref _status.LifespanMin, ref _status.LifespanMax, 0, ref _status.LifespanMaximum, 0.25f, 40, "" );

					if( ICEEditorLayout.RandomButton( "" ) )
					{
						_status.LifespanMax = Random.Range( _status.LifespanMin, _status.LifespanMaximum );
						_status.LifespanMin = Random.Range( 0, _status.LifespanMax );
					}

					ICEEditorLayout.ButtonMinMaxDefault( ref _status.LifespanMin, ref _status.LifespanMax, 0, 0 );
				ICEEditorLayout.EndHorizontal( Info.AGING );

				float _age = _status.Age/60;
				float _max_age = Mathf.Floor( ( Application.isPlaying ? _status.ExpectedLifespan : _status.LifespanMax )/60 );
				_status.SetAge( ICEEditorLayout.DefaultSlider( "Age (minutes)", "", _age , 1, 0, _max_age, 0, Info.AGING_AGE )*60 );
				_status.MaxAge = _max_age*60;
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
		}

		public static void DrawDurabilitySlider( EntityStatusObject _status )
		{
			EditorGUI.indentLevel++;
				EditorGUI.BeginDisabledGroup( _status.IsDestructible == false );
					if( ! Application.isPlaying )					
						_status.SetInitialDurability( _status.InitialDurabilityMax );
					_status.SetDurability( ICEEditorLayout.DefaultSlider( "Durability", "", _status.Durability, 0.0001f, 0, _status.InitialDurability, _status.InitialDurability, "" ) );
				EditorGUI.EndDisabledGroup();
			EditorGUI.indentLevel--;
		}

		public static void DrawDamageTransfer( EntityStatusObject _status, string _help = "" )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BODYPART_DAMAGE_TRANSFER;
			
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _status.UseDamageTransfer == false );
					_status.DamageTransferMultiplier = ICEEditorLayout.MaxDefaultSlider( "Damage Transfer Multiplier", "", _status.DamageTransferMultiplier, Init.DECIMAL_PRECISION, - _status.DamageTransferMultiplierMaximum, ref _status.DamageTransferMultiplierMaximum, 1 );
				EditorGUI.EndDisabledGroup();
				_status.UseDamageTransfer = ICEEditorLayout.EnableButton( "Allows damage forwarding to the current parent object.", _status.UseDamageTransfer );
			ICEEditorLayout.EndHorizontal( _help );

		}

		public static void DrawStatusMass( EntityStatusObject _status, string _help = "" )
		{
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.STATUS_MASS;

			ICEEditorLayout.BeginHorizontal();

				if( _status.MassMaximum <= 0.1f )
					_status.MassMaximum = 100;
			
				ICEEditorLayout.MinMaxSlider( "Mass", "", ref _status.MassMin,  ref _status.MassMax, 0.1f, ref _status.MassMaximum, Init.DECIMAL_PRECISION_MASS, 30 );
				
				int _min = Mathf.RoundToInt(_status.MassMin - ( _status.MassMaximum * 0.1f ) );
				int _max = Mathf.RoundToInt(_status.MassMax + ( _status.MassMaximum * 0.1f ) );

				if( ICEEditorLayout.ButtonOptionMini( "|<", _status.MassMax, 0.1f, "" ) == 0.1f )
				{
					_status.MassMin = 0.1f;
					_status.MassMax = 0.1f;
				}
				if( ICEEditorLayout.ButtonOptionMini( "<", _status.MassMax, _min, "" ) == _min )
				{
					_status.MassMin = ( _min < 0 ? 0.1f : _min );
					_status.MassMax = _status.MassMin;
				}
				if( ICEEditorLayout.ButtonOptionMini( ">", _status.MassMax, _max, "" ) == _max )
				{
					_status.MassMax = ( _max < 10 ? 10 : ( _max > _status.MassMaximum ? _status.MassMaximum : _max ) );
					_status.MassMin = _status.MassMax;
				}

				if( ICEEditorLayout.ButtonOptionMini( ">|", _status.MassMax, _status.MassMaximum, "" ) == _status.MassMaximum )
				{
					_status.MassMin = _status.MassMaximum;
					_status.MassMax = _status.MassMin;
				}
				if( ICEEditorLayout.RandomButton( "" ) )
				{
					_status.MassMax = UnityEngine.Random.Range( 0, _status.MassMaximum );
					_status.MassMin = UnityEngine.Random.Range( 0, _status.MassMax );					
				}

			ICEEditorLayout.EndHorizontal( _help );

		}


		/// <summary>
		/// Draws the initial durability.
		/// </summary>
		/// <param name="_status">Status.</param>
		public static void DrawInitialDurability( EntityStatusObject _status )
		{
			ICEEditorLayout.BeginHorizontal();

				EditorGUI.BeginDisabledGroup( _status.IsDestructible == false );
					ICEEditorLayout.MinMaxSlider( "Initial Durability (" + ( Mathf.Round( _status.InitialDurability / 0.01f ) * 0.01f ) + ")", "Defines the default physical integrity of the creature.", 
						ref _status.InitialDurabilityMin, 
						ref _status.InitialDurabilityMax,
						1, ref _status.InitialDurabilityMaximum, 1, 40, "" );

					ICEEditorLayout.ButtonMinMaxDefault( ref _status.InitialDurabilityMin, ref _status.InitialDurabilityMax, _status.InitialDurabilityMaximum, _status.InitialDurabilityMaximum );
				EditorGUI.EndDisabledGroup();

				_status.IsDestructible = ICEEditorLayout.EnableButton( _status.IsDestructible );
			ICEEditorLayout.EndHorizontal( Info.DURABILITY_INITIAL );
			//EditorGUILayout.Separator();
			EditorGUI.BeginDisabledGroup( _status.IsDestructible == false );
			//ICEEditorLayout.DrawProgressBar( "Durability (%)", _status.DurabilityInPercent, Info.DURABILITY_PERCENT );
			EditorGUI.EndDisabledGroup();
		}
		/*
		public static void DrawBodyPartObject( EntityBodyPartObject _part, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _part == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Body Part";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BODYPART;

			DrawObjectHeader( _part, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _part ) )
				return;
			
			ICEEditorLayout.BeginHorizontal();

				_part.DamageMultiplier = ICEEditorLayout.MaxDefaultSlider( "Damage Transfer Multiplier", "", _part.DamageMultiplier, Init.DECIMAL_PRECISION, - _part.DamageMultiplierMaximum, ref _part.DamageMultiplierMaximum, 1 );

				_part.UseDamageTransfer = ICEEditorLayout.CheckButtonMiddle( "TRANSFER", "Allows damage transfer for body parts", _part.UseDamageTransfer );
			ICEEditorLayout.EndHorizontal( Info.BODYPART_DAMAGE_TRANSFER );

			EndObjectContent();
			// CONTENT END
		}*/

		/// <summary>
		/// Draws the lifespan object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_lifespan">Lifespan.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEntityLifespanObject( LifespanObject _lifespan, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _lifespan == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Lifespan";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LIFESPAN;

			//LIFESPAN BEGIN
			DrawObjectHeader( _lifespan, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _lifespan ) )
				return;

			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.MinMaxSlider( "Lifespan", "", ref _lifespan.LifespanMin, ref _lifespan.LifespanMax, 0, ref _lifespan.LifespanMaximum, 0.25f, 40, "" );

			if( ICEEditorLayout.RandomButton( "" ) )
			{
				_lifespan.LifespanMax = Random.Range( _lifespan.LifespanMin, _lifespan.LifespanMaximum );
				_lifespan.LifespanMin = Random.Range( 0, _lifespan.LifespanMax );
			}

			ICEEditorLayout.ButtonMinMaxDefault( ref _lifespan.LifespanMin, ref _lifespan.LifespanMax, 0, 0 );

			ICEEditorLayout.EndHorizontal();
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawImpactBehaviourObject( ICEWorldBehaviour _component, ImpactBehaviourObject _behaviour, bool _is_basic, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _behaviour == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Behaviour";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.IMPACT_BEHAVIOUR;


			//LIFESPAN BEGIN
			DrawObjectHeader( _behaviour, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _behaviour ) )
				return;

			DrawLayerObject( _behaviour.ImpactLayer, EditorHeaderType.FOLDOUT_ENABLED, Info.IMPACT_BEHAVIOUR_LAYER, "Layer Mask" );
			/*
			_behaviour.UseImpactDelay = ICEEditorLayout.Toggle( "Impact Delay", "", _behaviour.UseImpactDelay, Info.IMPACT_BEHAVIOUR_USE_IMPACT_DELAY );
			EditorGUI.BeginDisabledGroup( _behaviour.UseImpactDelay == false );
			EditorGUI.indentLevel++;
			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.RandomMinMaxGroupExt( "Impact Delay", "", ref _behaviour.ImpactDelayMin, ref _behaviour.ImpactDelayMax, 0, ref _behaviour.ImpactDelayMaximum,2,2, 30, Init.DECIMAL_PRECISION_TIMER );
			ICEEditorLayout.EndHorizontal( Info.IMPACT_BEHAVIOUR_IMPACT_DELAY );
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();*/

			_behaviour.AllowOwnImpacts = ICEEditorLayout.Toggle( "Allow Own Impacts", "", _behaviour.AllowOwnImpacts, Info.IMPACT_BEHAVIOUR_ALLOW_OWN_IMPACTS );

			if( _is_basic )
			{
				_behaviour.UseDestroyOnHit = false;
				_behaviour.UseIgnoreHits = false;
			}
			else
			{
				// BEGIN ATTACH ON HIT
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _behaviour.UseAttachOnHit == false );
						ICEEditorLayout.Label( "Attach On Hit" );
					EditorGUI.EndDisabledGroup();
					_behaviour.UseAttachOnHit = ICEEditorLayout.EnableButton( _behaviour.UseAttachOnHit );
				ICEEditorLayout.EndHorizontal( Info.IMPACT_BEHAVIOUR_USE_ATTACH );
				// END ATTACH ON HIT

				// BEGIN HIDE ON HIT
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _behaviour.UseHideOnHit == false );
						ICEEditorLayout.Label( "Hide On Hit" );
					EditorGUI.EndDisabledGroup();
					_behaviour.UseHideOnHit = ICEEditorLayout.EnableButton( _behaviour.UseHideOnHit );
				ICEEditorLayout.EndHorizontal( Info.IMPACT_BEHAVIOUR_USE_HIDE );
				// END HIDE ON HIT

				// BEGIN DESTROY ON HIT
				ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _behaviour.UseDestroyOnHit == false );
						ICEEditorLayout.MinMaxDefaultSlider( "Destroy On Hit (delay)", "", ref _behaviour.DestroyingDelayMin, ref _behaviour.DestroyingDelayMax, 0, ref _behaviour.DestroyingDelayMaximum,2,2, Init.DECIMAL_PRECISION_TIMER, 30 );
					EditorGUI.EndDisabledGroup();

					_behaviour.UseDestroyOnHit = ICEEditorLayout.EnableButton( _behaviour.UseDestroyOnHit );
				ICEEditorLayout.EndHorizontal( Info.IMPACT_BEHAVIOUR_USE_DESTROY );
				// END DESTROY ON HIT

				// BEGIN IGNORE HITS
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _behaviour.UseIgnoreHits == false );
						ICEEditorLayout.MinMaxDefaultSlider( "Ignore Hits (min/max)", "", ref _behaviour.HitCountMin, ref _behaviour.HitCountMax, 0, ref _behaviour.HitCountMaximum,0,0, 30 );
					EditorGUI.EndDisabledGroup();
					_behaviour.UseIgnoreHits = ICEEditorLayout.EnableButton( _behaviour.UseIgnoreHits );
				ICEEditorLayout.EndHorizontal( Info.IMPACT_BEHAVIOUR_TRIGGERING_HITS );
				// END IGNORE HITS
			}



			EndObjectContent();
			// CONTENT END
		}


		/// <summary>
		/// Draws the effect object.
		/// </summary>
		/// <returns>The effect object.</returns>
		/// <param name="_control">Control.</param>
		/// <param name="_effect">Effect.</param>
		/// <param name="_help">Help.</param>
		public static void DrawDirectEffectObject( ICEWorldBehaviour _control, DirectEffectObject _effect, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _effect == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Effect";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EFFECT;

			DrawObjectHeader( _effect, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _effect ) )
				return;

			ICEEditorLayout.BeginHorizontal();
			_effect.ReferenceObject = (GameObject)EditorGUILayout.ObjectField( "Reference", _effect.ReferenceObject, typeof(GameObject), false);
			EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );
			_effect.Attached = ICEEditorLayout.CheckButtonMiddle( "Attached", "Attaches the effect instance to the given entity." , _effect.Attached ); 
			EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.EFFECT_REFERENCE );

			EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );

				EditorGUI.BeginDisabledGroup( _effect.Attached == true );
					EditorGUI.indentLevel++;
						_effect.Lifetime = ICEEditorLayout.MaxDefaultSlider( "Lifetime", "", _effect.Lifetime, Init.DECIMAL_PRECISION_TIMER, 0, ref _effect.LifetimeMaximum, 0, Info.EFFECT_DESTROY_DELAY );
					EditorGUI.indentLevel--;	
				EditorGUI.EndDisabledGroup();

			if( string.IsNullOrEmpty( _effect.MountPointName ) )
				_effect.MountPointName = _control.transform.name;

			_effect.MountPointName = ICEEditorLayout.TransformPopup( "Spawn Point", "", _effect.MountPointName, _control.transform, false, Info.EFFECT_MOUNTPOINT );

			_effect.OffsetType = (RandomOffsetType)ICEEditorLayout.EnumPopup( "Offset Type","", _effect.OffsetType, Info.EFFECT_OFFSET_TYPE );
			EditorGUI.indentLevel++;
			if( _effect.OffsetType == RandomOffsetType.EXACT )
				_effect.Offset = ICEEditorLayout.OffsetGroup( "Offset", _effect.Offset, Info.EFFECT_OFFSET_POSITION );
			else 
				_effect.OffsetRadius = ICEEditorLayout.MaxDefaultSlider( "Offset Radius", "", _effect.OffsetRadius, 0.25f, 0, ref _effect.OffsetRadiusMaximum, 0, Info.EFFECT_OFFSET_RADIUS );

			_effect.Rotation.eulerAngles = ICEEditorLayout.EulerGroup( "Rotation", _effect.Rotation.eulerAngles, Info.EFFECT_OFFSET_RADIUS );
			EditorGUI.indentLevel--;

			EditorGUI.EndDisabledGroup();

			EndObjectContent();
			// CONTENT END
		}


		/// <summary>
		/// Draws the effect object.
		/// </summary>
		/// <returns>The effect object.</returns>
		/// <param name="_control">Control.</param>
		/// <param name="_effect">Effect.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEffectObject( ICEWorldBehaviour _control, EffectObject _effect, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _effect == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Effect";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EFFECT;

			DrawObjectHeader( _effect, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _effect ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				_effect.ReferenceObject = (GameObject)EditorGUILayout.ObjectField( "Reference", _effect.ReferenceObject, typeof(GameObject), false);
				EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );
					_effect.Detach = ICEEditorLayout.CheckButtonMiddle( "DETACH", "Detaches the effect instance and will create further ones acording to the given interval" , _effect.Detach ); 
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal( Info.EFFECT_REFERENCE );

			EditorGUI.BeginDisabledGroup( _effect.ReferenceObject == null );

				DrawImpulsTimerObject( _effect );

				_effect.MountPointName = ICEEditorLayout.TransformPopup( "Mount Point", "", _effect.MountPointName, _control.transform, true, Info.EFFECT_MOUNTPOINT );
				_effect.OffsetType = (RandomOffsetType)ICEEditorLayout.EnumPopup( "Offset Type","", _effect.OffsetType, Info.EFFECT_OFFSET_TYPE );
				EditorGUI.indentLevel++;
					if( _effect.OffsetType == RandomOffsetType.EXACT )
						_effect.Offset = ICEEditorLayout.OffsetGroup( "Offset", _effect.Offset, Info.EFFECT_OFFSET_POSITION );
					else 
						_effect.OffsetRadius = ICEEditorLayout.MaxDefaultSlider( "Offset Radius", "", _effect.OffsetRadius, 0.25f, 0, ref _effect.OffsetRadiusMaximum, 0, Info.EFFECT_OFFSET_RADIUS );
				
					_effect.Rotation.eulerAngles = ICEEditorLayout.EulerGroup( "Rotation", _effect.Rotation.eulerAngles, Info.EFFECT_OFFSET_RADIUS );
				EditorGUI.indentLevel--;

			EditorGUI.EndDisabledGroup();

			EndObjectContent();
			// CONTENT END
		}


		public static void DrawTimerObject( ICETimerObject _timer, EditorHeaderType _type = EditorHeaderType.NONE , string _help = "", string _title = "", string _hint = ""  )
		{
			if( _timer.ImpulseIntervalMaximum == 0 )
				_timer.ImpulseIntervalMaximum = 10;
			if( _timer.ImpulseSequenceLimitMaximum == 0 )
				_timer.ImpulseSequenceLimitMaximum = 10;
			if( _timer.ImpulseSequenceBreakLengthMaximum == 0 )
				_timer.ImpulseSequenceBreakLengthMaximum = 10;


			if( _type != EditorHeaderType.NONE )
			{
				if( string.IsNullOrEmpty( _title ) )
					_title = "Timer";
				if( string.IsNullOrEmpty( _hint ) )
					_hint = "";
				if( string.IsNullOrEmpty( _help ) )
					_help = Info.IMPULSE_TIMER;


				ICEEditorLayout.BeginHorizontal();
				bool _enabled = _timer.TimerEnabled;
				DrawObjectHeaderLine( ref _enabled, ref _timer.TimerFoldout, _type, _title, _hint );
				_timer.TimerEnabled = _enabled;
				ICEEditorLayout.EndHorizontal( _help );

				// CONTENT BEGIN
				if( BeginObjectContentOrReturn( _type, _timer.TimerEnabled, _timer.TimerFoldout ) )
					return;
			}
			else
				_timer.TimerEnabled = true;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _timer.UseEnd == true /*|| _timer.UseTrigger == true*/ );
				_timer.InitialImpulsTime = ICEEditorLayout.MaxDefaultSlider( "Start Time (secs.)", "Time in seconds of the first impulse.", _timer.InitialImpulsTime , Init.DECIMAL_PRECISION_TIMER, 0, ref _timer.InitialImpulsTimeMaximum, 0 );

				_timer.UseInterval = ICEEditorLayout.CheckButtonSmall( "INT", "", _timer.UseInterval );

				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _timer.UseInterval == true /*|| _timer.UseTrigger == true*/ );
				_timer.UseEnd = ICEEditorLayout.CheckButtonSmall( "END", "", _timer.UseEnd );
				EditorGUI.EndDisabledGroup();
			/*
				EditorGUI.BeginDisabledGroup( _timer.UseEnd == true || _timer.UseInterval == true );
				_timer.UseTrigger = ICEEditorLayout.CheckButtonSmall( "TRG", "", _timer.UseTrigger );
				EditorGUI.EndDisabledGroup();
				*/

			ICEEditorLayout.EndHorizontal( Info.IMPULSE_TIMER_TIME );

			if( _timer.UseInterval )
			{

				ICEEditorLayout.MinMaxRandomDefaultSlider( "Impulse Interval (secs.)", "", ref _timer.ImpulseIntervalMin, ref _timer.ImpulseIntervalMax, 0, ref _timer.ImpulseIntervalMaximum,0,0, Init.DECIMAL_PRECISION_TIMER, 30, Info.IMPULSE_TIMER_INTERVAL );

				EditorGUI.indentLevel++;

				ICEEditorLayout.MinMaxRandomDefaultSlider( "Impulse Limit", "", ref _timer.ImpulseLimitMin, ref _timer.ImpulseLimitMax, 0, ref _timer.ImpulseLimitMaximum,0,0, 30, Info.IMPULSE_TIMER_LIMITS );

				if( Mathf.Max( _timer.ImpulseIntervalMin, _timer.ImpulseIntervalMax ) > 0 )
				{
					ICEEditorLayout.MinMaxRandomDefaultSlider( "Sequence Limit", "", ref _timer.ImpulseSequenceLimitMin, ref _timer.ImpulseSequenceLimitMax, 0, ref _timer.ImpulseSequenceLimitMaximum,0,0, 30, Info.IMPULSE_TIMER_SEQUENCE_LIMITS );

					EditorGUI.BeginDisabledGroup( _timer.ImpulseSequenceLimitMax == 0 );
						ICEEditorLayout.MinMaxRandomDefaultSlider( "Break Length (secs.)", "", ref _timer.ImpulseSequenceBreakLengthMin, ref _timer.ImpulseSequenceBreakLengthMax, 0, ref _timer.ImpulseSequenceBreakLengthMaximum,2,5, Init.DECIMAL_PRECISION_TIMER, 30, Info.IMPULSE_TIMER_SEQUENCE_BREAK_LENGTH );
					EditorGUI.EndDisabledGroup();

				}

				EditorGUI.indentLevel--;
			}

			if( _type != EditorHeaderType.NONE )
			{
				EndObjectContent();
				// CONTENT END
			}
		}


		/// <summary>
		/// Draws the impuls timer object.
		/// </summary>
		/// <param name="_timer">Timer.</param>
		public static void DrawImpulsTimerObject( ICEImpulsTimerObject _timer, EditorHeaderType _type = EditorHeaderType.NONE , string _help = "", string _title = "", string _hint = ""  )
		{
			if( _timer.ImpulseIntervalMaximum == 0 )
				_timer.ImpulseIntervalMaximum = 10;
			if( _timer.ImpulseSequenceLimitMaximum == 0 )
				_timer.ImpulseSequenceLimitMaximum = 10;
			if( _timer.ImpulseSequenceBreakLengthMaximum == 0 )
				_timer.ImpulseSequenceBreakLengthMaximum = 10;


			if( _type != EditorHeaderType.NONE )
			{
				if( string.IsNullOrEmpty( _title ) )
					_title = "Timer";
				if( string.IsNullOrEmpty( _hint ) )
					_hint = "";
				if( string.IsNullOrEmpty( _help ) )
					_help = Info.IMPULSE_TIMER;


				ICEEditorLayout.BeginHorizontal();
					bool _enabled = _timer.TimerEnabled;
					DrawObjectHeaderLine( ref _enabled, ref _timer.TimerFoldout, _type, _title, _hint );
					_timer.TimerEnabled = _enabled;
				ICEEditorLayout.EndHorizontal( _help );

				// CONTENT BEGIN
				if( BeginObjectContentOrReturn( _type, _timer.TimerEnabled, _timer.TimerFoldout ) )
					return;
			}
			else
				_timer.TimerEnabled = true;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _timer.UseEnd == true || _timer.UseTrigger == true );
					_timer.InitialImpulsTime = ICEEditorLayout.MaxDefaultSlider( "Start Time (secs.)", "Time in seconds of the first impulse.", _timer.InitialImpulsTime , Init.DECIMAL_PRECISION_TIMER, 0, ref _timer.InitialImpulsTimeMaximum, 0 );

					_timer.UseInterval = ICEEditorLayout.CheckButtonSmall( "INT", "", _timer.UseInterval );

				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _timer.UseInterval == true || _timer.UseTrigger == true );
					_timer.UseEnd = ICEEditorLayout.CheckButtonSmall( "END", "Fires one impluse at the end only", _timer.UseEnd );
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _timer.UseEnd == true || _timer.UseInterval == true );
					_timer.UseTrigger = ICEEditorLayout.CheckButtonSmall( "TRG", "A triggered timer will be controlled by an external event", _timer.UseTrigger );
				EditorGUI.EndDisabledGroup();

			ICEEditorLayout.EndHorizontal( Info.IMPULSE_TIMER_TIME );

			if( _timer.UseInterval )
			{

				ICEEditorLayout.MinMaxRandomDefaultSlider( "Impulse Interval (secs.)", "", ref _timer.ImpulseIntervalMin, ref _timer.ImpulseIntervalMax, 0, ref _timer.ImpulseIntervalMaximum,0,0, Init.DECIMAL_PRECISION_TIMER, 30, Info.IMPULSE_TIMER_INTERVAL );

				EditorGUI.indentLevel++;

				ICEEditorLayout.MinMaxRandomDefaultSlider( "Impulse Limit", "", ref _timer.ImpulseLimitMin, ref _timer.ImpulseLimitMax, 0, ref _timer.ImpulseLimitMaximum,0,0,30, Info.IMPULSE_TIMER_LIMITS );

				if( Mathf.Max( _timer.ImpulseIntervalMin, _timer.ImpulseIntervalMax ) > 0 )
				{
					ICEEditorLayout.MinMaxRandomDefaultSlider( "Sequence Limit", "", ref _timer.ImpulseSequenceLimitMin, ref _timer.ImpulseSequenceLimitMax, 0, ref _timer.ImpulseSequenceLimitMaximum,0,0,30, Info.IMPULSE_TIMER_SEQUENCE_LIMITS );

					EditorGUI.BeginDisabledGroup( _timer.ImpulseSequenceLimitMax == 0 );
						ICEEditorLayout.MinMaxRandomDefaultSlider( "Break Length (secs.)", "", ref _timer.ImpulseSequenceBreakLengthMin, ref _timer.ImpulseSequenceBreakLengthMax, 0, ref _timer.ImpulseSequenceBreakLengthMaximum,2,5, Init.DECIMAL_PRECISION_TIMER, 30, Info.IMPULSE_TIMER_SEQUENCE_BREAK_LENGTH );
					EditorGUI.EndDisabledGroup();

				}

				EditorGUI.indentLevel--;
			}

			if( _type != EditorHeaderType.NONE )
			{
				EndObjectContent();
				// CONTENT END
			}
		}

		public static void DrawUnderwaterCameraEffect( ICEWorldBehaviour _component, UnderwaterCameraEffect _underwater, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _underwater == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Underwater Effect";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			DrawObjectHeader( _underwater, _type, _title, _hint );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _underwater ) )
				return;

			_underwater.UseWaterZone = ICEEditorLayout.Toggle( "Use Water Zone", "", _underwater.UseWaterZone ); 

			_underwater.WaterLevel = ICEEditorLayout.MaxDefaultSlider( "Water Level", "", _underwater.WaterLevel, 0.25f, - _underwater.WaterLevelMaximum, ref _underwater.WaterLevelMaximum, 0, "" );

			_underwater.FogEnabled = ICEEditorLayout.Toggle( "Fog Enabled", "", _underwater.FogEnabled ); 

			_underwater.FogColor = ICEEditorLayout.ColorField( "Fog Color", "", _underwater.FogColor, "" ); 

			_underwater.FogDensity = ICEEditorLayout.DefaultSlider( "Fog Density", "", _underwater.FogDensity, 0.001f, 0, 1, 0.04f, "" );

			_underwater.UnderwaterBackgroundColor = ICEEditorLayout.ColorField( "Background Color", "", _underwater.UnderwaterBackgroundColor, "" ); 

			EndObjectContent();
			// CONTENT END
		}

		public static bool DrawBehaviourEventObject( ICEWorldBehaviour _component, BehaviourEventsObject _events, BehaviourEventObject _event, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _event == null || _events == null )
				return false;

			if( string.IsNullOrEmpty( _title ) )
				_title = ( string.IsNullOrEmpty( _event.Event.FunctionName ) ? "Event":_event.Event.FunctionName );
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			ICEEditorLayout.BeginHorizontal();

				if( IsEnabledType( _type ) )
					EditorGUI.BeginDisabledGroup( _event.Enabled == false );
		
				DrawObjectHeaderLine( _event, GetSimpleFoldout( _type ), _title, _hint );

				if( ICEEditorLayout.CopyButtonSmall( "" ) )
					_events.Events.Add( new BehaviourEventObject( _event ) );

				if( IsEnabledType( _type ) )
					EditorGUI.EndDisabledGroup();

				if( ICEEditorLayout.ListDeleteButton<BehaviourEventObject>( _events.Events, _event ) )
					return true;
			
				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<BehaviourEventObject>( _events.Events, _events.Events.IndexOf( _event ) ) )
					return true;

				DrawEnabledButton( _event, _type );	

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _event ) )
				return false;

				DrawBehaviourEvent( _component, _event.Event, "", "" );
				//EditorGUILayout.Separator();
				DrawImpulsTimerObject( _event );

			EndObjectContent();
			// CONTENT END

			return false;
		}
			

		/// <summary>
		/// Draws the message object.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_message">Message.</param>
		/// <param name="_left">If set to <c>true</c> left.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawEventsObject( ICEWorldBehaviour _component, BehaviourEventsObject _events, EditorHeaderType _type, EditorHeaderType _child_type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _events == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Events";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENTS;

			if( _events.Events.Count > 0 )
				_title += " (" + _events.Events.Count + ")";

			ICEEditorLayout.BeginHorizontal();

				if( IsEnabledType( _type ) )
					EditorGUI.BeginDisabledGroup( _events.Enabled == false );

				DrawObjectHeaderLine( _events, GetSimpleFoldout( _type ), _title, _hint );

				DrawAddButton<BehaviourEventObject>( _events, _type, _events.Events );	

				if( IsEnabledType( _type ) )
					EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _events.Events.Count == 0 );
					DrawClearButton( _events, _events.Events );	
				EditorGUI.EndDisabledGroup();

				DrawEnabledButton<BehaviourEventObject>( _events, _type, _events.Events );	

			ICEEditorLayout.EndHorizontal( _help );

			if( _events.Events.Count == 0 )
				_events.Foldout = false;

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _events ) )
				return;

				for( int i = 0 ; i < _events.Events.Count ; i++ )
					if( DrawBehaviourEventObject( _component, _events, _events.Events[i], _child_type ) )
						return;


			EndObjectContent();
			// CONTENT END
		}



		/// <summary>
		/// Draws the method data object.
		/// </summary>
		/// <param name="_component">Component.</param>
		/// <param name="_method">Method.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		public static void DrawBehaviourEvent( ICEWorldBehaviour _component, BehaviourEvent _event, string _help = "", string _title = "", string _hint = "" )
		{
			if( _event == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Method Name";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.EVENT;

			if( _component == null )
			{
				EditorGUI.BeginDisabledGroup( _component == null );
					WorldPopups.EventPopup( null, _event.Info, new BehaviourEventInfo[0], ref _event.UseCustomFunction, Info.EVENT_POPUP,  _title, _hint );
				EditorGUI.EndDisabledGroup();
			}
			else
				_event.Info = WorldPopups.EventPopup( _component, _event.Info, _component.BehaviourEventsInChildren, ref _event.UseCustomFunction, Info.EVENT_POPUP,  _title, _hint );

			EditorGUI.indentLevel++;
			if( _event.ParameterType == BehaviourEventParameterType.Boolean )
				_event.ParameterBoolean = ICEEditorLayout.Toggle( _event.ParameterTitle, _event.ParameterDescription, _event.ParameterBoolean, Info.EVENT_PARAMETER_BOOLEAN );
			else if( _event.ParameterType == BehaviourEventParameterType.Integer )
				_event.ParameterInteger = ICEEditorLayout.Integer( _event.ParameterTitle, _event.ParameterDescription, _event.ParameterInteger, Info.EVENT_PARAMETER_INTEGER );
			else if( _event.ParameterType == BehaviourEventParameterType.Float )
				_event.ParameterFloat = ICEEditorLayout.Float( _event.ParameterTitle, _event.ParameterDescription, _event.ParameterFloat, Info.EVENT_PARAMETER_FLOAT );
			else if( _event.ParameterType == BehaviourEventParameterType.String )
				_event.ParameterString = ICEEditorLayout.Text( _event.ParameterTitle, _event.ParameterDescription, _event.ParameterString, Info.EVENT_PARAMETER_STRING );
			/*else if( _event.ParameterType == BehaviourEventParameterType.Object )
			{
				ICEEditorLayout.BeginHorizontal();
					//if( _event.UseCustomParameterObject )
					//_event.ParameterObject = (GameObject)EditorGUILayout.ObjectField( "Parameter Object", (GameObject)_event.ParameterObject, typeof(GameObject), true );
					//else
					
					_event.UseCustomParameterObject = ICEEditorLayout.ButtonCheck( "CUSTOM", "", _event.UseCustomParameterObject, ICEEditorStyle.ButtonMiddle ); 
				ICEEditorLayout.EndHorizontal( Info.EVENT_PARAMETER_OBJECT );
			}*/
			EditorGUI.indentLevel--;
		}

		/// <summary>
		/// Draws the mouse look object.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		public static void DrawMouseLookObject( ICEMouseLook _object, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _object == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Mouse Look";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "HINT";
			if( string.IsNullOrEmpty( _help ) )
				_help = "";//Info.MOUSELOOK;

			if( ! IsEnabledFoldoutType( _type ) )
				_object.Enabled = true;

			DrawObjectHeader( _object, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _object ) )
				return;

			_object.SensitivityX = ICEEditorLayout.DefaultSlider( "X Sensitivity", "", _object.SensitivityX, 0.025f, 0, 100, 2, "" );
			_object.SensitivityY = ICEEditorLayout.DefaultSlider( "Y Sensitivity", "", _object.SensitivityY, 0.025f, 0, 100, 2, "" );
			_object.ClampVerticalRotation = ICEEditorLayout.Toggle( "Clamp Vertical Rotation", "", _object.ClampVerticalRotation , "" );

			ICEEditorLayout.MinMaxSlider( "X (min/max)", "", ref _object.MinimumX, ref _object.MaximumX, -90, 90, 1, 40, "" );

			_object.UseSmoothRotation = ICEEditorLayout.Toggle( "Smooth", "", _object.UseSmoothRotation , "" );
			EditorGUI.indentLevel++;
				_object.SmoothRotationSpeed = ICEEditorLayout.MaxDefaultSlider( "Smooth Speed", "", _object.SmoothRotationSpeed, 0.025f, 0, ref _object.SmoothRotationSpeedMaximum, 5, "" );
			EditorGUI.indentLevel--;
			_object.LockCursor = ICEEditorLayout.Toggle( "Lock Cursor", "", _object.LockCursor , "" );

			EndObjectContent();
			// CONTENT END
		}

	}
}
