// ##############################################################################
//
// ice_objects.cs | ICE.World.Objects.ICEObject | ICEDataObject | ICEInfoDataObject | ICEOwnerObject
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

using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using ICE.World.EnumTypes;
using ICE.World.Utilities;

namespace ICE.World.Objects
{
	[System.Serializable]
	public struct AxisInputData
	{
		public AxisInputType Type;
		public string Name;
		public int Value;

		public void Copy( AxisInputData _data )
		{
			Type = _data.Type;
			Name = _data.Name;
			Value = _data.Value;
		}
	}

	public static class ICEObjectFactory
	{
		public static T Create<T>() where T : new(){

			T _object = new T();

			// if the created object is derivated from ICEDataObject we enable and foldout it
			ICEDataObject _data = _object as ICEDataObject;
			if( _data != null )
			{
				_data.Enabled = true;
				_data.Foldout = true;
			}

			return _object;
		}
	}

	[System.Serializable]
	public abstract class ICEObject : System.Object 
	{
		public virtual void Reset(){}
	}

	[System.Serializable]
	public abstract class ICEScriptableObject : ScriptableObject 
	{
		public ICEScriptableObject(){}
	}

	[System.Serializable]
	public abstract class ICEDataObject : ICEObject 
	{
		public ICEDataObject(){}
		public ICEDataObject( ICEDataObject _object )
		{
			Copy( _object );
		}

		public void Copy( ICEDataObject _object )
		{
			if( _object == null )
				return;
			
			Enabled = _object.Enabled;
			Foldout = _object.Foldout;
		}
			
		/// <summary>
		/// Enables or disables the use of the object.
		/// </summary>
		public bool Enabled = false; 

		/// <summary>
		/// The foldout parameter is a display option and should be used in the editor only 
		/// </summary>
		public bool Foldout = false;


		/// <summary>
		/// IsMultiplayer indicates if the game is running in MP mode
		/// </summary>
		/// <value><c>true</c> if is multiplayer; otherwise, <c>false</c>.</value>
		public static bool IsMultiplayer{
			get{ return ICEWorldInfo.IsMultiplayer; }
		}

		/// <summary>
		/// IsMasterClient indicates whenever we are the master or just a slave. If IsMultiplayer 
		/// is false, IsMasterClient will be always true.
		/// </summary>
		/// <value><c>true</c> if is master client; otherwise, <c>false</c>.</value>
		public static bool IsMasterClient{
			get{ return ICEWorldInfo.IsServer; }
		}

		/// <summary>
		/// IsRemoteClient indicates whenever we are a remote-controlled client, in such a case 
		/// our activities are extremly limited and will be controlled by the master. If IsMultiplayer 
		/// is false, IsRemoteClient will be always true.
		/// </summary>
		/// <value><c>true</c> if is remote client; otherwise, <c>false</c>.</value>
		public static bool IsRemoteClient{
			get{ return ( ICEWorldInfo.IsMultiplayer && ! ICEWorldInfo.IsServer ? true : false ); }
		}

		/// <summary>
		/// NetworkConnectedAndReady indicats whether we are connected and joined to a room. That's 
		/// alone might be too less information to starts spawning processes, because we have here no 
		/// further information about the scene loading or additional RPCs which or something like 
		/// this, so we have to wait on NetworkSpawnerReady
		/// </summary>
		/// <value><c>true</c> if network connected and ready; otherwise, <c>false</c>.</value>
		public static bool NetworkConnectedAndReady{
			get{ return ICEWorldInfo.NetworkConnectedAndReady; }
		}

		/// <summary>
		/// NetworkSpawnerReady will be used by the NetworkSpawner to inform us whenever we 
		/// could spawn someting or not. As remote client we will spawn our own player only 
		/// all the rest will be handled bei the master server.
		/// </summary>
		/// <value><c>true</c> if network spawner ready; otherwise, <c>false</c>.</value>
		public static bool NetworkSpawnerReady{
			get{ return ICEWorldInfo.NetworkSpawnerReady; }
		}
	}

	[System.Serializable]
	public abstract class ICEInfoDataObject : ICEDataObject 
	{
		public ICEInfoDataObject(){}
		public ICEInfoDataObject( ICEInfoDataObject _object ) : base( _object ) { Copy( _object ); }

		public virtual void Copy( ICEInfoDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			InfoText = _object.InfoText;
			ShowInfoText = _object.ShowInfoText;
		}

		/// <summary>
		/// The info text.
		/// </summary>
		public string InfoText = "";

		/// <summary>
		/// The info text enabled.
		/// </summary>
		public bool ShowInfoText = false;

	}

	[System.Serializable]
	public abstract class ICEEntityObject : ICEInfoDataObject 
	{
		public ICEEntityObject(){}
		public ICEEntityObject( ICEEntityObject _object ) : base( _object ) { Copy( _object ); }

		public override void Copy( ICEInfoDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}

		[SerializeField, XmlIgnore]
		protected GameObject m_EntityGameObject = null;
		[XmlIgnore]
		public GameObject EntityGameObject{
			get{ return m_EntityGameObject = ( m_EntityGameObject == null ?( m_EntityComponent != null ? m_EntityComponent.gameObject:null ):m_EntityGameObject ); }
		}
			
		/// <summary>
		/// Sets the entity game object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public bool SetEntityGameObject( GameObject _object )
		{			
			bool _entity_changed = ( m_EntityGameObject != _object ? true : false );
				
			m_EntityGameObject = _object;
			m_EntityComponent = ( m_EntityGameObject != null ? m_EntityGameObject.GetComponent<ICEWorldEntity>():null );

			if( _entity_changed )
				DoEntityGameObjectChanged();

			return _entity_changed;
		}

		protected virtual void DoEntityGameObjectChanged(){} 

	
		/// <summary>
		/// Compares the GameObject.
		/// </summary>
		/// <returns><c>true</c>, if GameObject was compared, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/*
		public bool CompareGameObject( GameObject _object ){

			bool _test_1 = System.Object.ReferenceEquals( m_EntityGameObject, _object );
			bool _test_2 = ObjectTools.CompareGameObject( m_EntityGameObject, _object );
			if( _test_1 != _test_2 )
				Debug.Log( System.Object.ReferenceEquals( m_EntityGameObject, _object ).ToString() + " - " + ObjectTools.CompareGameObject( m_EntityGameObject, _object ).ToString() + " : " + 
					( m_EntityGameObject != null ? m_EntityGameObject.GetInstanceID().ToString() : "null" ) + " - " +
					( _object != null ? _object.GetInstanceID().ToString() : "null" ) );
			
			return ObjectTools.CompareGameObject( m_EntityGameObject, _object );
		}   */

		[SerializeField, XmlIgnore]
		protected ICEWorldEntity m_EntityComponent = null;
		public virtual ICEWorldEntity EntityComponent{
			get{ return m_EntityComponent = ( m_EntityComponent == null && m_EntityGameObject != null ? m_EntityGameObject.GetComponent<ICEWorldEntity>():m_EntityComponent ); }
		}

		public virtual void Init( ICEWorldEntity _component ){
			m_EntityComponent = _component;
			m_EntityGameObject = ( m_EntityComponent != null ? m_EntityComponent.gameObject:null );
		}

		public string Name{
			get{ return ( EntityGameObject != null ? EntityGameObject.name:"" ); }
		}

		public string Tag{
			get{ return ( EntityGameObject != null ? EntityGameObject.tag:"" ); }
		}

		public int ID{
			get{ return ( EntityGameObject != null ? EntityGameObject.GetInstanceID():0 ); }
		}

		public EntityClassType EntityType{
			get{ return ( EntityComponent != null ? EntityComponent.EntityType : EntityClassType.Undefined ); }
		}
			
	}


	/// <summary>
	/// ICEObject represents the abstract base class for all ICE related System Objects.
	/// </summary>
	[System.Serializable]
	public abstract class ICEOwnerObject : ICEInfoDataObject {

		/// <summary>
		/// Prints debug log.
		/// </summary>
		public bool EnableDebugLog = false;

		/// <summary>
		/// The enable debug ray.
		/// </summary>
		public bool EnableDebugRay = false;

		public void PrintErrorLog( ICEOwnerObject _object, string _log )
		{
#if UNITY_EDITOR
				ICEDebug.LogError( OwnerName + " (" + OwnerInstanceID + ") - " + ( _object != null?_object.GetType().ToString() + " ":"" ) + _log );				
#endif
		}

		/// <summary>
		/// Prints the debug log.
		/// </summary>
		/// <param name="_log">Log.</param>
		public void PrintDebugLog( ICEOwnerObject _object, string _log )
		{
#if UNITY_EDITOR
			if( DebugLogIsEnabled )
				ICEDebug.Log( OwnerName + " (" + OwnerInstanceID + ") - " + ( _object != null?_object.GetType().ToString() + " ":"" ) + _log );
#endif
		}

		/// <summary>
		/// Draws debug rays.
		/// </summary>
		/// <param name="_origin">Origin.</param>
		/// <param name="_direction">Direction.</param>
		/// <param name="_color">Color.</param>
		public void DebugRay( Vector3 _origin, Vector3 _direction, Color _color )
		{
#if UNITY_EDITOR
			if( DebugRayIsEnabled )
				ICEDebug.DrawRay( _origin, _direction, _color);
#endif
		}

		/// <summary>
		/// Draws debug line.
		/// </summary>
		/// <param name="_from">From.</param>
		/// <param name="_to">To.</param>
		/// <param name="_color">Color.</param>
		public void DebugLine( Vector3 _from, Vector3 _to, Color _color )
		{
#if UNITY_EDITOR
			if( DebugRayIsEnabled )
				ICEDebug.DrawLine( _from, _to, _color);
#endif
		}

		public bool DebugLogIsEnabled
		{
			get{ 
#if UNITY_EDITOR
				return ( ( EnableDebugLog || OwnerEnabledDebugLog ) && ( ! OwnerDebugLogsSelectedOnly || UnityEditor.Selection.activeGameObject == Owner ) ? true : false );
#else
				return false;
#endif
			}
		}
		public bool DebugRayIsEnabled
		{
			get{ 
#if UNITY_EDITOR
			return ( ( EnableDebugRay || OwnerEnabledDebugRay ) && ( ! OwnerDebugRaysSelectedOnly || UnityEditor.Selection.activeGameObject == Owner ) ? true : false );
#else
			return false;
#endif
			}
		}

		/// <summary>
		/// Owner represents the owning GameObject
		/// </summary>
		[XmlIgnore]
		private GameObject m_Owner = null;

		/// <summary>
		/// Gets the owning GameObject.
		/// </summary>
		/// <value>The parent or null</value>
		[XmlIgnore]
		public GameObject Owner{
			get{ return m_Owner = ( m_Owner == null ? ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null ) : m_Owner ); }
		}

		/// <summary>
		/// The m parent represents the owner component
		/// </summary>
		[XmlIgnore]
		private ICEWorldBehaviour m_OwnerComponent = null;
		/// <summary>
		/// Gets the owner component.
		/// </summary>
		/// <value>The parent or null</value>
		[XmlIgnore]
		public ICEWorldBehaviour OwnerComponent{
			get{ return m_OwnerComponent; }
		}

		/// <summary>
		/// Gets the name of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string OwnerName{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.name:"" ); }
		}

		/// <summary>
		/// Gets the tag of the parent.
		/// </summary>
		/// <value>The name of the parent.</value>
		public string OwnerTag{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.tag:"" ); }
		}

		/// <summary>
		/// Gets the parent InstanceID.
		/// </summary>
		/// <value>The parent InstanceID or 0</value>
		public int OwnerInstanceID{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.ObjectInstanceID :0 ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEObject"/> parent allows to print the debug log.
		/// </summary>
		/// <value><c>true</c> if parent print debug log; otherwise, <c>false</c>.</value>
		public bool OwnerEnabledDebugLog{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugLogs:false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEOwnerObject"/> owner debug logs selected only.
		/// </summary>
		/// <value><c>true</c> if owner debug logs selected only; otherwise, <c>false</c>.</value>
		public bool OwnerDebugLogsSelectedOnly{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugLogsSelectedOnly:false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEOwnerObject"/> owner enabled debug ray.
		/// </summary>
		/// <value><c>true</c> if owner enabled debug ray; otherwise, <c>false</c>.</value>
		public bool OwnerEnabledDebugRay{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugRays:false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.World.Objects.ICEOwnerObject"/> owner debug rays selected only.
		/// </summary>
		/// <value><c>true</c> if owner debug rays selected only; otherwise, <c>false</c>.</value>
		public bool OwnerDebugRaysSelectedOnly{
			get{ return ( m_OwnerComponent != null ? m_OwnerComponent.UseDebugRaysSelectedOnly :false ); }
		}
			
		public ICEOwnerObject(){}
		public ICEOwnerObject( ICEWorldBehaviour _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}
		public ICEOwnerObject( ICEOwnerObject _object ) : base( _object ){
			m_OwnerComponent = ( _object != null?_object.OwnerComponent:null );
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );

			EnableDebugLog = _object.EnableDebugLog;
		}

		/// <summary>
		/// Default Init method to initiate the object.
		/// </summary>
		/// <param name="_parent">Parent.</param>
		public virtual void Init( ICEWorldBehaviour _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}

		public virtual bool OwnerIsReady( ICEWorldBehaviour _component ){

			if( _component != null && _component != m_OwnerComponent )
			{
				m_OwnerComponent = _component;
				m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
			}

			return ( m_OwnerComponent != null || m_Owner != null ? true : false ); 
		}
			
		public void SetOwner( GameObject _object )
		{
			m_Owner = _object;
			m_OwnerComponent = ( _object != null ? _object.GetComponent<ICEWorldBehaviour>():null );
		}

		public void TrySetOwner( GameObject _object )
		{
			if( _object == m_Owner )
				return;

			m_Owner = _object;
			m_OwnerComponent = ( _object != null ? _object.GetComponent<ICEWorldBehaviour>():null );
		}
	}
}

namespace ICE.World
{
	public sealed class ICEDebug{


		public static void Log( string _msg ){
			Debug.Log( "<color=gray>Info: " + _msg + "</color>"  );
		}
			
		public static void LogInfo( string _msg ){
			Debug.Log( "<color=cyan>Info: " + _msg + "</color>" );
		}

		public static void LogInfo( string _msg, bool _display ){
			if( _display ) Debug.Log( "<color=cyan>Info: " + _msg + "</color>" );
		}

		public static void LogAction( string _msg ){
			Debug.Log( "<color=white>Info: " + _msg + "</color>" );
		}

		public static void LogAction( string _msg, bool _display ){
			if( _display ) Debug.Log( "<color=white>Info: " + _msg + "</color>" );
		}

		public static void LogStatus( string _msg ){
			Debug.Log( "<color=magenta>Info: " + _msg + "</color>" );
		}

		public static void LogStatus( string _msg, bool _display ){
			if( _display ) Debug.Log( "<color=magenta>Info: " + _msg + "</color>" );
		}

		public static void LogTip( string _msg ){
			Debug.Log( "<color=green>Tip: " + _msg + "</color>" );
		}

		public static void LogWarning( string _msg ){
			Debug.LogWarning( "<color=yellow>Warning: " + _msg + "</color>" );
		}

		public static void LogError( string _msg ){
			Debug.LogError( "<color=red>Error: " + _msg + "</color>" );
		}

		public static void DrawRay( Vector3 _origin, Vector3 _direction ){
			Debug.DrawRay( _origin, _direction );
		}

		public static void DrawRay( Vector3 _origin, Vector3 _direction, Color _color ){
			Debug.DrawRay( _origin, _direction, _color );
		}

		public static void DrawRay( Vector3 _origin, Vector3 _direction, Color _color, float _duration ){
			Debug.DrawRay( _origin, _direction, _color, _duration );
		}

		public static void DrawRay( Vector3 _origin, Vector3 _direction, Color _color, float _duration, bool _death_test ){
			Debug.DrawRay( _origin, _direction, _color, _duration, _death_test );
		}

		public static void DrawLine( Vector3 _from, Vector3 _to ){
			Debug.DrawLine( _from, _to );
		}

		public static void DrawLine( Vector3 _from, Vector3 _to, Color _color ){
			Debug.DrawLine( _from, _to, _color);
		}

		public static void DrawLine( Vector3 _from, Vector3 _to, Color _color, float _duration ){
			Debug.DrawLine( _from, _to, _color, _duration );
		}

		public static void DrawLine( Vector3 _from, Vector3 _to, Color _color, float _duration, bool _death_test ){
			Debug.DrawLine( _from, _to, _color, _duration, _death_test );
		}

	}

}
