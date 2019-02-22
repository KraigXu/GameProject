using System.Collections;
using UnityEngine;
using UnityEditor;

public class DynamicCreateTerrain : MonoBehaviour
{
    public TerrainData terrainData;
    public float[,] heightsBackups;

    void Start()
    {
        CreateTerrain();
        //var terrain = CreateTerrain();
        ModifyTerrainDataHeight(terrainData);
        // 5秒后恢复地形
        StartCoroutine(Disable());
    }

    // 动态创建地形
    public Terrain CreateTerrain()
    {
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 513;
        terrainData.baseMapResolution = 513;
        terrainData.size = new Vector3(50, 50, 50);
        terrainData.alphamapResolution = 512;
        terrainData.SetDetailResolution(32, 8);
        GameObject obj = Terrain.CreateTerrainGameObject(terrainData);
        AssetDatabase.CreateAsset(terrainData, "Assets/Terrain_ModifyHeight.asset");
        AssetDatabase.SaveAssets();
        return obj.GetComponent<Terrain>();
    }

    // 动态改变地形
    public void ModifyTerrainDataHeight(TerrainData terrainData)
    {
        int width = terrainData.heightmapWidth;
        int height = terrainData.heightmapHeight;
        float[,] array = new float[width, height];
        print("width:" + width + " height:" + height);
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                float f1 = i;
                float f2 = width;
                float f3 = j;
                float f4 = height;
                float baseV = (f1 / f2 + f3 / f4) / 2 * 1;
                array[i, j] = baseV * baseV;
            }
        // 备份高度图
        heightsBackups = terrainData.GetHeights(0, 0, width, height);
        // 设置高度图
        terrainData.SetHeights(0, 0, array);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Recove Terrain.");
        terrainData.SetHeights(0, 0, heightsBackups);
    }
}