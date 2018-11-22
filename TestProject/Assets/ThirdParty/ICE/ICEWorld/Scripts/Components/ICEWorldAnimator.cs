using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.Objects
{
	public class ICEWorldAnimator : ICEWorldBehaviour {

		private Animator m_Animator = null;
		public Animator AnimatorComponent{
			get{ return m_Animator = ( m_Animator == null ? GetComponent<Animator>() : m_Animator ); }
		}

		public AxisInputData AxisInput;
		public string Parameter = "";
		public bool HandleColliders = true;


		public override void Update () 
		{
			if( AnimatorComponent != null && AnimatorComponent.isInitialized )
			{
				if( Input.GetAxis( AxisInput.Name ) > 0 )
				{
					if( HandleColliders )
						SystemTools.EnableColliders( transform, true );
					
					AnimatorComponent.SetBool( Parameter, true );
				}
				else
				{
					if( HandleColliders )
						SystemTools.EnableColliders( transform, false );
					
					AnimatorComponent.SetBool( Parameter, false );
				}
			}

			base.Update();
		}
	}
}
