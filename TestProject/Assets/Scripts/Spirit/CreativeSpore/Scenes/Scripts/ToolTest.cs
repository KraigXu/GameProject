using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTest : MonoBehaviour
{

    public TilemapGroup group;

    public RogueLikeMapGenerator rogueLike;

    public Button initBtn;


    // Start is called before the first frame update
    void Start()
    {
        initBtn = transform.GetChild(0).GetComponent<Button>();
        initBtn.onClick.AddListener(WorldInit);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void WorldInit()
    {
        rogueLike.GenerateMap();
    }
}
