using UnityEngine;
using System.Collections;

namespace CreativeSpore.SuperTilemapEditor
{
    


    public class RogueLikeMapGenerator : MonoBehaviour 
    {
        public int Altitude;  //海拔
        public int TemperatureMin=10;  //最低温度
        public int TemperatureMax=40;  //最高温度


        public string MapName;

        public string Seed;

        //------Define


        public float MapTimeYear=0;
        public float MapTimeMonth=0;
        public float MapTimeDay=0;
        public float MapTimeCell=0;
        
        public float MapWindDirection;
        public float MapWindPower;
        
        //-----Define

        public STETilemap Ground;
        public STETilemap GroundOverlay;

        public int Width = 300;
        public int Height = 300;

        uint tileWater = (8 << 16); //NOTE: this tile will be using the brush with id 8, because it's shifted 16 bits to the left.   //水
        uint tileWaterPlants = (24 << 16);                                //水
        uint tileDarkGrass = 66; // NOTE: this tile is using the tileId 66, because it's not shifted to the left   //草地
        uint tileGrass = (9 << 16);             //草地
        uint tileFlowers = (22 << 16);    //花
        uint tileMountains = (23 << 16);    //  山脉

        ////地面层
        
        void OnGUI()
        {
            if(GUI.Button(new Rect(20, 20, 100, 50), "Generate Map"))
            {
                GenerateMap();
            }
        }

        [ContextMenu("GenerateMap")]
	    public void GenerateMap()
        {

            uint test = 3<< 3;

            Debug.Log(test);

            char[] namechars=MapName.ToCharArray();

            string value="";
            for(int i = 0; i < namechars.Length; i++)
            {
                value += char.ConvertToUtf32(MapName, i).ToString();
            }

            Seed = value;

            Ground.ClearMap();
            GroundOverlay.ClearMap();

            float now;
            now = Time.realtimeSinceStartup;
            float fDiv = 25f;
            float xf = Random.value * 100;
            float yf = Random.value * 100;

            //*/ Rogue Demo (280ms with 180x180)

            InitModel(xf, yf, fDiv);

            Debug.Log("Generation time(ms): " + (Time.realtimeSinceStartup - now) * 1000);

            now = Time.realtimeSinceStartup;
            Ground.UpdateMesh();
            GroundOverlay.UpdateMesh();
            
            Debug.Log("UpdateMesh time(ms): " + (Time.realtimeSinceStartup - now) * 1000);
        }


        public void InitModel(float xf,float yf,float fDiv)
        {
         
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    float fRand = Random.value;
                    float noise = Mathf.PerlinNoise((i + xf) / fDiv, (j + yf) / fDiv);
                  //  Debug.Log("noise: " + noise + "; i: " + i + "; j: " + j);
                    if (noise < 0.1) //water
                    {
                        Ground.SetTileData(i, j, tileWater);
                    }
                    else if (noise < 0.2) // water plants
                    {
                        Ground.SetTileData(i, j, tileWater);
                        if (fRand < noise / 3)
                            GroundOverlay.SetTileData(i, j, tileWaterPlants);
                    }
                    else if (noise < 0.5 && fRand < (1 - noise / 2)) // dark grass
                    {
                        Ground.SetTileData(i, j, tileDarkGrass);
                    }
                    else if (noise < 0.6 && fRand < (1 - 1.2 * noise)) // flowers
                    {
                        Ground.SetTileData(i, j, tileGrass);
                        GroundOverlay.SetTileData(i, j, tileFlowers);
                    }
                    else if (noise < 0.7) // grass
                    {
                        Ground.SetTileData(i, j, tileGrass);
                    }
                    else // mountains
                    {
                        Ground.SetTileData(i, j, tileGrass);
                        GroundOverlay.SetTileData(i, j, tileMountains);
                    }
                 //   GroundOverlay.SetTileData(i, j, tileMountains);
                }
            }
        }

        public void InitWorld(float xf, float yf, float fDiv)
        {

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    float fRand = Random.value;
                    float noise = Mathf.PerlinNoise((i + xf) / fDiv, (j + yf) / fDiv);

                    if (noise < 0.1) //water
                    {
                        Ground.SetTileData(i, j, tileWater);
                    }
                    else if (noise < 0.2) // water plants
                    {
                        Ground.SetTileData(i, j, tileWater);
                        if (fRand < noise / 3)
                            GroundOverlay.SetTileData(i, j, tileWaterPlants);
                    }
                    else if (noise < 0.5 && fRand < (1 - noise / 2)) // dark grass
                    {
                        Ground.SetTileData(i, j, tileDarkGrass);
                    }
                    else if (noise < 0.6 && fRand < (1 - 1.2 * noise)) // flowers
                    {
                        Ground.SetTileData(i, j, tileGrass);
                        GroundOverlay.SetTileData(i, j, tileFlowers);
                    }
                    else if (noise < 0.7) // grass
                    {
                        Ground.SetTileData(i, j, tileGrass);
                    }
                    else // mountains
                    {
                        Ground.SetTileData(i, j, tileGrass);
                        GroundOverlay.SetTileData(i, j, tileMountains);
                    }

                   
                }
            }


        }
    }
}
