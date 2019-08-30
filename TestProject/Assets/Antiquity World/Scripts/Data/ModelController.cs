using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;

/// <summary>
/// 模型控制
/// </summary>
public class ModelController : MonoBehaviour
{

    private static ModelController _instance;

    public List<ModelFileData> ModelFileDatas=new List<ModelFileData>();

    /// <summary>
    /// 所有已经加载的Render
    /// </summary>
    public List<Renderer> AllRenderers=new List<Renderer>();

    [SerializeField]
    private LayerMask _layer;
    [SerializeField]
    private Camera _camera;

    public static ModelController Instance
    {
        get { return _instance; }
    }
    void Awake()
    {
        _instance = this;
        
    }
    //下载
    public IEnumerator ReadModelFileData()
    {

        AllRenderers.Clear();

        for (int i = 0; i < ModelFileDatas.Count; i++)
        {
            GameObject parentNode = new GameObject();
            parentNode.transform.SetParent(transform);
            parentNode.layer = 19;

            Renderer renderer =Instantiate(Resources.Load<GameObject>(ModelFileDatas[i].Path)).GetComponent<Renderer>();
            renderer.gameObject.layer = 19;
            renderer.transform.SetParent(parentNode.transform);

            AllRenderers.Add(renderer);

        }

        yield return null;
    }




}
