using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntiquityWorld
{
    public static class WXSceneManager
    {

        public static void Load(string sceneName)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("SceneSwitcher/LoadCanvas"));
            SceneSwitcher ss= go.GetComponent<SceneSwitcher>();
            ss.SceneNameNext = sceneName;
        }

    }
}

