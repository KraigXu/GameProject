using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

namespace GameSystem.Ui
{
    public class UGUITools : MonoBehaviour
    {


        /// <summary>
        /// Save the specified binary data into the specified file.
        /// </summary>

        static public bool Save(string fileName, byte[] bytes)
        {
#if UNITY_WEBPLAYER || UNITY_FLASH || UNITY_METRO || UNITY_WP8 || UNITY_WP_8_1
        return false;
#else

            string path = Application.persistentDataPath + "/" + fileName;

            if (bytes == null)
            {
                //  if (File.Exists(path)) File.Delete(path);
                return false;
            }

            FileStream file = null;

            try
            {
                file = File.Create(path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }

            file.Write(bytes, 0, bytes.Length);
            file.Close();
            return true;
#endif
        }

        /// <summary>
        /// Load all binary data from the specified file.
        /// </summary>

        static public byte[] Load(string fileName)
        {
#if UNITY_WEBPLAYER || UNITY_FLASH || UNITY_METRO || UNITY_WP8 || UNITY_WP_8_1
        return null;
#else

            // string path = Application.persistentDataPath + "/" + fileName;

            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            return null;
#endif
        }



        /// <summary>
        /// Get the rootmost object of the specified game object.
        /// </summary>

        static public GameObject GetRoot(GameObject go)
        {
            Transform t = go.transform;

            for (; ; )
            {
                Transform parent = t.parent;
                if (parent == null) break;
                t = parent;
            }

            return t.gameObject;
        }

        /// <summary>
        /// Instantiate an object and add it to the specified parent.
        /// </summary>

        static public GameObject AddChild(GameObject parent, GameObject prefab)
        {
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.SetParent(parent.transform);
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        /// <summary>
        /// 将child 作为子物体移动到parent下
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Child"></param>
        static public void ChangeParent(GameObject Parent, GameObject Child)
        {
            Child.transform.SetParent(Parent.transform);
            Child.transform.localPosition = Vector3.zero;
            Child.transform.localRotation = Quaternion.identity;
            Child.transform.localScale = Vector3.one;
            Child.layer = Parent.layer;
        }

        static public GameObject AddUnderlineByText(GameObject parent, GameObject prefab,
            UIEventTriggerListener.VoidDelegate _Click)
        {
            GameObject res = Resources.Load("Prefabs/MouseBlock") as GameObject;

            GameObject go = AddChild(parent, prefab);
            GameObject mouseBlock = AddChild(go, res); //刷入的是换鼠标的碰撞
            Text _underline = go.GetComponent<Text>();
            Text parenttext = parent.GetComponent<Text>();
            int length = parenttext.text.Length;
            _underline.text = "";
            for (int i = 0; i < length * 2; i++)
            {
                _underline.text += "_";
            }
            //Debug.Log(" _underline.flexibleHeight" + _underline.flexibleHeight);

            TextGenerator m_TextGenerator = _underline.cachedTextGeneratorForLayout;
            TextGenerationSettings m_TextGenerationSettings = _underline.GetGenerationSettings(Vector2.zero);
            float fWidth = m_TextGenerator.GetPreferredWidth(_underline.text, m_TextGenerationSettings);

            m_TextGenerationSettings =
                _underline.GetGenerationSettings(new Vector2(_underline.rectTransform.rect.x, 0.0f));
            float fHeight = m_TextGenerator.GetPreferredHeight(_underline.text, m_TextGenerationSettings);
            mouseBlock.GetComponent<MouseImageControl>()._callback = _Click;
            UGUIGraphControl.SetTransformSize(mouseBlock, new Vector2(fWidth, fHeight));
            //mouseBlock.GetComponent<Rect>()
            _underline.rectTransform.anchorMin = Vector2.zero;
            _underline.rectTransform.anchorMax = Vector2.one;
            _underline.rectTransform.offsetMax = Vector2.zero;
            _underline.rectTransform.offsetMin = Vector2.zero;
            return go;
        }

        //IEnumerator WaitLabelInit(GameObject go,Text _text)
        //{
        //    yield return new WaitForEndOfFrame();

        //}
        /// <summary>
        /// 给label添加下划线
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        static public GameObject AddUnderlineByText(GameObject label, UIEventTriggerListener.VoidDelegate _Click)
        {
            return AddUnderlineByText(label, label, _Click);
        }

        static public GameObject AddInterface(GameObject parent, string path)
        {
            //将 界面 作为子物体 放到parent 下

            GameObject prefab = Resources.Load(path) as GameObject;
            GameObject go = AddChild(parent, prefab);
            go.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            prefab = null;
            return go;
        }

        static public void DestoryAllChild(GameObject Parent)
        {
            //删除所有子物体
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                Destroy(Parent.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 删除 某个物体底下 除了名为 OneChildName之外 的所有子物体
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="OneChild"></param>
        static public void DestoryChildExceptOneChild(GameObject Parent, string OneChildName)
        {
            for (int i = 0; i < Parent.transform.childCount; i++)
            {
                GameObject ChildObj = Parent.transform.GetChild(i).gameObject;
                if (ChildObj.name != OneChildName)
                {
                    Destroy(ChildObj);
                }
            }
        }

        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public bool IsNumberCheck(string str, out int value)
        {
            try
            {
                value = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                value = -1;
                return false;
            }

        }


        /// <summary>
        /// 根据后缀判断是否为 图片
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsImageCheck(string str)
        {
            bool IsPhoto = false;

            if (str == "bmp" || str == "tif" || str == "iff" || str == "cpx" || str == "dwg" || str == "eps" ||
                str == "gif" || str == "ico" || str == "peg" || str == "pm5" || str == "jpg" || str == "png")
            {
                IsPhoto = true; //png
            }

            return IsPhoto;
        }

        /// <summary>
        /// 根据后缀判断是否为 Excel 文件
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsExcelCheck(string str)
        {
            bool isExcel = false;
            if (str == "xlsx" || str == "xls")
            {
                isExcel = true;
            }

            return isExcel;
        }

        /// <summary>
        /// 根据后缀判断是否为 Excel 文件
        /// </summary>
        /// <param name="str">后缀后三位</param>
        /// <returns></returns>
        static public bool IsWordCheck(string str)
        {
            bool isWord = false;
            if (str == "doc" || str == "docx")
            {
                isWord = true;
            }

            return isWord;
        }

        /// <summary>
        /// 添加子节点
        /// Add child to target
        /// </summary>
        public static void AddChildToTarget(Transform target, Transform child)
        {
            child.SetParent(target);
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;

            ChangeChildLayer(child, target.gameObject.layer);
        }

        /// <summary>
        /// 修改子节点Layer  NGUITools.SetLayer();
        /// Change child layer
        /// </summary>
        public static void ChangeChildLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChildLayer(child, layer);
            }
        }

        /// <summary>
        /// 获得指定目标最大depth值
        /// Get the target Max depth
        /// </summary>
        public static int GetMaxTargetDepth(GameObject obj, bool includeInactive = false)
        {
            int minDepth = -1;
            //List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
            //if (lsPanels != null)
            //    return lsPanels[lsPanels.Count - 1].depth;
            return minDepth;
        }

        public static void SetTargetMinPanelDepth(GameObject obj, int depth)
        {
            //List<UIPanel> lsPanels = GameUtility.GetPanelSorted(obj, true);
            //if (lsPanels != null)
            //{
            //    int i = 0;
            //    while (i < lsPanels.Count)
            //    {
            //        lsPanels[i].depth = depth + i;
            //        i++;
            //    }
            //}
        }

        // <summary>
        /// 给目标添加Collider背景
        /// Add Collider Background for target
        /// </summary>
        public static GameObject AddColliderBgToTarget(GameObject target, string maskNames, bool isTransparent)
        {
            // 添加UIPaneldepth最小上面
            // 保证添加的Collider放置在屏幕中间
            Transform windowBg = UGUITools.FindDeepChild(target, "WindowBg");
            if (windowBg == null)
            {
                //GameObject targetParent = GameUtility.GetPanelDepthMaxMin(target, false, true);
                //if (targetParent == null)
                //    targetParent = target;

                GameObject temp = new GameObject("WindowBg");
                UGUITools.AddChildToTarget(target.transform, temp.transform);
                RectTransform rt = temp.AddComponent<RectTransform>();
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.anchorMin = Vector2.zero;
                windowBg = rt.transform;
                windowBg.SetAsFirstSibling();
            }

            Transform Masker = UGUITools.FindDeepChild(target, "WindowColliderBg(Cool)");
            if (Masker == null)
            {

                Transform root = UICenterMasterManager.Instance.GetTargetRoot(UIWindowType.ForegroundLayer);
                GameObject res = Resources.Load("UIPrefab/Mask/" + maskNames) as GameObject;
                Masker = UGUITools.AddChild(root.gameObject, res).transform;
                Masker.name = "WindowColliderBg(Cool)";
                UGUIGraphControl.SetRectTransformOffset(Masker.gameObject, Vector2.zero, Vector2.zero);
                res = null;
                UGUITools.AddChildToTarget(windowBg, Masker.transform);

                if (isTransparent)
                {
                    Masker.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.1f);
                }
                else
                {
                    Masker.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.5f);
                }


                // add sprite or widget to ColliderBg
                //UIWidget widget = null;
                //if (!isTransparent)
                //    widget = NGUITools.AddSprite(windowBg.gameObject, altas, maskName);
                //else
                //    widget = NGUITools.AddWidget<UIWidget>(windowBg.gameObject);

                //widget.name = "WindowColliderBg(Cool)";
                //bg = widget.transform;

                //// fill the screen
                //// You can use the new Anchor system
                //UIStretch stretch = bg.gameObject.AddComponent<UIStretch>();
                //stretch.style = UIStretch.Style.Both;
                //// set relative size bigger
                //stretch.relativeSize = new Vector2(1.5f, 1.5f);

                //// set a lower depth
                //widget.depth = -5;

                //// set alpha
                //widget.alpha = 0.6f;

                //// add collider
                //NGUITools.AddWidgetCollider(bg.gameObject);
            }

            return Masker.gameObject;
        }

        /// <summary>
        /// Find Deep child with name
        /// </summary>
        public static Transform FindDeepChild(GameObject _target, string _childName)
        {
            Transform resultTrs = null;
            resultTrs = _target.transform.Find(_childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in _target.transform)
                {
                    resultTrs = UGUITools.FindDeepChild(trs.gameObject, _childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }

            return resultTrs;
        }

        /// <summary>
        /// Find component in Target Child
        /// </summary>
        public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
        {
            Transform resultTrs = UGUITools.FindDeepChild(_target, _childName);
            if (resultTrs != null)
                return resultTrs.gameObject.GetComponent<T>();
            return (T)((object)null);
        }

    }
}