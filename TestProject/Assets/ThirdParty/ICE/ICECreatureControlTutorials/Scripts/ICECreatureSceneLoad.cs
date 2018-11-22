using UnityEngine;
using UnityEngine.UI;

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif

using System.Collections;
using System.Collections.Generic;

using ICE.Creatures;
using ICE.Creatures.Objects;

public class ICECreatureSceneLoad : MonoBehaviour {

	private ICE.Creatures.ICECreatureRegister m_Register = null;
	private ICE.Creatures.ICECreatureRegister m_AttachedRegister{
		get{ return m_Register = ( m_Register == null ? this.gameObject.GetComponent<ICE.Creatures.ICECreatureRegister>() : m_Register ); }
	}

	void Start () 
	{

	}

	public void OnEnable()
	{
#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnLevelLoad;
#endif
	}

	public void OnDisable()
	{
#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded -= OnLevelLoad;
#endif
	}

#if UNITY_5_4_OR_NEWER
	protected virtual void OnLevelLoad( Scene scene, LoadSceneMode mode )
#else
	protected virtual void OnLevelWasLoaded()
#endif
	{
	}

}
