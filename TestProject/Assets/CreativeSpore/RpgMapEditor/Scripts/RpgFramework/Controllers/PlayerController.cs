using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Controllers/PlayerController", 10)]
	public class PlayerController : CharBasicController {

		public GameObject BulletPrefab;
		public float TimerBlockDirSet = 0.6f;
		public Camera2DController Camera2D;
		public float BulletAngDispersion = 15f;
        public SpriteRenderer ShadowSprite;
        public SpriteRenderer WeaponSprite;
        public int FogSightLength = 5;

        /// <summary>
        /// If player is driving a vehicle, this will be that vehicle
        /// </summary>
        public VehicleCharController Vehicle;        

		private FollowObjectBehaviour m_camera2DFollowBehaviour;

        #region Singleton and Persistence
        static PlayerController s_instance;
        void Awake()
        {
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
            if(s_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                s_instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }      
        }
        #endregion

        public void UndoDontDestroyOnLoad()
        {
            s_instance = null;
        }

        void OnDestroy()
        {
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        }

#if UNITY_5_4 || UNITY_5_5_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (mode != UnityEngine.SceneManagement.LoadSceneMode.Single) return;
#else
        void OnLevelWasLoaded()
        {
#endif
            if (s_instance != this) // this happens if UndoDontDestroyOnLoad was called
            {
                DestroyImmediate(gameObject);
            }
        }

        public override void SetVisible(bool value)
        {
            base.SetVisible(value);
            ShadowSprite.enabled = value;
            WeaponSprite.enabled = value;
        }

		protected override void Start () 
		{
            base.Start();
			if( Camera2D == null )
			{
				Camera2D = GameObject.FindObjectOfType<Camera2DController>();
			}
			
			m_camera2DFollowBehaviour = Camera2D.transform.GetComponent<FollowObjectBehaviour>();
			m_camera2DFollowBehaviour.Target = transform;
		}

		void CreateBullet( Vector3 vPos, Vector3 vDir )
		{
			GameFactory.CreateBullet( gameObject, BulletPrefab, vPos, vDir, 4f );
		}

        private float m_keepAttackDirTimer = 0f;
		void DoInputs()
		{			
			Vector3 vBulletDir = Vector3.zero;
			Vector3 vBulletPos = Vector3.zero;
            float keepAttackDirTimerValue = 0.5f;
			if( Input.GetKeyDown( "j" ) ) //down
			{
				vBulletPos = new Vector3( -0.08f, -0.02f, 0f );
				vBulletPos += transform.position;
				vBulletDir = Vector3.down;
				m_animCtrl.AnimDirection = eAnimDir.Down;
				m_timerBlockDir = TimerBlockDirSet;
                m_keepAttackDirTimer = keepAttackDirTimerValue;
			}
			else if( Input.GetKeyDown( "h" ) ) // left
			{
				vBulletPos = new Vector3( -0.10f, 0.10f, 0f );
				vBulletPos += transform.position;
				vBulletDir = -Vector3.right;
                m_animCtrl.AnimDirection = eAnimDir.Left;
				m_timerBlockDir = TimerBlockDirSet;
                m_keepAttackDirTimer = keepAttackDirTimerValue;
			}
			else if( Input.GetKeyDown( "k" ) ) // right
			{
				vBulletPos = new Vector3( 0.10f, 0.10f, 0f );
				vBulletPos += transform.position;
				vBulletDir = Vector3.right;
                m_animCtrl.AnimDirection = eAnimDir.Right;
				m_timerBlockDir = TimerBlockDirSet;
                m_keepAttackDirTimer = keepAttackDirTimerValue;
			}
			else if( Input.GetKeyDown( "u" ) ) // up
			{
				vBulletPos = new Vector3( 0.08f, 0.32f, 0f );
				vBulletPos += transform.position;
				vBulletDir = -Vector3.down;
                m_animCtrl.AnimDirection = eAnimDir.Up;
				m_timerBlockDir = TimerBlockDirSet;
                m_keepAttackDirTimer = keepAttackDirTimerValue;
			}

			if( vBulletDir != Vector3.zero )
			{
				float fRand = Random.Range(-1f, 1f);
				fRand = Mathf.Pow(fRand, 5f);
				vBulletDir = Quaternion.AngleAxis(BulletAngDispersion*fRand, Vector3.forward) * vBulletDir;
				CreateBullet( vBulletPos, vBulletDir);
			}
		}

        private int m_lastTileIdx = -1;
        private int m_lastFogSightLength = 0;

        protected override void Update()
		{
            eAnimDir savedAnimDir = m_animCtrl.AnimDirection;
            base.Update();
            if(m_keepAttackDirTimer > 0f)
            {
                m_keepAttackDirTimer -= Time.deltaTime;
                m_animCtrl.AnimDirection = savedAnimDir;
            }
            m_phyChar.enabled = (Vehicle == null);
            if (Vehicle != null)
            {
                m_animCtrl.IsPlaying = false;
            }
            else
            {
                DoInputs();

                bool isMoving = (m_phyChar.Dir.sqrMagnitude >= 0.01);
                if (isMoving)
                {
                    //m_phyChar.Dir.Normalize();
                    m_camera2DFollowBehaviour.Target = transform;
                }
                else
                {
                    m_phyChar.Dir = Vector3.zero;
                }
            }

            int tileIdx = RpgMapHelper.GetTileIdxByPosition(transform.position);

            if (tileIdx != m_lastTileIdx || m_lastFogSightLength != FogSightLength)
            {
                RpgMapHelper.RemoveFogOfWarWithFade(transform.position, FogSightLength);
            }

            m_lastFogSightLength = FogSightLength;
            m_lastTileIdx = tileIdx;
		}             
	}
}