using System;
using System.Collections.Generic;

using DG.Tweening;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public delegate void OnLoadingOverCallback();

    [Serializable]
    public class SceneConfig
    {
        public string SceneName;
        public int CustomDuration;
    }
    public SceneConfig[] SceneConfigs;
    public int SceneSwitchInterval = 5;
    public float TimeUntilNextSwitch = 0.0f;
    public int CurrentSceneIndex = 0;
    public bool EntitiesDestroyed = false;


    public OnLoadingOverCallback Callback;
    public string SceneNameNext;
    public float value;

    [SerializeField]
    private RawImage _mask;
    private GameObject _icon;
    private Sequence _sequence;
    private Slider _slider;
    private Button _btnUp;
    private Button _btnDown;

    private Text _tipsTitle;
    private Text _tipsContent;
    private List<TipsData> _tipsDatas;

    private int _currentPage = 0;
    private int _maxPage = 0;

    private float loadingSpeed = 1;
    private float targetValue;

    private AsyncOperation _asyncOperation;
    private bool _isLoad = false;
    private bool _isLoadDone = false;
    private float _intervalView = 0;
    private float _intervalLoad = 0;


    void Awake()
    {
        DontDestroyOnLoad(this);

        this._icon = transform.Find("Icon").gameObject;
        _sequence = DOTween.Sequence();
        _sequence.Append(_icon.transform.DORotate(new Vector3(0, 0, 180), 2));
        _sequence.Append(_icon.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 1));
        _sequence.Append(_icon.transform.DORotate(new Vector3(0, 0, 360), 2));
        _sequence.Append(_icon.transform.DOScale(new Vector3(1f, 1f, 1f), 1));
        _sequence.SetLoops(-1);

        _slider = transform.Find("Slider").GetComponent<Slider>();
        _tipsTitle = transform.Find("TipsTitle").GetComponent<Text>();
        _tipsContent = transform.Find("TipsContent").GetComponent<Text>();
        _btnUp = transform.Find("TipsUp").GetComponent<Button>();
        _btnDown = transform.Find("TipsDown").GetComponent<Button>();

        _btnUp.onClick.AddListener(ButtonUp);
        _btnDown.onClick.AddListener(ButtonDown);

        //取值
        _tipsDatas = SQLService.Instance.SimpleQuery<TipsData>(" TipsType=? ", new object[] { "1" });
        _maxPage = _tipsDatas.Count;
        _tipsTitle.text = _currentPage + "/" + _maxPage;

        _tipsContent.text = _tipsDatas[_currentPage].Content;
        _maxPage = _tipsDatas.Count;
        _tipsTitle.text = _tipsDatas[_currentPage].ContentTitle;
        _tipsContent.text = _tipsDatas[_currentPage].Content;

    }

    void Start()
    {
        //淡入
        _mask.DOFade(1, 1);
        _slider.enabled = false;
        _tipsTitle.enabled = false;
        _tipsContent.enabled = false;
        _btnUp.enabled = false;
        _btnDown.enabled = false;

    }
    private void DestroyAllEntitiesInScene()
    {
        var entities = SystemManager.ActiveManager.GetAllEntities();
        SystemManager.ActiveManager.DestroyEntity(entities);
        entities.Dispose();
        EntitiesDestroyed = true;
    }

    private void LoadNextScene()
    {
        //var sceneCount = SceneManager.sceneCountInBuildSettings;
        //var nextIndex = CurrentSceneIndex + 1;
        //if (nextIndex >= sceneCount)
        //{
        //    Quit();
        //    return;
        //}

        //var nextScene = SceneUtility.GetScenePathByBuildIndex(nextIndex);
        //TimeUntilNextSwitch = GetSceneDuration(nextScene);
        //CurrentSceneIndex = nextIndex;

        //SceneManager.LoadScene(nextIndex);
        //EntitiesDestroyed = false;
        _slider.enabled = true;
        _tipsTitle.enabled = true;
        _tipsContent.enabled = true;
        _btnUp.enabled = true;
        _btnDown.enabled = true;

        if (string.IsNullOrEmpty(SceneNameNext) == false)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(SceneNameNext);
        }
    }

    private int GetSceneDuration(string scenePath)
    {
        foreach (var scene in SceneConfigs)
        {
            if (!scenePath.EndsWith(scene.SceneName + ".unity"))
                continue;
            if (scene.CustomDuration <= 0)
                continue;
            return scene.CustomDuration;
        }

        return SceneSwitchInterval;
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    void Update()
    {
        Define.LoadingValue = value;
        if (_isLoad == false)
        {
            _intervalLoad += Time.deltaTime;
            if (_intervalLoad >= 1)
            {
                _isLoad = true;
                LoadNextScene();
            }

        }
        else
        {
            if (_asyncOperation.isDone)
            {
                if (_isLoadDone == false)
                {
                    _isLoadDone = true;
                    _slider.enabled = false;
                    _tipsTitle.enabled = false;
                    _tipsContent.enabled = false;
                    _btnUp.enabled = false;
                    _btnDown.enabled = false;
                    _mask.DOFade(0, 1);
                }
                    
               
            }

            if (_isLoadDone == true)
            {
                _intervalView += Time.deltaTime;

                if (_intervalView >= 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
        //TimeUntilNextSwitch -= Time.deltaTime;
        //if (TimeUntilNextSwitch > 0.0f)
        //    return;

        //if (!EntitiesDestroyed)
        //{
        //    DestroyAllEntitiesInScene();
        //}
        //else
        //{
        //    DestroyAllEntitiesInScene();
        //    LoadNextScene();
        //}
    }

    void LateUpdate()
    {
        _slider.value = Define.LoadingValue;

        if (_slider.value >= 1)
        {
            // UICenterMasterManager.Instance.DestroyWindow(this.ID);
            if (Callback != null)
            {
                Callback();
            }

        }
    }

    public float Schedule
    {
        get { return _slider.value; }
        set
        {
            if (!_slider)
                _slider = transform.Find("Slider").GetComponent<Slider>();

            _slider.value = value;
        }
    }

    /// <summary>
    /// 翻页
    /// </summary>
    private void ButtonUp()
    {
        _currentPage = _currentPage == 0 ? _maxPage - 1 : _currentPage - 1;
        ChangeTips();
    }
    /// <summary>
    /// 下页
    /// </summary>
    private void ButtonDown()
    {
        _currentPage = _currentPage == _maxPage - 1 ? 0 : _currentPage + 1;
        ChangeTips();
    }

    private void ChangeTips()
    {
        _tipsTitle.text = _tipsDatas[_currentPage].ContentTitle;
        _tipsContent.text = _tipsDatas[_currentPage].Content;
    }
    private void OnDisable()
    {
        base.StopCoroutine("rotation");
    }

    void OnDestroy()
    {
        _sequence.Kill();
    }
}
