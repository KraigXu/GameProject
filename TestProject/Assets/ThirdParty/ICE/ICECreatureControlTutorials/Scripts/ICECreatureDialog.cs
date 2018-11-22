using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using ICE.Creatures;
using ICE.Creatures.Objects;

public class ICECreatureDialog : MonoBehaviour {

	private ICE.Creatures.ICECreatureControl m_Creature = null;
	private ICE.Creatures.ICECreatureControl m_AttachedCreature{
		get{ return m_Creature = ( m_Creature == null ? this.gameObject.GetComponent<ICE.Creatures.ICECreatureControl>() : m_Creature ); }
	}

	public Text m_DialogText = null;
	public List<string> m_DialogBehaviourKeys = new List<string>();
	public List<string> m_DialogBehaviourPhrases = new List<string>();

	private Dictionary<string, string> m_Dialog = new Dictionary<string, string>();

	void Start () 
	{
		for( int i = 0 ; i < m_DialogBehaviourKeys.Count ; i++ )
		{
			if( i < m_DialogBehaviourPhrases.Count )
				m_Dialog.Add( m_DialogBehaviourKeys[i], m_DialogBehaviourPhrases[i] );
		}
	}

	private void OnEnable()
	{
		if( m_AttachedCreature != null )
			m_AttachedCreature.OnBehaviourModeChanged += OnBehaviourModeChanged;
	}

	private void OnDisable()
	{
		if( m_AttachedCreature != null )
			m_AttachedCreature.OnBehaviourModeChanged -= OnBehaviourModeChanged;
	}

	void OnBehaviourModeChanged( GameObject _sender, BehaviourModeObject _new_mode, BehaviourModeObject _last_mode )
	{
		if( _new_mode == null )
			return;

		string _text = "";
		if( m_Dialog.TryGetValue( _new_mode.Key , out _text ) )
			m_DialogText.text = _text;	
	}
}
